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
        private ItemMover _itemMover;

        public bool Empty { get; private set; } = true;

        private void Awake()
        {
            _itemMover = GetComponent<ItemMover>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Empty)
                {
                    phone.gameObject.SetActive(true);
                    TryPickUp(phone);
                }
                else
                {
                    if (phone.IsLightOn) phone.IsLightOn = false;
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
            if (!Empty) return;

            Empty = false;
            currentItem = item;
            currentItem.transform.SetParent(transform);
            currentItem.transform.localPosition = handOffScreenPosition.localPosition;

            _itemMover.MoveTo(currentItem.transform, handScreenPosition, 0.5f);
        }

        public GameObject DropItem()
        {
            if (Empty) return null;

            _itemMover.MoveTo(currentItem.transform, handOffScreenPosition, 0.5f, () =>
            {
                Empty = true;
                currentItem = null;
            });

            return currentItem;
        }
    }
}