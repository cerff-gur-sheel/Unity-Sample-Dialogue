using System.Collections.Generic;
using UnityEngine;

namespace SampleDialogue.Ellements
{
    [CreateAssetMenu(fileName = "Choice", menuName = "Scriptable Objects/Choice")]
    public class Choice : ScriptableObject
    {
        public string text;
        public Dialogue next;
    }
}
