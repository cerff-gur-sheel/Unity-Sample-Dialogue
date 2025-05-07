using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SampleDialogue.Assets.Runtime
{
  /// <summary>
  /// Handles the UI logic for displaying dialogues and choices in the game.
  /// </summary>
  public class DialogueCanvas : MonoBehaviour
  {
    /// <summary>
    /// The dialogue tree currently being used.
    /// </summary>
    private DialogueTree _dialogueFile;

    /// <summary>
    /// The list of choice options available in the current dialogue node.
    /// </summary>
    private List<DialogueNode> _choiceOptions;

    /// <summary>
    /// The current dialogue node being displayed.
    /// </summary>
    private DialogueNode _currentNode;

    /// <summary>
    /// The index of the current text within the dialogue node.
    /// </summary>
    private int _currentDialogueNodeIndex = 0;

    [Header("Dialogue UI")]
    /// <summary>
    /// The canvas displaying the dialogue.
    /// </summary>
    [SerializeField] private GameObject dialogueCanvas;

    /// <summary>
    /// The text element displaying the dialogue content.
    /// </summary>
    [SerializeField] private TextMeshProUGUI dialogueText;

    /// <summary>
    /// The text element displaying the character's name.
    /// </summary>
    [SerializeField] private TextMeshProUGUI characterName;

    /// <summary>
    /// The image element displaying the character's emotion or portrait.
    /// </summary>
    [SerializeField] private Image dialogueImage;

    [Header("Choice UI")]
    /// <summary>
    /// The canvas displaying the choice options.
    /// </summary>
    [SerializeField] private GameObject choiceCanvas;

    /// <summary>
    /// The buttons representing the choice options.
    /// </summary>
    [SerializeField] private GameObject[] choiceButtons;

    /// <summary>
    /// The text elements for each choice button.
    /// </summary>
    private TextMeshProUGUI[] _choiceTexts;

    /// <summary>
    /// Initializes the dialogue and choice UI elements.
    /// </summary>
    private void Awake()
    {
      // Initialize choice texts
      _choiceTexts = new TextMeshProUGUI[choiceButtons.Length];
      for (var i = 0; i < choiceButtons.Length; i++)
      {
        _choiceTexts[i] = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
        choiceButtons[i].SetActive(false);
      }

      // Initialize dialogue and choice states
      _choiceOptions = new List<DialogueNode>();
      dialogueCanvas.SetActive(false);
      choiceCanvas.SetActive(false);
    }

    /// <summary>
    /// Updates the dialogue UI fields with the current dialogue text.
    /// </summary>
    private void UpdateDIalogueFields()
    {
      dialogueCanvas.SetActive(true);
      choiceCanvas.SetActive(false);

      var currentText = _currentNode.Texts[_currentDialogueNodeIndex];
      characterName.text = currentText.Character;
      dialogueText.text = currentText.Content;
      dialogueImage.sprite = CharacterSprite(currentText.Character, currentText.Emotion);

      static Sprite CharacterSprite(string characterName, string emotion)
      {
        // Check if characterName or emotion is empty and throw an exception if true
        if (string.IsNullOrEmpty(characterName) || string.IsNullOrEmpty(emotion))
        {
          // uses default Image
          return null;
        }

        // Construct the path to the character sprite based on the character name and emotion
        // Note: This assumes the sprites are stored in a specific folder structure 
        // (Assets/StreamingAssets/CharacterSprites/{characterName}/{emotion}.png)
        // Adjust the path as necessary to match your project structure
        var path = System.IO.Path.Combine(
          Application.streamingAssetsPath,
          "CharacterSprites",
          characterName,
          emotion + ".png"
        );

        // Load the sprite from the specified path
        var sprite = Resources.Load<Sprite>(path);
        return sprite;
      }
    }

    /// <summary>
    /// Updates the choice UI fields with the current dialogue options.
    /// </summary>
    private void UpdateChoiceFields()
    {
      dialogueCanvas.SetActive(false);
      choiceCanvas.SetActive(true);

      _choiceOptions.Clear();

      var options = _currentNode.Options;
      for (var i = 0; i < options.Length; i++)
      {
        choiceButtons[i].SetActive(true);
        _choiceTexts[i].text = options[i].Text;
        _choiceOptions.Add(_dialogueFile.Nodes[options[i].NextNodeID]);
      }
    }

    /// <summary>
    /// Advances to the next dialogue text or displays choice options if available.
    /// </summary>
    public void Next()
    {
      if (_currentDialogueNodeIndex >= _currentNode.Texts.Length - 1)
      {
        // If there are options, show them
        // If no more texts or options, hide the dialogue canvas
        if (_currentNode.Options.Length > 0) UpdateChoiceFields();
        else dialogueCanvas.SetActive(false);
      }
    }

    /// <summary>
    /// Moves back to the previous dialogue text if possible.
    /// </summary>
    public void Previous()
    {
      if (_currentDialogueNodeIndex > 0)
      {
        _currentDialogueNodeIndex--;
        UpdateDIalogueFields();
      }
    }

    /// <summary>
    /// Selects a dialogue option and updates the dialogue to the selected node.
    /// </summary>
    /// <param name="index">The index of the selected option.</param>
    public void SelectOption(int index)
    {
      _currentNode = _choiceOptions[index];
      _currentDialogueNodeIndex = 0;
      UpdateDIalogueFields();
    }

    /// <summary>
    /// Starts a new dialogue using the provided dialogue tree.
    /// </summary>
    /// <param name="dialogueFile">The dialogue tree to start.</param>
    public void StartDialogue(DialogueTree dialogueFile)
    {
      _dialogueFile = dialogueFile;
      _currentDialogueNodeIndex = 0;
      _currentNode = _dialogueFile.Nodes[_currentDialogueNodeIndex];

      UpdateDIalogueFields();
    }
  }
}