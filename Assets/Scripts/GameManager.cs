using System;
using System.Collections;
using UnityEngine;

namespace MrLucy
{
    public enum GameState
    {
        Idle = 0,
        WaitingButton,
        Downhill,
        ChaoticFall,
        RedButtonFired,
        WaitForRedButtonReceive,
        FirstCallRedButton,
        WaitSpaceButton,
        ElevatorStuck,
        TheHatchIsOpened,
        EbakaState,
        EnteredTheCode,
        OpenDoorsState,
        Final
    }

    public class GameManager : Singleton<GameManager>
    {
        public Code312 code;

        [SerializeField] private HandSlot _handSlot;
        [SerializeField] private ElevatorDownhillScenario _elevatorDownhillScenario;
        [SerializeField] private ElevatorLight _elevatorLight;
        [SerializeField] private ElevatorDoors _elevatorDoors;
        [SerializeField] private RedButton _redButton;
        [SerializeField] private CameraShaker _cameraShaker;
        [SerializeField] private Hatch _hatch;
        [SerializeField] private Phone _phone;
        [SerializeField] private Ebaka _ebaka;
        [SerializeField] private GameObject _newOutside;
        [SerializeField] private GameObject _oldOutside;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Final _final;

        [SerializeField] private DialogueData _jumpDialogueData;
        [SerializeField] private DialogueData _startGameDialogueData;
        [SerializeField] private DialogueData _findButton;

        public GameState CurrentState { get; private set; }

        public HandSlot GetHandSlot() => _handSlot;

        public event Action<GameState> OnStateChanged;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Start()
        {
            SetState(NextState());
            CutsceneManager.Instance.StartCutscene("StartGame");
        }

        private void Update()
        {
            if (CurrentState == GameState.WaitSpaceButton)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SetState(GameState.ElevatorStuck);
                }
            }

            if (CurrentState == GameState.TheHatchIsOpened)
            {
                if (_phone.IsLightOn)
                {
                    _ebaka.ShowEbaka();
                    SetState(GameState.EbakaState);
                }
            }

            if (CurrentState == GameState.OpenDoorsState)
            {
                if (_playerTransform.position.z < -0.64f && Input.GetKeyDown(KeyCode.Space))
                {
                    _final.Run();
                }
            }
        }

        public void SetState(GameState newState)
        {
            Debug.Log($"[GameManager] Trying to SetState: {newState} ({(int)newState}), current: {CurrentState}");

            if (!Enum.IsDefined(typeof(GameState), newState))
            {
                Debug.LogError($"Invalid GameState: {newState} ({(int)newState})");
                return;
            }

            // стейт можно только следующий включать
            if (newState <= CurrentState) return;

            CurrentState = newState;
            OnStateChanged?.Invoke(CurrentState);
            Debug.Log($"Состояние изменено: {newState}");

            switch (newState)
            {
                case GameState.WaitingButton:
                    // активируем кнопку для начала спуска
                    break;
                case GameState.Downhill:
                    _elevatorDownhillScenario.StartDownhill(); // на -100 этаже сценарий переключит стейт на след
                    _cameraShaker.StartShake(1f, 1.2f);
                    _phone.HidePhone();
                    break;
                case GameState.ChaoticFall:
                    // моргает свет, красная кнопка выпадает
                    _elevatorDownhillScenario.StartChaoticDownhill();
                    _cameraShaker.StartShake(0.8f, 3.2f);
                    // через 5 секунд свет полностью погаснет
                    _elevatorLight.StartBlinking(5f);
                    // переход на след сцену
                    InvokeAfterDelay(5f, SetNextState);
                    break;
                case GameState.RedButtonFired:
                    // выстреливаем кнопкой
                    _redButton.Fire();
                    break;
                case GameState.WaitForRedButtonReceive:
                    // слот для установки красной кнопки включается сам
                    break;
                case GameState.FirstCallRedButton:
                    DialogueSystem.Instance.EndDialogue();
                    //DialogueSystem.Instance.StartDialogue(_jumpDialogueData);
                    DialogueSystem.Instance.StartDialogueJump();
                    // в этом стейте при нажатии красной кнопки вызовится диалог
                    SetState(GameState.WaitSpaceButton);
                    break;
                case GameState.WaitSpaceButton:
                    // ждем прыжка который остановит лифт в Update()
                    break;
                case GameState.ElevatorStuck:
                    DialogueSystem.Instance.EndDialogue();
                    // останавливаем лифт
                    _cameraShaker.StopShake();
                    _elevatorDownhillScenario.StopDownhill();
                    _elevatorDownhillScenario.StartChaotic666();
                    _elevatorLight.SetWarningMode(true);
                    _elevatorLight.TurnOn();
                    break;
                case GameState.TheHatchIsOpened:
                    GetHandSlot().DropItem();
                    _hatch.gameObject.AddComponent<Rigidbody>();
                    _phone.isActive = true;
                    break;
                case GameState.EbakaState:
                    break;
                case GameState.EnteredTheCode:
                    code.active = true;
                    _oldOutside.SetActive(false);
                    _newOutside.SetActive(true);
                    break;
                case GameState.OpenDoorsState:
                    code.active = false;
                    _elevatorDoors.OpenDoors();
                    break;
                case GameState.Final:
                    _final.Run();
                    break;
            }
        }

        protected virtual void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnStateChanged -= OnStateChanged;
            }
        }

        public static void InvokeAfterDelay(float delay, Action action)
        {
            Instance.StartCoroutine(Instance.RunDelay(delay, action));
        }

        private IEnumerator RunDelay(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }

        private GameState NextState()
        {
            int next = (int)CurrentState + 1;
            if (Enum.IsDefined(typeof(GameState), next))
                return (GameState)next;

            Debug.LogWarning("NextState() вызван, но дальше уже некуда переходить.");
            return CurrentState; // или FinalScene
        }
        
        private void SetNextState() => SetState(NextState());

        public void ButtonDialogue()
        {
            //DialogueSystem.Instance.StartDialogue(_findButton);
            DialogueSystem.Instance.StartDialogueFind();
        }
    }
}