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

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueTextDisplay;

    private Story currentStory;

    [Header("Choices UI")]
    [SerializeField] private GameObject choisesScroll;
    [SerializeField] private Transform content;
    [SerializeField] private Button buttonPrefab;
    private List<GameObject> choices = new List<GameObject>();
    public bool dialogueIsPlaying { get; private set; }

    public int choiceIndex { private get; set; }

    private TextMeshProUGUI[] choicesText;

    public LoadStatusEnum Status { get; private set; }

    public void StartUp()
    {
        if(instance != null)
        {
            Debug.LogWarning("Found more than one Dialog Controller in the scene");
        }
        instance = this;

        Status = LoadStatusEnum.IsLoaded;
    }

    public static DialogueController GetInstance()
    {
        return instance;
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueTextDisplay.text = "";
        playerController.IsCanMove = true;
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueTextDisplay.text = currentStory.Continue();
            DisplayChoices();
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        for (int i = 0; i < currentChoices.Count; i++)
        {
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
        choisesScroll.SetActive(false);
        currentStory.ChooseChoiceIndex(choiceIndex);
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ContinueStory();
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
