using UnityEngine;

namespace MrLucy
{
    public class ElevatorButton : BaseInteractableObject
    {
        public int buttonNumber;
        
        public override void Interact()
        {
            if (!isActive) return;
            Debug.Log($"Interacting with ElevatorButton {buttonNumber}");
        }

        protected override void OnGameStateChanged(GameState state)
        {
            // nothing
        }
    }
}