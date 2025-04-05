using UnityEngine;

namespace MrLucy
{
    [RequireComponent(typeof(Collider))]
    public abstract class BaseInteractableObject : GameStateListener
    {
        public bool isActive = true;

        private const int InteractableLayer = 6;

        protected virtual void Awake()
        {
            if (gameObject.layer != InteractableLayer)
            {
                gameObject.layer = InteractableLayer;
#if UNITY_EDITOR
                Debug.LogWarning(
                    $"{gameObject.name}: Автоматически установлен слой Interactable (слой {InteractableLayer})");
#endif
            }
        }

        public abstract void Interact();
    }
}