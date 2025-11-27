using System.Collections;
using Project.Scripts.Character;
using Project.Scripts.Level;
using Project.Scripts.UI.Manager;
using UnityEngine;
using TMPro;

namespace Project.Scripts.UI.Screen
{

    public class GamePlay : UICanvas
    {
        public TMP_Text aliveText;
        public void GetNumberAlive()
        {
           
            aliveText.text = "Alive: " + RandomNavMeshSpawner.Instance.TotalAlive;
        }

        public void SetNumberAlive()
        {
            RandomNavMeshSpawner.Instance.TotalAlive--;
            aliveText.text = "Alive: " + RandomNavMeshSpawner.Instance.TotalAlive;
            StartCoroutine(WaitGenerateBot());
            if ( RandomNavMeshSpawner.Instance.TotalAlive <= 1 )
            {
                StateUI.Instance.ChangeState(StateType.Victory);
            }
        }
        IEnumerator WaitGenerateBot()
        {
            yield return new WaitForSeconds(0.5f);
            RandomNavMeshSpawner.Instance.GenerateCharacter(SpawnCharacterType.Bot);
        }
    }
}
