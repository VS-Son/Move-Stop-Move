using Project.Scripts.UI.Screen;
using UnityEngine;

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

    public class StateUI : MonoBehaviour
    {
        private static StateType _gameState;

        private void Start()
        {
            ChangeState(StateType.MainMenu);
        }

        public static void ChangeState(StateType gameState)
        {
            var uiManager = UIManager.Instance;
            _gameState = gameState;
            switch (_gameState)
            {
                case StateType.MainMenu:
                    uiManager.OpenUI<MainMenu>();
                    break;
                case StateType.Gameplay:
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

        public static bool IsState(StateType gameState)
        {
            return _gameState == gameState;
        }
    }
}