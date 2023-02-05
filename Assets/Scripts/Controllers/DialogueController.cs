using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour, IController
{
    private static DialogueController instance;
    private PlayerController playerController;

    private DialogueVariables dialogueVariables;

    [Header("Load Globals JSON")]
    [SerializeField] private TextAsset loadGlobalJSON;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueTextDisplay;
    [SerializeField] private Image icon;

    private Story currentStory;

    [Header("Choices UI")]
    [SerializeField] private GameObject choisesScroll;
    [SerializeField] private Transform content;
    [SerializeField] private Button buttonPrefab;
    private List<GameObject> choices = new List<GameObject>();
    public bool dialogueIsPlaying { get; private set; }
    private bool answerIsOpen = false;

    public int choiceIndex { private get; set; }

    private CharactersEnum character;

    private TextMeshProUGUI[] choicesText;

    public LoadStatusEnum Status { get; private set; }

    public void StartUp()
    {
        if(instance != null)
        {
            Debug.LogWarning("Found more than one Dialog Controller in the scene");
        }
        instance = this;

        dialogueVariables = new DialogueVariables(loadGlobalJSON);

        Status = LoadStatusEnum.IsLoaded;
    }

    public static DialogueController GetInstance()
    {
        return instance;
    }

    private void SetCharacterIcon()
    {
        int index = ((IntValue)GetVariableState(GlobalVariablesConstants.CHARACTER_INDEX)).value;
        icon.sprite = Resources.Load<Sprite>($"CharacterIcon/{index}");
    }

    public void EnterDialogueMode(CharactersEnum ch)
    {
        var json = (Resources.Load($"Dialogue/{ch.ToString()}")) as TextAsset;
        SetCharacterIcon();
        currentStory = new Story(json.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        dialogueVariables.StartListening(currentStory);

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueVariables.StopListening(currentStory);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueTextDisplay.text = "";

        PlayerController.GetInstance().IsCanMove = true;
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            SetCharacterIcon();
            dialogueTextDisplay.text = currentStory.Continue();
            SoundController.GetInctanse().PlayDialog();
            DisplayChoices();
        }
        else
        {
            if (((Ink.Runtime.IntValue)GetVariableState(GlobalVariablesConstants.GET_KVZHP)).value == 1)
            {
                var item = new ItemsModel((int)ItemIds.KVZHP,
                                           TextController.items.ItemName[(int)ItemIds.KVZHP],
                                           ItemsTypeEnum.Tool);
                InventarController.GetInstance().AddNewItemTool(item);
                TextController.GetInstance().openInventar.SetActive(true);
                SetVariableState(GlobalVariablesConstants.GET_KVZHP, new IntValue(2));
            }
            if (((Ink.Runtime.BoolValue)GetVariableState(GlobalVariablesConstants.OPEN_SHOP)).value)
            {
                InventarController.GetInstance().OpenShop();
                SetVariableState(GlobalVariablesConstants.OPEN_SHOP, new BoolValue(false));
            }

            ExitDialogueMode();
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        for (int i = 0; i < currentChoices.Count; i++)
        {
            if (currentChoices[i].text == "") continue;
            var buttonObj = Instantiate(buttonPrefab, content);
            buttonObj.gameObject.SetActive(false);
            buttonObj.GetComponent<ChoiceIndex>().Index = i;
            choices.Add(buttonObj.gameObject);
        }

        choicesText = new TextMeshProUGUI[choices.Count];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
        
        index = 0;
        foreach (Choice choice in currentChoices)
        {
            if (choice.text == "") continue;
            answerIsOpen = true;
            choisesScroll.SetActive(true);
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Count; i++)
        {
            Destroy(choices[i]);
        }
        if (currentChoices.Count == 0)
        {
            choices.Clear();
        }

        StartCoroutine(SelectFirstChoice());
    }

    public void MakeChoice()
    {
        answerIsOpen = false;
        choisesScroll.SetActive(false);
        currentStory.ChooseChoiceIndex(choiceIndex);
        Debug.Log("I am here!");
        ContinueStory();
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        playerController = GameObject.Find(ControllerConstants.PLAYER_CONTROLLER).GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!dialogueIsPlaying) return;

        //Debug.Log(answerIsOpen);
        if (Input.GetMouseButtonDown(0) && !answerIsOpen)
        {
            ContinueStory();
        }
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if(variableValue == null)
        {
            Debug.LogWarning("Ink variable was found to be null: " + variableName);
        }

        return variableValue;
    }

    public void SetVariableState(string variableName, Ink.Runtime.Object variableValue)
    {
        dialogueVariables.VariableChanged(variableName, variableValue);
        if (dialogueVariables != null)
        {
            dialogueVariables.SaveVariables();
        }
    }

    public void OnApplicationQuit()
    {
        if(dialogueVariables != null)
        {
            dialogueVariables.SaveVariables();
        }
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        try
        {
            EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
        }
        catch (Exception)
        {

        }
    }
}
