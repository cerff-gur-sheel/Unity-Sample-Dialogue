// This is a test script that starts the dialogue when the game starts.
// It loads the dialogue tree from an XML file and displays it using the DialogueCanvas component.
// Use this script as a example to start the dialogue in your game.
// Don't forget to add the XML file to the Resources folder in Unity.
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