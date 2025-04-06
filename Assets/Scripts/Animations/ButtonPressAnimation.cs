using UnityEngine;

public class ButtonPressAnimation : MonoBehaviour
{
    public bool active = true;
    
    private float pushDistanceX = 0.01f;
    private float pushDuration = 0.5f;
    
    private Vector3 _originalPosition;
    private bool _isMoving;
    private float _moveProgress;
    private bool _isForwardMovement = true;

    private void Awake()
    {
        _originalPosition = transform.localPosition;
    }

    public void PushButton()
    {
        if (!active) return;
        if (_isMoving) return;
        
        _isMoving = true;
        _moveProgress = 0f;
        _isForwardMovement = true;
    }

    private void Update()
    {
        if (!active) return;
        if (!_isMoving) return;

        _moveProgress += Time.deltaTime / pushDuration;
        
        float newX = _isForwardMovement 
            ? Mathf.Lerp(_originalPosition.x, _originalPosition.x + pushDistanceX, _moveProgress)
            : Mathf.Lerp(_originalPosition.x + pushDistanceX, _originalPosition.x, _moveProgress);

        transform.localPosition = new Vector3(
            newX,
            _originalPosition.y,
            _originalPosition.z
        );

        if (_moveProgress >= 1f)
        {
            if (_isForwardMovement)
            {
                _isForwardMovement = false;
                _moveProgress = 0f;
            }
            else
            {
                _isMoving = false;
            }
        }
    }
}
