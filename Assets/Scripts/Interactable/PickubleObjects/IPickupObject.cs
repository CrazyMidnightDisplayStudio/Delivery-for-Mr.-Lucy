using UnityEngine;

namespace MrLucy
{
    public interface IPickupObject
    {
        bool CanBePickedUp { get; }
        GameObject GetPickupPrefab();
    }
}