using MrLucy;
using UnityEngine;

[RequireComponent(typeof(ItemMover))]
public class Phone : MonoBehaviour
{
    [SerializeField] private Light flashlight;
    [SerializeField] private Transform handScreenPosition;
    [SerializeField] private Transform handOffScreenPosition;

    public bool isActive = true;

    private ItemMover _itemMover;
    private MeshRenderer _meshRenderer;
    private HandSlot _handSlot;

    private bool _phoneEquipped;

    public bool PhoneEquipped
    {
        get => _phoneEquipped;
        private set { _phoneEquipped = value; }
    }

    public bool IsLightOn
    {
        get => flashlight.enabled;
        set => flashlight.enabled = value;
    }

    private void Awake()
    {
        _itemMover = GetComponent<ItemMover>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _handSlot = GameManager.Instance.GetHandSlot();
    }

    private void Start()
    {
        IsLightOn = false;
        transform.SetParent(handScreenPosition.parent);
        transform.localPosition = handOffScreenPosition.localPosition;
        PhoneEquipped = false;
        ShowPhone();
    }

    private void Update()
    {
        if (!isActive) return;
        if (!_handSlot.Empty)
        {
            // если взяли отвертку, телефон убираем
            if (_handSlot.currentItem.TryGetComponent(out Screw sc))
            {
                HidePhone();
                isActive = false;
                return;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (PhoneEquipped)
                HidePhone();
            else
                ShowPhone();
        }

        if (Input.GetKeyDown(KeyCode.F) && PhoneEquipped)
        {
            if (PhoneEquipped)
            {
                IsLightOn = !IsLightOn;
            }
            else
            {
                IsLightOn = false;
            }
        }
    }

    public void ShowPhone()
    {
        _meshRenderer.enabled = true;
        _itemMover.MoveToAndDestroyRB(transform, handScreenPosition, 1f, () => PhoneEquipped = true);
    }

    public void HidePhone()
    {
        IsLightOn = false;
        _itemMover.MoveToAndDestroyRB(transform, handOffScreenPosition, 1f, () =>
        {
            PhoneEquipped = false;
            _meshRenderer.enabled = false;
        });
    }
}