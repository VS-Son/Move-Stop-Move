using Project.Scripts.Level;
using Project.Scripts.UI.Manager;

namespace Project.Scripts.UI.Screen
{
    public class Revive : UICanvas
    {
        public void OnClose()
        {
            StateUI.ChangeState(StateType.MainMenu);
            CloseDirectly();
            RandomNavMeshSpawner.Instance.OnResetPlayZone();
        }
        public void OnReviveCoin()
        {
            StateUI.ChangeState(StateType.Gameplay);
            CloseDirectly();
            RandomNavMeshSpawner.Instance.GenerateCharacter(SpawnCharacterType.Player);
        }

        public void OnReviveAds()
        {
            StateUI.ChangeState(StateType.Gameplay);
            CloseDirectly();
            RandomNavMeshSpawner.Instance.GenerateCharacter(SpawnCharacterType.Player);
        }
    }
}
