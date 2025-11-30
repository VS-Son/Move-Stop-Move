using Project.Scripts.Level;
using Project.Scripts.UI.Manager;

namespace Project.Scripts.UI.Screen
{
    public class MainMenu : UICanvas
    {
        public void OnStartGame()
        {
            StateUI.ChangeState(StateType.Gameplay);
            CloseDirectly();
            RandomNavMeshSpawner.Instance.OnStartGame();
            UIManager.Instance.GetUI<GamePlay>().GetNumberAlive(RandomNavMeshSpawner.Instance.TotalAlive);
        }
    }
}