using System;
using System.Collections;
using UnityEngine;

namespace MrLucy
{
    public class ItemMover : MonoBehaviour
    {
        public void MoveToAndDestroyRB(Transform item, Transform target, float moveSpeed = 1f, Action onComplete = null)
        {
            if (item.TryGetComponent(out Rigidbody rb))
            {
                Destroy(rb);
            }
            
            StartCoroutine(MoveRoutine(item, target, moveSpeed, onComplete));
        }

        private IEnumerator MoveRoutine(Transform item, Transform target, float moveSpeed, Action onComplete)
        {
            var hasCol = item.TryGetComponent<Collider>(out var col);

            // Сохраняем мировую позицию и вращение до смены родителя
            Vector3 worldPosition = item.position;
            Quaternion worldRotation = item.rotation;

            // Назначаем нового родителя
            item.SetParent(target.parent);

            // Восстанавливаем мировые координаты, чтобы не было рывка
            item.position = worldPosition;
            item.rotation = worldRotation;

            // Целевая локальная позиция относительно родителя
            Vector3 targetLocalPos = target.localPosition;

            while (Vector3.Distance(item.localPosition, targetLocalPos) > 0.01f)
            {
                item.localPosition = Vector3.MoveTowards(
                    item.localPosition,
                    targetLocalPos,
                    moveSpeed * Time.deltaTime
                );
                yield return null;
            }

            item.localPosition = targetLocalPos;
            if (hasCol && col)
            {
                col.enabled = true;
            }
            onComplete?.Invoke();
        }
    }
}