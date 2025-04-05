using UnityEngine;

namespace MrLucy
{
    public class HandSlot : MonoBehaviour
    {
        [SerializeField] Transform handPosition;
        [SerializeField] private Phone phone;
        
        public GameObject currentItem;
        private bool _isOccupied;

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
            if (_isOccupied) return;

            _isOccupied = true;
            currentItem = item.GetPickupPrefab();
            currentItem.transform.SetParent(transform);
            currentItem.transform.localPosition = handPosition.transform.localPosition;
        }

        public void TryPickUp(GameObject item)
        {
            if (_isOccupied) return;

            _isOccupied = true;
            currentItem = item;
            currentItem.transform.SetParent(transform);
            currentItem.transform.localPosition = handPosition.transform.localPosition;
        }

        public GameObject DropItem()
        {
            if (!_isOccupied) return null;
            _isOccupied = false;
            var item = currentItem;
            currentItem = null;
            return item;
        }
    }
}