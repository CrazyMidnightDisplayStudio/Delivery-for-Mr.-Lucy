using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MrLucy
{
    [RequireComponent(typeof(AudioSource))]
    public class Final : MonoBehaviour
    {
        [SerializeField] AudioClip _horrorSound;
        [SerializeField] private RectTransform _uiPanel;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _uiPanel.gameObject.SetActive(false);
        }

        public void Run()
        {
            Debug.Log("Final run");
            StartCoroutine(RunSequence());
        }

        private IEnumerator RunSequence()
        {
            _audioSource.PlayOneShot(_horrorSound);
            yield return StartCoroutine(SmoothSlideInFromTop());
            yield return new WaitForSeconds(5f);
            LoadNextScene();
        }

        private void LoadNextScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                nextSceneIndex = 0;
            }

            SceneManager.LoadScene(nextSceneIndex);
        }

        private IEnumerator SmoothSlideInFromTop()
        {
            float screenHeight = Screen.height;
            Vector2 startPos = new Vector2(0f, screenHeight); // выше экрана
            Vector2 targetPos = Vector2.zero; // позиция (0, 0)

            _uiPanel.anchoredPosition = startPos;

            float duration = 1f;
            float elapsed = 0f;

            _uiPanel.gameObject.SetActive(true);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                _uiPanel.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
                yield return null;
            }

            _uiPanel.anchoredPosition = targetPos;
        }
    }
}