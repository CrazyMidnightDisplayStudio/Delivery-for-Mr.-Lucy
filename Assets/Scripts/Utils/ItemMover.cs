using System;
using System.Collections;
using UnityEngine;

public class ItemMover : MonoBehaviour
{
    public void MoveToAndDestroyRB(Transform item, Transform target, float moveSpeed = 1f, Action onComplete = null)
    {
        // Удаляем Rigidbody до смены родителя, чтобы физика не мешала
        if (item.TryGetComponent<Rigidbody>(out var rb))
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

        // Подождать 1 кадр, чтобы избежать глюков при смене родителя
        yield return null;

        // Восстанавливаем мировые координаты, чтобы не было рывка
        item.position = worldPosition;
        item.rotation = worldRotation;

        // Получаем локальную позицию цели относительно нового родителя
        Vector3 targetLocalPos = item.parent.InverseTransformPoint(target.position);

        // Плавно двигаем к целевой позиции
        while (Vector3.Distance(item.localPosition, targetLocalPos) > 0.01f)
        {
            item.localPosition = Vector3.MoveTowards(
                item.localPosition,
                targetLocalPos,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        // Финальная корректировка позиции
        item.localPosition = targetLocalPos;

        // Включаем коллайдер, если был
        if (hasCol && col)
        {
            col.enabled = true;
        }

        // Коллбек
        onComplete?.Invoke();
    }
}