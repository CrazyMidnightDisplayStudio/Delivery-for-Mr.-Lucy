using UnityEngine;

namespace MrLucy
{
    [RequireComponent(typeof(ItemMover))]
    public class HandSlot : MonoBehaviour
    {
        [SerializeField] Transform handScreenPosition;
        [SerializeField] Transform handOffScreenPosition;
        [SerializeField] private Phone phone;

        public GameObject currentItem;
        private bool _isOccupied;
        private ItemMover _itemMover;

        private void Awake()
        {
            _itemMover = GetComponent<ItemMover>();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!_isOccupied)
                {
                    TryPickUp(phone);
                }
                else
                {
                    DropItem();
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
            }
        }

        public void TryPickUp(IPickupObject item)
        {
            var gameObject = item.GetPickupPrefab();
            TryPickUp(gameObject);
        }

        public void TryPickUp(GameObject item)
        {
            if (_isOccupied) return;

            _isOccupied = true;
            currentItem = item;
            currentItem.transform.SetParent(transform);
            currentItem.transform.localPosition = handOffScreenPosition.localPosition;

            _itemMover.MoveTo(currentItem.transform, handScreenPosition, 0.5f);
        }

        public GameObject DropItem()
        {
            if (!_isOccupied) return null;

            _itemMover.MoveTo(currentItem.transform, handOffScreenPosition, 0.5f, () =>
            {
                _isOccupied = false;
                currentItem = null;
            });

            return currentItem;
        }
    }
}