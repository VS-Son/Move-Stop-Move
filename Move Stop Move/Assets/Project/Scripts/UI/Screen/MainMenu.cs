using Project.Scripts.Level;
using Project.Scripts.UI.Manager;

namespace Project.Scripts.UI.Screen
{
    public class MainMenu : UICanvas
    {
        public void OnStartGame()
        {
            StateUI.Instance.ChangeState(StateType.Gameplay);
            RandomNavMeshSpawner.Instance.OnStartGame();
            UIManager.Instance.GetUI<GamePlay>().GetNumberAlive();
        }
    }
}