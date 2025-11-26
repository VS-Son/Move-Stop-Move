using Project.Scripts.UI.Screen;

namespace Project.Scripts.UI.Manager
{
    public enum StateType
    {
        MainMenu,
        Gameplay,
        Pause
    }

    public class StateUI : Singleton<StateUI>
    {
        private StateType _gameState;

        private void Start()
        {
            ChangeState(StateType.MainMenu);
        }

        public void ChangeState(StateType gameState)
        {
            _gameState = gameState;
            if (_gameState == StateType.Gameplay) UIManager.Instance.CloseUI<MainMenu>();
        }

        public bool IsState(StateType gameState)
        {
            return _gameState == gameState;
        }
    }
}