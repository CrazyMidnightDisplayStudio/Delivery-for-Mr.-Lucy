using NUnit.Framework.Constraints;
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
                Destroy(gameObject);
            }
        }

        public override void Interact()
        {
            GameManager.Instance.GetHandSlot().TryPickUp(this);
        }

        public GameObject GetPickupPrefab() => gameObject;
    }
}