using UnityEngine;

namespace MrLucy
{
    [RequireComponent(typeof(Collider))]
    public abstract class BaseInteractableObject : GameStateListener
    {
        public bool isInteractActive = true;

        private const int InteractableLayer = 6;

        protected virtual void Awake()
        {
            if (gameObject.layer != InteractableLayer)
            {
                gameObject.layer = InteractableLayer;
            }
        }

        public abstract void Interact();
    }
}