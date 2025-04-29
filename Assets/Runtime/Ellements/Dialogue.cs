using System.Collections.Generic;
using UnityEngine;

namespace SampleDialogue.Ellements
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        public List<Parse> Parses;
        public List<Choice> Choices;
    }
}
