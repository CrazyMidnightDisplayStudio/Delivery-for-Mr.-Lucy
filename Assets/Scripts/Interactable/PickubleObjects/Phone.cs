using UnityEngine;

namespace MrLucy
{
    public class Phone : BaseInteractableObject, IPickupObject
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private bool canBePickedUp = true;

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
            // TODO
        }

        public GameObject GetPickupPrefab()
        {
            if (CanBePickedUp)
            {
                return prefab;
            }

            return null;
        }
    }
}