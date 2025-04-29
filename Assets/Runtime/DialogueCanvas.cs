using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SampleDialogue.Ellements;

namespace SampleDialogue
{
    /// <summary>
    /// Handles the UI logic for displaying dialogues and choices in the game.
    /// </summary>
    public class DialogueCanvas : MonoBehaviour
    {
        // Dialogue data
        private Dialogue _dialogueFile;
        private List<Dialogue> _choiceOptions;
        private int _currentDialogueIndex = 0;

        // Dialogue UI elements
        [Header("Dialogue UI")]
        [SerializeField] private GameObject dialogueCanvas;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private TextMeshProUGUI characterName;
        [SerializeField] private Image dialogueImage;

        // Choice UI elements
        [Header("Choice UI")]
        [SerializeField] private GameObject choiceCanvas;
        [SerializeField] private GameObject[] choiceButtons;

        private TextMeshProUGUI[] _choiceTexts;

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
            _choiceOptions = new List<Dialogue>();
            dialogueCanvas.SetActive(false);
            choiceCanvas.SetActive(false);
        }

        /// <summary>
        /// Starts displaying the dialogue from a given dialogue file.
        /// </summary>
        /// <param name="dialogueFile">The dialogue file to load.</param>
        public void StartDialogue(Dialogue dialogueFile)
        {
            _dialogueFile = dialogueFile;
            _currentDialogueIndex = 0;
            UpdateDialogueFields();
        }

        /// <summary>
        /// Displays the next dialogue entry, if available.
        /// </summary>
        public void Next()
        {
            _currentDialogueIndex++;
            UpdateDialogueFields();
        }

        /// <summary>
        /// Returns to the previous dialogue entry.
        /// </summary>
        public void Previous()
        {
            _currentDialogueIndex--;
            UpdateDialogueFields();
        }

        /// <summary>
        /// Shows the available dialogue choices.
        /// </summary>
        public void ShowChoices()
        {
            _choiceOptions.Clear();
            
            for (var i = 0; i < _choiceTexts.Length; i++)
            {
                choiceButtons[i].SetActive(true);
                _choiceTexts[i].text = _dialogueFile.Choices[i].text;
                _choiceOptions.Add(_dialogueFile.Choices[i].next);
            }

            dialogueCanvas.SetActive(false);
            choiceCanvas.SetActive(true);
        }

        /// <summary>
        /// Selects a dialogue path from the choices.
        /// </summary>
        /// <param name="option">The selected choice index.</param>
        public void SelectChoice(int option)
        {
            _dialogueFile = _choiceOptions[option];
            _currentDialogueIndex = 0;
            choiceCanvas.SetActive(false);
            foreach (var t in choiceButtons) t.SetActive(false);
            UpdateDialogueFields();
        }

        /// <summary>
        /// Updates the dialogue UI with current dialogue data.
        /// </summary>
        private void UpdateDialogueFields()
        {
            dialogueCanvas.SetActive(true);
            choiceCanvas.SetActive(false);

            var currentParse = _dialogueFile.Parses[_currentDialogueIndex];
            dialogueText.text = currentParse.text;
            dialogueImage.sprite = currentParse.face;
            characterName.text = currentParse.name;
        }
    }
}