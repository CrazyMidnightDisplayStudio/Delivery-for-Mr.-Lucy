namespace MrLucy
{
    public class ElevatorButtonDown : BaseInteractableObject
    {
        private System.Action<bool> onButtonDownActivatedHandler;

        private void Start()
        {
            IsActive = false;
        }

        public override void Interact()
        {
            if (!IsActive) return;
            GameManager.Instance.SetState(GameState.Downhill);
        }

        protected override void OnGameStateChanged(GameState state)
        {
            IsActive = state == GameState.WaitingButton;
        }
    }
}