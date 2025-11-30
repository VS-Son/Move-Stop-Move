using Project.Scripts.Character.ScriptableObject;
using UnityEngine;

namespace Project.Scripts.Characters
{
    public class ColorObject : GameUnit
    {
        [SerializeField] private ColorData colorData;
        [SerializeField] private new Renderer renderer;

        public void ChangeColor(ColorType cType)
        {
            renderer.material = colorData.GetColorMat(cType);
        }
        
    }
}