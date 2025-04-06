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
        FirstFlashlight,
        ElevatorStuck,
        TheHatchIsOpened,
        EnteredTheCode,
        FinalScene,
    }

    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private HandSlot _handSlot;
        [SerializeField] private ElevatorDownhillScenario _elevatorDownhillScenario;
        [SerializeField] private ElevatorLight _elevatorLight;
        [SerializeField] private RedButton _redButton;
        public GameState CurrentState { get; private set; }
        private GameState NextState() => CurrentState + 1;

        public HandSlot GetHandSlot() => _handSlot;

        public event Action<GameState> OnStateChanged;

        private void Start()
        {
            SetState(NextState());
        }

        public void SetState(GameState newState)
        {
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
                    break;
                case GameState.ChaoticFall:
                    // моргает свет, красная кнопка выпадает
                    _elevatorDownhillScenario.StartChaoticDownhill();
                    // через 5 секунд свет полностью погаснет
                    _elevatorLight.StartBlinking(5f);
                    // выстреливаем кнопкой
                    InvokeAfterDelay(5f, _redButton.Fire);
                    break;
                case GameState.FirstFlashlight:
                    // логика при движении лифта
                    break;
                case GameState.ElevatorStuck:
                    // запуск головоломок
                    break;
                case GameState.TheHatchIsOpened:
                    // 
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
    }
}