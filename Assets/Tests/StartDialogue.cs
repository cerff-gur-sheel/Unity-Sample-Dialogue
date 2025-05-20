using SampleDialogue.Runtime;
using UnityEngine;

namespace SampleDialogue.Tests
{
  public class StartDialogue : MonoBehaviour
  {
    /// <summary>
    /// The dialogue tree to be loaded and displayed.
    /// </summary>
    [SerializeField] private TextAsset dialogueFile;

    /// <summary>
    /// The DialogueCanvas component responsible for displaying the dialogue.
    /// </summary>
    [SerializeField] private DialogueCanvas dialogueCanvas;

    private void Start()
    {
      // Load the dialogue tree from the XML file
      dialogueCanvas.StartDialogue(dialogueFile);
    }
  }
}