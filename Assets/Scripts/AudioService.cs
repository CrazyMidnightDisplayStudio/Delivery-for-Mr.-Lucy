using UnityEngine;
using UnityEngine.Audio;

namespace MrLucy
{
    public class AudioService : Singleton<AudioService>
    {
        [SerializeField] private AudioMixer mixer;
    
        private AudioMixerGroup _masterGroup;
        private int _volume = 80;
        private float _lastInputTime;
        private const float INPUT_COOLDOWN = 0.5f;

        protected override void Awake()
        {
            base.Awake();
            _masterGroup = mixer.FindMatchingGroups("Master")[0];
            DontDestroyOnLoad(this);
            SetMasterVolume(_volume);
        }

        private void Update()
        {
            if (Time.time - _lastInputTime < INPUT_COOLDOWN) return;

            bool volumeChanged = false;

            if (Input.GetKey(KeyCode.Minus))
            {
                _volume = Mathf.Clamp(_volume - 10, 0, 100);
                volumeChanged = true;
            }
            else if (Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.Plus))
            {
                _volume = Mathf.Clamp(_volume + 10, 0, 100);
                volumeChanged = true;
            }

            if (volumeChanged)
            {
                SetMasterVolume(_volume);
                _lastInputTime = Time.time;
            }
        }

        public void SetMasterVolume(int volumePercent)
        {
            _volume = Mathf.Clamp(volumePercent, 0, 100);
            float dB = Mathf.Lerp(-80f, 0f, _volume / 100f);
            Debug.Log(dB);
            Debug.Log(_volume);
            _masterGroup.audioMixer.SetFloat("MasterVolume", dB);
        }
    }
}