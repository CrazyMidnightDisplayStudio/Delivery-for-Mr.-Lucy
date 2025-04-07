using UnityEngine;
using UnityEngine.Audio;

namespace MrLucy
{
    public class AudioService : Singleton<AudioService>
    {
        [SerializeField] private AudioMixer mixer;

        private AudioMixerGroup _masterGroup;
        private int _volume = 60;

        protected override void Awake()
        {
            base.Awake();
            _masterGroup = mixer.FindMatchingGroups("Master")[0];
            DontDestroyOnLoad(this);
            SetMasterVolume(_volume);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Minus))
            {
                _volume = Mathf.Clamp(_volume - 5, 0, 100);
                SetMasterVolume(_volume);
            }

            if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Plus))
            {
                _volume = Mathf.Clamp(_volume + 5, 0, 100);
                SetMasterVolume(_volume);
            }
        }

        public void SetMasterVolume(int volumePercent)
        {
            _volume = Mathf.Clamp(volumePercent, 0, 100);

            float dB = Mathf.Lerp(-80f, 0f, _volume / 100f);
            _masterGroup.audioMixer.SetFloat("MasterVolume", dB);
        }
    }
}