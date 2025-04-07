using UnityEngine;

namespace MrLucy
{
    public class RedButtonSlot : BaseInteractableObject
    {
        private Collider _collider;

        protected override void Awake()
        {
            base.Awake();
            _collider = GetComponent<Collider>();
        }

        protected override void OnGameStateChanged(GameState state)
        {
            isInteractActive = (state == GameState.WaitForRedButtonReceive || state == GameState.RedButtonFired);
            _collider.enabled = isInteractActive;
            if (isInteractActive)
            {
                gameObject.layer = LayerMask.NameToLayer("Interactable");
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }

        public override void Interact()
        {
            if (GameManager.Instance.CurrentState == GameState.RedButtonFired)
            {
                GameManager.Instance.ButtonDialogue();
            }
            
            if (GameManager.Instance.CurrentState == GameState.WaitForRedButtonReceive)
            {
                GameManager.Instance.GetHandSlot().ReceiveItem(transform, droppedItem =>
                {
                    if (droppedItem != null && droppedItem.TryGetComponent(out RedButton redButton))
                    {
                        redButton.ReturnToPanel();
                    }

                    GameManager.Instance.SetState(GameState.FirstCallRedButton);
                });
            }
        }
    }
}