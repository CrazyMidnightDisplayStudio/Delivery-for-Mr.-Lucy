using System.Collections;
using Cinemachine;
using UnityEngine;

namespace MrLucy
{
    [RequireComponent(typeof(MeshRenderer), typeof(AudioSource))]
    public class Ebaka : BaseInteractableObject
    {
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float moveDuration = 1f;
        [SerializeField] private AudioClip ebakaSound;
        [SerializeField] private CinemachineVirtualCamera vcam;
        [SerializeField] private float targetFOV = 15f;

        private Coroutine fovRoutine;
        private AudioSource _audioSource;
        private MeshRenderer _meshRenderer;
        private float _normalFOV;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshRenderer.enabled = false;
            _normalFOV = vcam.m_Lens.FieldOfView;
        }

        public override void Interact()
        {
            Debug.Log("Ebaka: Interact");
            if (!isInteractActive) return;

            GameManager.Instance.SetBlackoutActive(false);
            isInteractActive = false;
            _audioSource.clip = ebakaSound;
            _audioSource.Play();
            fovRoutine = StartCoroutine(FocusOnEbaka());
            StartCoroutine(ScurryAway());
        }

        public void ShowEbaka()
        {
            _meshRenderer.enabled = true;
        }

        private IEnumerator ScurryAway()
        {
            yield return new WaitForSeconds(1f);

            float timer = 0f;

            while (timer < moveDuration)
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                timer += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(1f);

            _meshRenderer.enabled = false;
        }

        protected override void OnGameStateChanged(GameState state)
        {
            isInteractActive = state == GameState.EbakaState;
        }

        private IEnumerator FocusOnEbaka()
        {
            var originalFOV = vcam.m_Lens.FieldOfView;
            vcam.LookAt = transform;

            float duration = 0.2f;
            float timer = 0f;

            // Плавно уменьшаем FOV до targetFOV
            while (timer < duration)
            {
                vcam.m_Lens.FieldOfView = Mathf.Lerp(originalFOV, targetFOV, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }

            vcam.m_Lens.FieldOfView = targetFOV;

            yield return new WaitForSeconds(0.2f);

            timer = 0f;
            while (timer < duration)
            {
                vcam.m_Lens.FieldOfView = Mathf.Lerp(targetFOV, _normalFOV, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }

            vcam.m_Lens.FieldOfView = _normalFOV;

            vcam.LookAt = null;

            GameManager.Instance.SetState(GameState.EnteredTheCode);
        }
    }
}