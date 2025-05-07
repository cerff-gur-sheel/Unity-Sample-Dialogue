using System;
using System.Xml;
using SampleDialogue.Assets.Runtime;
using UnityEngine;

public class StartDialogue : MonoBehaviour
{
  /// <summary>
  /// The dialogue tree to be loaded and displayed.
  /// </summary>
  [SerializeField] private TextAsset dialogueFile;

  /// <summary>
  /// The DialogueCanvas component responsible for displaying the dialogue.
  /// </summary>
  [SerializeField] private DialogueCanvas _dialogueCanvas;

  private void Start()
  {
    // Load the dialogue tree from the XML file
    Debug.Log("a");
    _dialogueCanvas.StartDialogue(dialogueFile);
    Debug.Log("Dialogue started");
  }
}