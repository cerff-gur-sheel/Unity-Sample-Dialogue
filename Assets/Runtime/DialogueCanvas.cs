// This script is used to manage the dialogue and choice UI in a game.
// It handles the display of dialogue text, character names, and images,
// as well as the selection of dialogue options.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SampleDialogue.Runtime
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
        private int _currentTextIndex;

        /// <summary>
        /// The canvas displaying the dialogue.
        /// </summary>
        [Header("Dialogue UI")] [SerializeField]
        private GameObject dialogueCanvas;

        [SerializeField] private EventPlayer eventPlayer;

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

        /// <summary>
        /// The canvas displaying the choice options.
        /// </summary>
        [Header("Choice UI")] [SerializeField] private GameObject choiceCanvas;

        /// <summary>
        /// The buttons representing the choice options.
        /// </summary>
        [SerializeField] private GameObject[] choiceButtons;

        /// <summary>
        /// The text elements for each choice button.
        /// </summary>
        private TextMeshProUGUI[] _choiceTexts;

        private bool _writingText;
        private float _speed;
        private Coroutine _writeCoroutine;

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
        private void UpdateDialogueFields()
        {
            if (_writingText)
                return;

            dialogueCanvas.SetActive(true);
            choiceCanvas.SetActive(false);

            var currentText = _currentNode.Texts[_currentTextIndex];
            characterName.text = currentText.Character;
            dialogueImage.sprite = CharacterSprite(currentText.Character, currentText.Emotion);
            dialogueImage.SetNativeSize();
            _writeCoroutine = StartCoroutine(WriteText((currentText.Content)));
            if (currentText.Event != null) eventPlayer.PlayEvent(currentText.Event);
            return;

            IEnumerator WriteText(string rawText)
            {
                dialogueText.text = "";
                _speed = 25f;
                var i = 0;
                _writingText = true;

                while (i < rawText.Length)
                {
                    //  Detect commands like :cmd(param)
                    if (rawText[i] == ':' && Regex.IsMatch(rawText[i..], @"^:([a-z]+)\((.*?)\)"))
                    {
                        if (i >= 0 && i <= rawText.Length)
                        {
                            var match = Regex.Match(rawText[i..], @"^:([a-z]+)\((.*?)\)");
                            var cmd = match.Groups[1].Value;
                            var param = match.Groups[2].Value;

                            switch (cmd)
                            {
                                case "spd":
                                    if (float.TryParse(param, out var newSpeed))
                                        _speed = newSpeed;
                                    break;

                                case "wait":
                                    if (float.TryParse(param, out var waitTime))
                                        yield return new WaitForSeconds(waitTime / 1000);
                                    break;

                                case "col":
                                    dialogueText.text += $"<color={param}>";
                                    break;

                                case "dec":
                                    dialogueText.text += param switch
                                    {
                                            "bold" => "<b>",
                                            "italic" => "<i>",
                                            "nbold" => "</b>",
                                            "nitalic" => "</i>",
                                            _ => ""
                                    };
                                    break;

                                case "endcol":
                                    dialogueText.text += "</color>";
                                    break;

                                case "enddec":
                                    dialogueText.text += "</b></i>"; // close both
                                    break;

                                // ADD NEW COMMANDS HERE

                                default:
                                    throw new ArgumentOutOfRangeException($"Unkown command: {cmd}");
                            }

                            i += match.Length;
                        }

                        continue;
                    }

                    dialogueText.text += rawText[i];
                    yield return new WaitForSeconds(_speed / 1000);
                    i++;
                }

                _writingText = false;
            }

            static Sprite CharacterSprite(string characterName, string emotion)
            {
                if (string.IsNullOrEmpty(characterName) || string.IsNullOrEmpty(emotion)) return null;

                var path = $"CharacterSprites/{characterName}/{emotion}";

                var sprite = Resources.Load<Sprite>(path);
                if (sprite == null)
                    Debug.LogError($"Sprite not found in resources: {path}");

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
            _writingText = false;
            StopCoroutine(_writeCoroutine);
            
            var options = _currentNode.Options;
            for (var i = 0; i < options.Length; i++)
            {
                choiceButtons[i].SetActive(true);
                _choiceTexts[i].text = options[i].Content;
                _choiceOptions.Add(_dialogueFile.Nodes[options[i].NextNodeID - 1]);
            }
        }

        /// <summary>
        /// Advances to the next dialogue text or displays choice options if available.
        /// </summary>
        public void Next()
        {
            if (_writingText)
            {
                _speed = 1f;
                return;
            }
            
            if (_currentTextIndex >= _currentNode.Texts.Length - 1)
            {
                // If there are options, show them
                // If no more texts or options, hide the dialogue canvas
                if (_currentNode.Options.Length > 0) UpdateChoiceFields();
                else dialogueCanvas.SetActive(false);
            }
            else
            {
                // If there are more texts, show the next one
                _currentTextIndex++;
                UpdateDialogueFields();
            }
        }

        /// <summary>
        /// Moves back to the previous dialogue text if possible.
        /// </summary>
        public void Previous()
        {
            if (_currentTextIndex <= 0) return;
            _writingText = false;
            StopCoroutine(_writeCoroutine);
            _currentTextIndex--;
            UpdateDialogueFields();
        }

        /// <summary>
        /// Selects a dialogue option and updates the dialogue to the selected node.
        /// </summary>
        /// <param name="index">The index of the selected option.</param>
        public void SelectOption(int index)
        {
            _currentNode = _choiceOptions[index];
            _currentTextIndex = 0;
            UpdateDialogueFields();
        }

        /// <summary>
        /// Starts a new dialogue using the provided dialogue tree.
        /// </summary>
        /// <param name="dialogueFile">The dialogue tree to start.</param>
        public void StartDialogue(TextAsset dialogueFile)
        {
            _dialogueFile = DialogueLoader.LoadDialogue(dialogueFile);

            _currentTextIndex = 0;
            _currentNode = _dialogueFile.Nodes[_currentTextIndex];
            UpdateDialogueFields();
        }
    }
}