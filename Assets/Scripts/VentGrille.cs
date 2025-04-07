using UnityEngine;

namespace MrLucy
{
    [RequireComponent(typeof(BoxCollider), typeof(AudioSource))]
    public class VentGrille : GameStateListener
    {
        [SerializeField] private AudioClip _dropAudioClip;
        private Rigidbody _rigidbody;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        protected override void OnGameStateChanged(GameState state)
        {
            if (state == GameState.ElevatorStuck)
            {
                _rigidbody = gameObject.AddComponent<Rigidbody>();
                _rigidbody.angularVelocity = new Vector3(0f, 0f, 5f);
                _audioSource.PlayOneShot(_dropAudioClip);
            }
        }
    }
}