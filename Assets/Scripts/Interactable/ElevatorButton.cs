using UnityEngine;

namespace MrLucy
{
    public class ElevatorButton : BaseInteractableObject
    {
        public int buttonNumber;
        protected ButtonPressAnimation _pressAnimation;

        private AudioSource _audioSource;
        private AudioClip _buttonPressSound;

        protected override void Awake()
        {
            base.Awake();
            _pressAnimation = gameObject.AddComponent<ButtonPressAnimation>();
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _buttonPressSound = Resources.Load<AudioClip>("Audio/Sounds/elevatorButton");
            _audioSource.clip = _buttonPressSound;

            var groups = AudioManager.Instance?.audioMixer?.FindMatchingGroups("Sounds");
            if (groups != null && groups.Length > 0)
            {
                _audioSource.outputAudioMixerGroup = groups[0];
            }
            else
            {
                Debug.LogWarning("AudioManager или микшер не готов");
            }
        }

        public override void Interact()
        {
            _pressAnimation.PushButton();
            _audioSource.Play();
            GameManager.Instance.code.AddDigit(buttonNumber);
            if (!isInteractActive) return;
            Debug.Log($"Interacting with ElevatorButton {buttonNumber}");
        }

        protected override void OnGameStateChanged(GameState state)
        {
            // nothing
        }
    }
}