using UnityEngine;

namespace MrLucy
{
    public class RedButton : ElevatorButton, IPickupObject
    {
        [SerializeField] private Transform positionOnPanel;
        Rigidbody _rigidbody;
        private bool _isFired;

        public bool CanBePickedUp => _isFired;

        protected override void Awake()
        {
            base.Awake();
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = true;
        }

        public void Fire()
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(transform.forward * 200f + transform.up * 50f);
            _isFired = true;
            _pressAnimation.active = false;
        }

        public override void Interact()
        {
            base.Interact();

            if (_isFired)
            {
                GameManager.Instance.GetHandSlot().TryPickUp(this, () => GameManager.Instance.SetState(GameState.WaitForRedButtonReceive));
            }

            if (GameManager.Instance.CurrentState == GameState.WaitSpaceButton)
            {
                DialogueSystem.Instance.PrintSingleMessage("Jump!");
            }
        }

        public void ReturnToPanel()
        {
            _pressAnimation.active = true;
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            _isFired = false;
        }

        public GameObject GetPickupPrefab()
        {
            return gameObject;
        }
    }
}