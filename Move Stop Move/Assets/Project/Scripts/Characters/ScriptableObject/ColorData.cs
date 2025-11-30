using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Character.ScriptableObject
{
    public enum ColorType { Default, Black, Red, Blue, Green, Yellow, Orange, Brown, Violet }
    [CreateAssetMenu(fileName = "ColorData", menuName = "ScriptableObjects/ColorData", order = 1)]
    public class ColorData : UnityEngine.ScriptableObject
    {
        [SerializeField] Material[] colorMats;

        public Material GetColorMat(ColorType colorType)
        {
            return colorMats[(int)colorType];
        }
    }
}