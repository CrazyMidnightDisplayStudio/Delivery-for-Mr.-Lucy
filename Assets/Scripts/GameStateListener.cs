using UnityEngine;

namespace MrLucy
{
    public abstract class GameStateListener : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnStateChanged += HandleStateChanged;
        }

        protected virtual void OnDisable()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnStateChanged -= HandleStateChanged;
        }

        private void HandleStateChanged(GameState newState)
        {
            OnGameStateChanged(newState);
        }

        /// <summary>
        /// Наследники реализуют поведение при смене состояния игры
        /// </summary>
        protected abstract void OnGameStateChanged(GameState state);
    }
}
