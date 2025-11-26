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
            var uiManager = UIManager.Instance;
            _gameState = gameState;
            switch (_gameState)
            {
                case StateType.Gameplay:
                    uiManager.CloseUI<MainMenu>();
                    uiManager.OpenUI<GamePlay>();
                    break;
            }
        }

        public bool IsState(StateType gameState)
        {
            return _gameState == gameState;
        }
    }
}