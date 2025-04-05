using System;
using UnityEngine;

namespace MrLucy
{
    public enum GameState
    {
        WaitingButton,
        Downhill,
        FirstFlashlight,
        ElevatorStuck,
        TheHatchIsOpened,
        EnteredTheCode,
        FinalScene,

        MaxValue
    }

    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private HandSlot _handSlot;
        public GameState CurrentState { get; private set; } = GameState.MaxValue;
        
        public HandSlot GetHandSlot() => _handSlot;

        public event Action<GameState> OnStateChanged;
        private void Start()
        {
            SetState(0);
        }

        public void SetState(GameState newState)
        {
            if (CurrentState == newState) return;

            CurrentState = newState;
            OnStateChanged?.Invoke(CurrentState);
            Debug.Log($"Состояние изменено: {newState}");

            switch (newState)
            {
                case GameState.WaitingButton:
                    // активируем кнопку для начала спуска
                    break;
                case GameState.Downhill:
                    // спустя 5 сек выключаем свет в лифте и включаем подсказку как достать телефон
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
    }
}