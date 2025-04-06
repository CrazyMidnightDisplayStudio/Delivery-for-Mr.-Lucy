using System;
using System.Collections;
using UnityEngine;

namespace MrLucy
{
    public class ItemMover : MonoBehaviour
    {
        public void MoveTo(Transform item, Transform target, float moveSpeed = 1f, Action onComplete = null)
        {
            StartCoroutine(MoveRoutine(item, target, moveSpeed, onComplete));
        }

        private IEnumerator MoveRoutine(Transform item, Transform target, float moveSpeed, Action onComplete)
        {
            while (Vector3.Distance(item.localPosition, target.localPosition) > 0.01f)
            {
                item.localPosition = Vector3.MoveTowards(
                    item.localPosition,
                    target.localPosition,
                    moveSpeed * Time.deltaTime
                );
                yield return null;
            }

            item.localPosition = target.localPosition;
            onComplete?.Invoke();
        }
    }
}