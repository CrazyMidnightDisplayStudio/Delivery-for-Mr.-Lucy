using UnityEngine;

namespace MrLucy
{
    [RequireComponent(typeof(MeshCollider), typeof(AudioSource))]
    public class Hatch : BaseInteractableObject
    {
        [SerializeField] private AudioClip hatchDropSound;
        private HandSlot _handSlot;
        private AudioSource _audioSource;

        protected override void Awake()
        {
            base.Awake();
            var gm = GameManager.Instance;
            _handSlot = gm.GetHandSlot();
            _audioSource = GetComponent<AudioSource>();
        }

        protected override void OnGameStateChanged(GameState state)
        {
            isInteractActive = state == GameState.ElevatorStuck;
        }

        public override void Interact()
        {
            if (!_handSlot.Empty)
            {
                if (_handSlot.currentItem.TryGetComponent(out Screw screw))
                {
                    gameObject.AddComponent<Rigidbody>();
                    GameManager.Instance.SetState(GameState.TheHatchIsOpened);
                }

                if (hatchDropSound && _audioSource)
                {
                    _audioSource.PlayOneShot(hatchDropSound);
                }
                else
                {
                    Debug.LogWarning("Hatch sound not played");
                }
            }
        }
    }
}