using UnityEngine;
using UnityEngine.UI;

namespace MrLucy
{
    public class Crosshair : MonoBehaviour
    {
        [SerializeField] private HandSlot handSlot;
        private int _interactableLayerMask;
        private float lastInteractTime = -Mathf.Infinity;

        public Image crosshair;
        public float rayDistance = 1.5f;

        private BaseInteractableObject current;
        private Transform highlight;

        private void Awake()
        {
            _interactableLayerMask = 1 << 6; // interactable layer == 6
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

            if (Physics.Raycast(ray, out var hit, rayDistance, _interactableLayerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);
                if (!hit.collider.TryGetComponent<BaseInteractableObject>(out var interactable))
                {
                    return;
                }

                highlight = hit.transform;

                current = interactable;

                if (interactable.isInteractActive)
                {
                    if (highlight.gameObject.TryGetComponent<Outline>(out var outline))
                        outline.enabled = true;

                    if (GameManager.Instance.CurrentState == GameState.EbakaState)
                    {
                        Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
                        if (current.TryGetComponent(out Ebaka ebaka))
                        {
                            Debug.Log("It is ebaka");
                            ebaka.Interact();
                        }
                    }

                    if (Input.GetMouseButtonDown(0) && Time.time - lastInteractTime >= 0.3f)
                    {
                        lastInteractTime = Time.time;
                        current.Interact();
                    }
                }

                return;
            }

            current = null;
        }
    }
}