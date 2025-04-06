using UnityEngine;

namespace MrLucy
{
    public class Phone : BaseInteractableObject, IPickupObject
    {
        [SerializeField] private bool canBePickedUp = true;
        [SerializeField] private Light flashlight;

        public bool IsLightOn
        {
            get => flashlight.enabled;
            set => flashlight.enabled = value;
        }

        private void Start()
        {
            IsLightOn = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                IsLightOn = !IsLightOn;
            }
        }

        public bool CanBePickedUp
        {
            get => canBePickedUp;
            protected set => canBePickedUp = value;
        }

        protected override void OnGameStateChanged(GameState state)
        {
            // nothing
        }

        public override void Interact()
        {
            // nothing
        }

        public GameObject GetPickupPrefab()
        {
            if (CanBePickedUp)
            {
                return gameObject;
            }

            return null;
        }
    }
}