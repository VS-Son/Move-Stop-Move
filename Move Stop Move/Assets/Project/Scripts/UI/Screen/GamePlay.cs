using System.Collections;
using Project.Scripts.Level;
using Project.Scripts.UI.Manager;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI.Screen
{
    public class GamePlay : UICanvas
    {
        public TMP_Text aliveText;
        private int _totalAlive;

        private void OnEnable()
        {
            Character.Character.SetNumberAlive += SetNumberAlive;
        }

        private void OnDisable()
        {
            Character.Character.SetNumberAlive -= SetNumberAlive;
        }

        public void GetNumberAlive(int count)
        {
            _totalAlive = count;
            aliveText.text = "Alive: " + _totalAlive;
        }

        private void SetNumberAlive()
        {
            _totalAlive--;
            aliveText.text = "Alive: " + _totalAlive;
            StartCoroutine(WaitGenerateBot());
            if (RandomNavMeshSpawner.Instance.TotalAlive <= 1) StateUI.Instance.ChangeState(StateType.Victory);
        }

        private IEnumerator WaitGenerateBot()
        {
            yield return new WaitForSeconds(0.5f);
            if (_totalAlive > RandomNavMeshSpawner.Instance.ListBot.Count)
                RandomNavMeshSpawner.Instance.GenerateCharacter(SpawnCharacterType.Bot);
        }
    }
}