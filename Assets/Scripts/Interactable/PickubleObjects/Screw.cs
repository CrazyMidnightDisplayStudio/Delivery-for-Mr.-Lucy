using UnityEngine;

namespace MrLucy
{
    public class Screw : BaseInteractableObject, IPickupObject
    {
        public bool CanBePickedUp { get; }

        protected override void OnGameStateChanged(GameState state)
        {
            isInteractActive = state == GameState.ElevatorStuck;
            if (state == GameState.TheHatchIsOpened)
            {
                GameManager.Instance.GetHandSlot().DropItem();
            }
        }

        public override void Interact()
        {
            GameManager.Instance.GetHandSlot().TryPickUp(this);
        }

        public GameObject GetPickupPrefab() => gameObject;
    }
}