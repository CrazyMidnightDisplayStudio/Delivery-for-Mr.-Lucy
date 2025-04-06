using UnityEngine;
using UnityEngine.Audio;

namespace MrLucy
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        public AudioMixer audioMixer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            if (audioMixer == null)
            {
                audioMixer = Resources.Load<AudioMixer>("Audio/AudioMixer");
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}