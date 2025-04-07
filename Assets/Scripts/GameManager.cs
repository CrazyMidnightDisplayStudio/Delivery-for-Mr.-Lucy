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
        WaitForRedButtonReceive,
        FirstCallRedButton,
        WaitSpaceButton,
        ElevatorStuck,
        TheHatchIsOpened,
        EbakaState,
        EnteredTheCode,
        FinalScene,
    }

    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private HandSlot _handSlot;
        [SerializeField] private ElevatorDownhillScenario _elevatorDownhillScenario;
        [SerializeField] private ElevatorLight _elevatorLight;
        [SerializeField] private RedButton _redButton;
        [SerializeField] private CameraShaker _cameraShaker;
        [SerializeField] private Hatch _hatch;
        [SerializeField] private Phone _phone;
        [SerializeField] private Ebaka _ebaka;

        [SerializeField] private DialogueData _jumpDialogueData;
        [SerializeField] private DialogueData _startGameDialogueData;

        private DialogueSystem _dialogueSystem;

        public GameState CurrentState { get; private set; }

        public HandSlot GetHandSlot() => _handSlot;

        public event Action<GameState> OnStateChanged;

        private void Awake()
        {
            _dialogueSystem = DialogueSystem.Instance;
        }

        private void Start()
        {
            SetState(NextState());
            CutsceneManager.Instance.StartCutscene("StartGame");
            Time.timeScale *= 2f;
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
                    DialogueSystem.Instance.StartDialogue(_startGameDialogueData);
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
                    // выстреливаем кнопкой
                    InvokeAfterDelay(5f, _redButton.Fire);
                    break;
                case GameState.WaitForRedButtonReceive:
                    // слот для установки красной кнопки включается сам
                    break;
                case GameState.FirstCallRedButton:
                    DialogueSystem.Instance.StartDialogue(_jumpDialogueData);
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
                    break;
                case GameState.FinalScene:
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
    }
}