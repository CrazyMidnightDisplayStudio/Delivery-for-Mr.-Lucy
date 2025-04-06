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
            isInteractActive = (state == GameState.WaitForRedButtonReceive);
            _collider.enabled = isInteractActive;
        }

        public override void Interact()
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