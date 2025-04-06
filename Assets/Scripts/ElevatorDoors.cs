using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ElevatorDoors : MonoBehaviour
{
    [SerializeField] private Transform leftDoor;
    [SerializeField] private Transform rightDoor;
    [SerializeField] private float openDistance = 1f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    private Vector3 _leftDoorStartPos;
    private Vector3 _rightDoorStartPos;
    private AudioSource _audioSource;
    private bool _isMoving;
    private bool _isOpen;
    private float _moveProgress;

    private void Awake()
    {
        InitializeComponents();
        CacheStartPositions();
    }

    private void InitializeComponents()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }
    }

    private void CacheStartPositions()
    {
        _leftDoorStartPos = leftDoor.localPosition;
        _rightDoorStartPos = rightDoor.localPosition;
    }

    public void OpenDoors()
    {
        if (_isOpen || _isMoving) return;
        
        StartDoorMovement(true);
        PlaySound(openSound);
    }

    public void CloseDoors()
    {
        if (!_isOpen || _isMoving) return;
        
        StartDoorMovement(false);
        PlaySound(closeSound);
    }

    private void StartDoorMovement(bool opening)
    {
        _isMoving = true;
        _isOpen = opening;
        _moveProgress = 0f;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            _audioSource.PlayOneShot(clip);
        }
    }

    private void Update()
    {
        if (!_isMoving) return;

        UpdateDoorMovement();
    }

    private void UpdateDoorMovement()
    {
        _moveProgress += Time.deltaTime * moveSpeed;
        _moveProgress = Mathf.Clamp01(_moveProgress);

        Vector3 leftTarget = _isOpen 
            ? _leftDoorStartPos + Vector3.forward * openDistance 
            : _leftDoorStartPos;

        Vector3 rightTarget = _isOpen 
            ? _rightDoorStartPos - Vector3.forward * openDistance 
            : _rightDoorStartPos;

        leftDoor.localPosition = Vector3.Lerp(
            _isOpen ? _leftDoorStartPos : _leftDoorStartPos + Vector3.forward * openDistance,
            leftTarget,
            _moveProgress
        );

        rightDoor.localPosition = Vector3.Lerp(
            _isOpen ? _rightDoorStartPos : _rightDoorStartPos - Vector3.forward * openDistance,
            rightTarget,
            _moveProgress
        );

        if (_moveProgress >= 1f)
        {
            _isMoving = false;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ElevatorDoors))]
    public class ElevatorDoorsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ElevatorDoors doors = (ElevatorDoors)target;
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("Open Doors"))
            {
                doors.OpenDoors();
            }
            
            if (GUILayout.Button("Close Doors"))
            {
                doors.CloseDoors();
            }
        }
    }
#endif
}
