using System.Collections;
using Project.Scripts.Character;
using Project.Scripts.Characters;
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
            Characters.Character.SetNumberAlive += SetNumberAlive;
        }

        private void OnDisable()
        {
            Characters.Character.SetNumberAlive -= SetNumberAlive;
        }

        public void GetNumberAlive(int count)
        {
            _totalAlive = count;
            aliveText.text = "Alive: " + _totalAlive;
        }

        private void SetNumberAlive(Characters.Character character)
        {
           
            _totalAlive--;
            aliveText.text = "Alive: " + _totalAlive;
            StartCoroutine(WaitGenerateBot());
            if (_totalAlive <= 1 && character is Player player)
                if (player.transform.gameObject.activeSelf)
                {
                    StateUI.ChangeState(StateType.Victory);
                    CloseDirectly();
                }
                    
           
        }

        private IEnumerator WaitGenerateBot()
        {
            yield return new WaitForSeconds(0.5f);
            if (_totalAlive > RandomNavMeshSpawner.Instance.ListBot.Count)
                RandomNavMeshSpawner.Instance.GenerateCharacter(SpawnCharacterType.Bot);
        }
        
    }
}