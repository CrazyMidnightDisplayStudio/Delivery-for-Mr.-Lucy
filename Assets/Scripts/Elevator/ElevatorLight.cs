using System.Collections;
using UnityEngine;

namespace MrLucy
{
    public class ElevatorLight : MonoBehaviour
    {
        [SerializeField] private Light[] elevatorLight; // обычный Unity Light
        [SerializeField] private Renderer[] lightedSurfaces; // объекты, чьи материалы имитируют освещение
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color warningColor = Color.red;

        private Coroutine _blinkingRoutine;
        private Coroutine _pulseRoutine;

        public void TurnOn()
        {
            SetLightActive(true);
        }

        public void TurnOff()
        {
            SetLightActive(false);
        }

        public void SetLightColor(Color color)
        {
            foreach (var light in elevatorLight)
            {
                light.color = color;
            }

            foreach (var r in lightedSurfaces)
            {
                r.material.SetColor("_EmissionColor", color * 1.5f);
            }
        }

        public void SetLightIntensity(float intensity)
        {
            foreach (var light in elevatorLight)
            {
                light.intensity = intensity;
            }
        }

        public void StartBlinking(float duration)
        {
            if (_blinkingRoutine == null)
                _blinkingRoutine = StartCoroutine(BlinkRoutine(duration));
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
            foreach (var light in elevatorLight)
            {
                light.enabled = isOn;
            }

            foreach (var r in lightedSurfaces)
            {
                r.material.EnableKeyword("_EMISSION");
                r.material.SetColor("_EmissionColor", isOn ? normalColor * 1.5f : Color.black);
            }
        }

        public void SetWarningMode(bool enabled)
        {
            if (_pulseRoutine != null)
            {
                StopCoroutine(_pulseRoutine);
                _pulseRoutine = null;
            }

            if (enabled)
            {
                _pulseRoutine = StartCoroutine(PulseWarningLight());
            }
            else
            {
                foreach (var light in elevatorLight)
                {
                    light.color = normalColor;
                    light.intensity = 1f;
                }
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


        private IEnumerator PulseWarningLight()
        {
            SetLightColor(warningColor);

            while (true)
            {
                // Пульсация синусом
                float t = (Mathf.Sin(Time.time * 2f) + 1f) / 2f;
                SetLightIntensity(Mathf.Lerp(0.5f, 2f, t));
                yield return null;
            }
        }
    }
}