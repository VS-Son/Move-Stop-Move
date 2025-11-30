using Project.Scripts.Level;
using Project.Scripts.UI.Manager;

namespace Project.Scripts.UI.Screen
{
    public class Victory : UICanvas
    {
        public void OnPlayZone()
        {
            StateUI.ChangeState(StateType.MainMenu);
            RandomNavMeshSpawner.Instance.OnResetPlayZone();
            CloseDirectly();
        }
    }
}
