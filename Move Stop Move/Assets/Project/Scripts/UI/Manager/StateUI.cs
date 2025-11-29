using Project.Scripts.UI.Screen;

namespace Project.Scripts.UI.Manager
{
    public enum StateType
    {
        MainMenu,
        Gameplay,
        Pause,
        Revive,
        Victory
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
                case StateType.Revive:
                    uiManager.CloseUI<GamePlay>();
                    uiManager.OpenUI<Revive>();
                    break;
                case StateType.Victory:
                    uiManager.CloseUI<GamePlay>();
                    uiManager.OpenUI<Victory>();
                    break;
            }
        }

        public bool IsState(StateType gameState)
        {
            return _gameState == gameState;
        }
    }
}