using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// The DialogueSystem class is responsible for managing dialogues in the game.
/// </summary>
public class DialogueSystem : MonoBehaviour
{
    /// <summary>
    /// The DialogueType enum is used to define the types of dialogues available in the game.
    /// </summary>
    public enum DialogueType
    {   /// <summary>
        /// The TUORIAL element is to define the tutorial's dialogue 
        /// </summary>
        TUTORIAL
    }

    /// <summary>
    /// The dialogueJsonFile attribute is used to reference the JSON file that contains the dialogues.
    /// </summary>
    [SerializeField]
    private TextAsset dialogueJsonFile;

    /// <summary>
    /// The skipDialogue attribute is used to reference the skip dialogue action from the InputActionAsset.
    /// </summary>
    [SerializeField]
    private InputActionReference skipDialogue;

    /// <summary>
    /// The dialogueBox attribute is used to reference the dialogue box GameObject that will be displayed during dialogues.
    /// </summary>
    [SerializeField]
    private GameObject dialogueBox;

    /// <summary>
    /// The dialogueText attribute is used to reference the TextMeshProUGUI component that will display the dialogue text in the dialogue box.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI dialogueText;

    /// <summary>
    /// The currentDialogue attribute is used to store the type of the current dialogue being displayed.
    /// </summary>
    private string currentDialogueType;

    /// <summary>
    /// The isDialogueActive attribute is a flag used to check if a dialogue is currently active or not.
    /// </summary>
    private bool isDialogueActive;

    /// <summary>
    /// The dialoguesLines attribute is a list that stores the lines of dialogue for the current dialogue.
    /// </summary>
    private List<string> dialoguesLines;

    /// <summary>
    /// The firstPersonController attribute is used to reference the FirstPersonController (Player Controller) component.
    /// </summary>
    private FirstPersonController firstPersonController;

    /// <summary>
    /// The Awake Method is called when the script instance is being loaded (Unity callback).
    /// In this method the firstPersonController attribute is initialized.
    /// </summary>

    private void Awake()
    {
        firstPersonController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
    }

    /// <summary>
    /// The Update method is called once per frame (Unity callback).
    /// In this method its check if the dialogue is active and if the skipDialogue action was triggered.
    /// If these conditions are met, the SkipDialogue method is called to skip the current line of dialogue.
    /// </summary>
    private void Update()
    {
        if (isDialogueActive && skipDialogue.action.triggered)
        {
            SkipDialogue();
        }
    }

    /// <summary>
    /// The SkipDialogue method is responsible for skipping the current line of dialogue and showing the next line.
    /// If the are no more lines of dialogue, it calls the EndDialogue method to handle the dialogue's end logic.
    /// </summary>
    private void SkipDialogue()
    {
        dialoguesLines.RemoveAt(0);  

        if (dialoguesLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        // Shows the next line of dialogue  
        dialogueText.text = dialoguesLines[0]; 
    }

    /// <summary>
    /// The EndDialogue method is responsible for handling the end of a dialogue (deactivating the dialogue box and resuming the game if it was a tutorial).
    /// </summary>
    private void EndDialogue()
    {
        dialogueBox.SetActive(false);
        isDialogueActive = false;

        if (currentDialogueType == "tutorial")
        {
            Time.timeScale = 1; // Resume the game if the dialogue was a tutorial
            firstPersonController.enabled = true; // Re-enable the player controller
        }
    }

    /// <summary>
    /// The GetDialogueType method is responsible for returning the string representation of the dialogue type based on the DialogueType enum.
    /// </summary>
    /// <param name="type">The type of dialogue</param>
    /// <returns> A string containing the name of the dialogue's type</returns>
    private string GetDialogueType(DialogueType type)
    {
        return type switch
        {
            DialogueType.TUTORIAL => "tutorial",
            _ => "unknown"
        };
    }

    /// <summary>
    /// The StartDialogue method is responsible for starting a dialogue based on the provided DialogueType.
    /// </summary>
    /// <remarks>
    /// This methods gets the name of the dialogue type, deserializes the JSON file to get the lines of dialogue for the current dialogue,
    /// then activates the dialogue box and sets the first line of dialogue to be displayed.
    /// If the dialogue type is a tutorial, it pauses the game and disables the player controller to prevent player movement during the dialogue.
    /// </remarks>
    /// <param name="type"></param>
    public void StartDialogue(DialogueType type)
    {
        currentDialogueType = GetDialogueType(type);

        dialoguesLines = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(dialogueJsonFile.text)[currentDialogueType];

        dialogueBox.SetActive(true);
        dialogueText.text = dialoguesLines[0];
        isDialogueActive = true;

        if (currentDialogueType == "tutorial")
        {
            Time.timeScale = 0; // Pause the game if the dialogue is a tutorial
            firstPersonController.enabled = false;
        }
    }
}
