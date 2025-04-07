using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace MrLucy
{
    [RequireComponent(typeof(ItemMover))]
    public class HandSlot : MonoBehaviour
    {
        [SerializeField] Transform handPosition;

        public GameObject currentItem;
        private ItemMover _itemMover;

        public bool Empty { get; private set; } = true;

        private void Awake()
        {
            _itemMover = GetComponent<ItemMover>();
        }

        public void TryPickUp(IPickupObject item, Action onComplete = null)
        {
            var gameObject = item.GetPickupPrefab();
            TryPickUp(gameObject, onComplete);
        }

        public void TryPickUp(GameObject item, Action onComplete = null)
        {
            if (!Empty) return;

            Empty = false;
            currentItem = item;

            _itemMover.MoveToAndDestroyRB(currentItem.transform, handPosition, 2f, onComplete);
        }

        public GameObject ReceiveItem(Transform receiver, Action<GameObject> onFinish = null)
        {
            if (Empty) return null;

            var itemToDrop = currentItem;

            _itemMover.MoveToAndDestroyRB(itemToDrop.transform, receiver, 2f, () =>
            {
                Empty = true;
                currentItem = null;
                onFinish?.Invoke(itemToDrop);
            });

            return itemToDrop;
        }
        
        public void DropItem(Action onComplete = null)
        {
            if (Empty) return;
            
            currentItem.transform.SetParent(null);
            currentItem.gameObject.AddComponent<Rigidbody>();
            currentItem = null;
            
            Empty = true;
        }
    }
}