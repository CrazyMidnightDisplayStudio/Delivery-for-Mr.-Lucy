using UnityEngine;

namespace MrLucy
{
    public class ElevatorButton : BaseInteractableObject
    {
        public int buttonNumber;
        
        public override void Interact()
        {
            Debug.Log($"Interacting with ElevatorButton {buttonNumber}");
        }
    }
}