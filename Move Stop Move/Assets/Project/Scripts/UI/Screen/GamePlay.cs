using System.Collections;
using Project.Scripts.Level;
using Project.Scripts.UI.Manager;
using UnityEngine;
using TMPro;

namespace Project.Scripts.UI.Screen
{

    public class GamePlay : UICanvas
    {
        public TMP_Text aliveText;
        private int _alive;
        public void GetNumberAlive(int count)
        {
            _alive = count;
            aliveText.text = "Alive: " + _alive;
        }

        public void SetNumberAlive()
        {
            _alive--;
            aliveText.text = "Alive: " + _alive;
            StartCoroutine(WaitGenerateBot());
        }
        IEnumerator WaitGenerateBot()
        {
            yield return new WaitForSeconds(0.5f);
            RandomNavMeshSpawner.Instance.GenerateCharacter(SpawnCharacterType.Bot);
        }
    }
}
