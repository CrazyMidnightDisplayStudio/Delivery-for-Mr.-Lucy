namespace MrLucy
{
    public class ElevatorButtonDown : ElevatorButton
    {
        private void Start()
        {
            //isInteractActive = false;
        }

        public override void Interact()
        {
            base.Interact();
            if (!isInteractActive) return;
            GameManager.Instance.SetState(GameState.Downhill);
        }

        protected override void OnGameStateChanged(GameState state)
        {
            isInteractActive = state == GameState.WaitingButton;
        }
    }
}