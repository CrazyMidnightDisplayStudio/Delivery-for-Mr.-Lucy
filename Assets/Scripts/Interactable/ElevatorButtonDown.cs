namespace MrLucy
{
    public class ElevatorButtonDown : BaseInteractableObject
    {
        private System.Action<bool> onButtonDownActivatedHandler;
        private Button _buttonAnimator;

        private void Start()
        {
            isActive = false;
            _buttonAnimator = gameObject.GetComponent<Button>();
        }

        public override void Interact()
        {
            _buttonAnimator.PushButton();
            if (!isActive) return;
            GameManager.Instance.SetState(GameState.Downhill);
        }

        protected override void OnGameStateChanged(GameState state)
        {
            //isActive = state == GameState.WaitingButton;
            isActive = true;
        }
    }
}