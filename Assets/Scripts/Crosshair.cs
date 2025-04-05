using UnityEngine;
using UnityEngine.UI;

namespace MrLucy
{
    public class Crosshair : MonoBehaviour
    {
        private int interactableLayerMask;
        
        public Image crosshair;
        public float rayDistance = 1.5f;

        private BaseInteractableObject current;
        private Transform highlight;

        private void Awake()
        {
            interactableLayerMask = 1 << 6; // interactable layer == 6
        }

        private void Start()
        {
            crosshair.enabled = false;
        }
        
        private void Update()
        {
            if (highlight != null)
            {
                if (highlight.gameObject.TryGetComponent<Outline>(out var outline))
                    outline.enabled = false;
                highlight = null;
            }
            
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));

            if (Physics.Raycast(ray, out var hit, rayDistance, interactableLayerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);
                var interactable = hit.collider.GetComponent<BaseInteractableObject>();
                
                highlight = hit.transform;
                    
                current = interactable;
                
                if (interactable.isActive)
                {
                    crosshair.enabled = true;
                    
                    if (highlight.gameObject.TryGetComponent<Outline>(out var outline))
                        outline.enabled = true;

                    if (Input.GetMouseButton(0))
                    {
                        current.Interact();
                    }
                }

                return;
            }

            current = null;
            crosshair.enabled = false;
        }
    }
}
