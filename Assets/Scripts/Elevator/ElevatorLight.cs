using System.Collections;
using UnityEngine;

namespace MrLucy
{
    public class ElevatorLight : MonoBehaviour
    {
        [SerializeField] private Light elevatorLight; // обычный Unity Light
        [SerializeField] private Renderer[] lightedSurfaces; // объекты, чьи материалы имитируют освещение
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color warningColor = Color.red;

        private Coroutine _blinkingRoutine;

        public void TurnOn()
        {
            SetLightActive(true);
        }

        public void TurnOff()
        {
            SetLightActive(false);
        }

        public void SetWarningMode(bool enabled)
        {
            SetLightColor(enabled ? warningColor : normalColor);
        }

        public void StartBlinking(float interval = 0.3f)
        {
            if (_blinkingRoutine == null)
                _blinkingRoutine = StartCoroutine(BlinkRoutine(interval));
        }

        public void StopBlinking()
        {
            if (_blinkingRoutine != null)
            {
                StopCoroutine(_blinkingRoutine);
                _blinkingRoutine = null;
                SetLightActive(true); // вернём свет обратно
            }
        }

        private void SetLightActive(bool isOn)
        {
            if (elevatorLight != null)
                elevatorLight.enabled = isOn;

            foreach (var r in lightedSurfaces)
            {
                r.material.EnableKeyword("_EMISSION");
                r.material.SetColor("_EmissionColor", isOn ? normalColor * 1.5f : Color.black);
            }
        }

        private void SetLightColor(Color color)
        {
            if (elevatorLight != null)
                elevatorLight.color = color;

            foreach (var r in lightedSurfaces)
            {
                r.material.SetColor("_EmissionColor", color * 1.5f);
            }
        }

        private IEnumerator BlinkRoutine(float duration)
        {
            float timer = 0f;
            bool state = true;

            while (timer < duration)
            {
                SetLightActive(state);
                state = !state;

                float interval = Random.Range(0.05f, 0.2f);
                yield return new WaitForSeconds(interval);
                timer += interval;
            }

            // По завершению — выключить свет
            SetLightActive(false);
        }
    }
}