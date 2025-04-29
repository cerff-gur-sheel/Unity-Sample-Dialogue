using UnityEngine;

namespace SampleDialogue.Ellements
{
    [CreateAssetMenu(fileName = "Parse", menuName = "Scriptable Objects/Parse")]
    public class Parse : ScriptableObject
    {
        public string text;
        public string character;
        public Sprite face;
    }
}