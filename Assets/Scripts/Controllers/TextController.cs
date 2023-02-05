using UnityEngine;
using System.IO;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextController : MonoBehaviour, IController
{
    private static TextController instance;

    private string json;
    private const string LANGUAGE = "ua_UKR.json";

    public static UIText uiText = new UIText();
    public static Items items = new Items();

    [Header("Message")]
    public GameObject warningPanel;
    public GameObject newDialogPanel;
    public GameObject pauseMenu;
    [SerializeField] private EndGame endGamePanel;
    [SerializeField] private TextMeshProUGUI endMessage;
    [SerializeField] private TextMeshProUGUI error;
    public GameObject openInventar;
    [SerializeField] private GameObject errorWindow;
    [SerializeField] private GameObject loadingWindow;
    [SerializeField] private Slider slider;

    public LoadStatusEnum Status { get; private set; }

    public void StartUp()
    {
        instance = this;
        Debug.Log("SaveController.sv.isFirstGame: ");
        Debug.Log(SaveController.sv.isFirstGame);
        Debug.Log("GlobalVariablesConstants.FIRST_GAME: ");
        Debug.Log(((Ink.Runtime.IntValue)DialogueController
                    .GetInstance()
                    .GetVariableState(GlobalVariablesConstants.FIRST_GAME)).value);
        warningPanel.SetActive(false);
        endGamePanel.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);

        if (SceneManager.GetActiveScene().name != "Lobbi" && !SaveController.sv.isFirstGame)
            openInventar.SetActive(false);

        if (((Ink.Runtime.IntValue)DialogueController
                    .GetInstance()
                    .GetVariableState(GlobalVariablesConstants.FIRST_GAME)).value == 1 && 
                    SceneManager.GetActiveScene().name == "Lobbi")
        {
            newDialogPanel.SetActive(true);
        }
        else
        {
            newDialogPanel.SetActive(false);
        }

        LangLoad();

        Status = LoadStatusEnum.IsLoaded;
    }

    public static TextController GetInstance()
    {
        return instance;
    }

    void LangLoad()
    {
#if !UNITY_EDITOR
        string path = Path.Combine(Application.streamingAssetsPath, "UI text/" + LANGUAGE);
        WWW reader = new WWW(path);
        while(!reader.isDone) { }
        json = reader.text;
        uiText = JsonUtility.FromJson<UIText>(json);

        path = Path.Combine(Application.streamingAssetsPath, "Items/" + LANGUAGE);
        reader = new WWW(path);
        while(!reader.isDone) { }
        json = reader.text;
        items = JsonUtility.FromJson<Items>(json);
#endif
#if UNITY_EDITOR
        json = File.ReadAllText(Application.streamingAssetsPath + "/UI text/" + LANGUAGE);
        uiText = JsonUtility.FromJson<UIText>(json);

        json = File.ReadAllText(Application.streamingAssetsPath + "/Items/" + LANGUAGE);
        items = JsonUtility.FromJson<Items>(json);
#endif
    }

    public void Update()
    {
        LangLoad();

        if(((Ink.Runtime.IntValue)DialogueController
                    .GetInstance()
                    .GetVariableState("open_error")).value == 1 &&
                    ((Ink.Runtime.IntValue)DialogueController
                    .GetInstance()
                    .GetVariableState(GlobalVariablesConstants.WHAT_BE_AFTER_DESTROY)).value == 1) 
        {
            if (errorWindow != null) errorWindow.SetActive(true);
            SoundController.GetInctanse().MuteSound();
        }
        else if (((Ink.Runtime.IntValue)DialogueController
                    .GetInstance()
                    .GetVariableState("open_error")).value == 0)
        {
            if(errorWindow != null) errorWindow.SetActive(false);
            SoundController.GetInctanse().UnMuteSound();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundController.GetInctanse().MuteSound();
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }

        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.R))
        {
            InventarController.GetInstance().ResetData();
            Application.Quit();
        }
    }

    public void WriteErrorMessage(string ex)
    {
        //error.text += ex;
    }

    public void OpenEndGamePanel(int index)
    {
        SoundController.GetInctanse().MuteSound();

        foreach (var item in GameObject.FindGameObjectsWithTag("Item"))
        {
            Destroy(item);
        }

        endGamePanel.ScenarioIndex = index;
        endMessage.text = uiText.EndText[index];
        Time.timeScale = 0;
        endGamePanel.gameObject.SetActive(true);
    }

    public IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation asyncLoad;

        try
        {
            asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            loadingWindow.SetActive(true);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("LoadAsync");
            Debug.LogError(ex);
            WriteErrorMessage(ex.Message);
            throw ex;
        }

        while (!asyncLoad.isDone)
        {
            slider.value = asyncLoad.progress;

            yield return null;
        }
    }
}

public class UIText
{
    public string OpenDoor;
    public string OpenDialog;

    public string[] EndText = new string[3];
}
public class Items
{
    public string[] ItemName = new string[10];
    public string[] ItemDis = new string[10];
}