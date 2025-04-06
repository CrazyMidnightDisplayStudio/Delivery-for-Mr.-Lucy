using UnityEngine;
using UnityEngine.Audio;

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
            
            _audioSource.outputAudioMixerGroup = AudioManager.Instance.audioMixer.FindMatchingGroups("Sounds")[0];
        }
        
        public override void Interact()
        {
            _pressAnimation.PushButton();
            _audioSource.Play();
            if (!isInteractActive) return;
            Debug.Log($"Interacting with ElevatorButton {buttonNumber}");
        }

        protected override void OnGameStateChanged(GameState state)
        {
            // nothing
        }
    }
}