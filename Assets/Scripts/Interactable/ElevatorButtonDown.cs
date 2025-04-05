namespace MrLucy
{
    public class ElevatorButtonDown : BaseInteractableObject
    {
        private System.Action<bool> onButtonDownActivatedHandler;

        private void Start()
        {
            isActive = false;
        }

        public override void Interact()
        {
            if (!isActive) return;
            GameManager.Instance.SetState(GameState.Downhill);
        }

        protected override void OnGameStateChanged(GameState state)
        {
            isActive = state == GameState.WaitingButton;
        }
    }
}