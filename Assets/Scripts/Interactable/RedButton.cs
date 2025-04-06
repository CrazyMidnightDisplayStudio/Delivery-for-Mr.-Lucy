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
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = true;
        }

        public void Fire()
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(transform.forward * 200f + transform.up * 50f);
            _isFired = true;
        }

        public override void Interact()
        {
            base.Interact();

            if (_isFired)
            {
                _rigidbody.isKinematic = true;
                _rigidbody.velocity = Vector3.zero;
                GameManager.Instance.GetHandSlot().TryPickUp(GetPickupPrefab());
            }
        }

        public void ReturnToPanel()
        {
            _rigidbody.isKinematic = true;
            transform.SetParent(positionOnPanel);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            _isFired = false;
        }

        public GameObject GetPickupPrefab()
        {
            return gameObject;
        }
    }
}