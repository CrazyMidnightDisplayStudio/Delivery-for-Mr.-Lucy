using UnityEngine;

namespace MrLucy
{
    public class ElevatorButton : BaseInteractableObject
    {
        public int buttonNumber;
        private ButtonPressAnimation _pressAnimation;

        protected override void Awake()
        {
            base.Awake();
            _pressAnimation = gameObject.AddComponent<ButtonPressAnimation>();
        }
        
        public override void Interact()
        {
            _pressAnimation.PushButton();
            if (!isInteractActive) return;
            Debug.Log($"Interacting with ElevatorButton {buttonNumber}");
        }

        protected override void OnGameStateChanged(GameState state)
        {
            // nothing
        }
    }
}