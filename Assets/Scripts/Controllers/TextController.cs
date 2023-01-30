using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class TextController : MonoBehaviour, IController
{
    private string json;
    private const string LANGUAGE = "ua_UKR.json";

    public static UIText uiText = new UIText();
    public static Dialogs dial = new Dialogs();
    public static Answers ans = new Answers();

    [Header("Components")]
    [SerializeField] private GameObject dialogObj;

    [Header("Dialog's components")]
    [SerializeField] private GameObject dialogContent;
    [SerializeField] private GameObject answerContent;

    public LoadStatusEnum Status { get; private set; }

    public void StartUp()
    {
        LangLoad();

        Status = LoadStatusEnum.IsLoaded;
    }

    void LangLoad()
    {
#if !UNITY_EDITOR
        string path = Path.Combine(Application.streamingAssetsPath, "UI text/" + LANGUAGE);
        WWW reader = new WWW(path);
        while(!reader.isDone) { }
        json = reader.text;
        uiText = JsonUtility.FromJson<UIText>(json);

        path = Path.Combine(Application.streamingAssetsPath, "Dialogs/" + LANGUAGE);
        reader = new WWW(path);
        while(!reader.isDone) { }
        json = reader.text;
        dial = JsonUtility.FromJson<Dialogs>(json);

        path = Path.Combine(Application.streamingAssetsPath, "Answer/" + LANGUAGE);
        reader = new WWW(path);
        while(!reader.isDone) { }
        json = reader.text;
        ans = JsonUtility.FromJson<Answers>(json);
#endif
#if UNITY_EDITOR
        json = File.ReadAllText(Application.streamingAssetsPath + "/UI text/" + LANGUAGE);
        uiText = JsonUtility.FromJson<UIText>(json);

        json = File.ReadAllText(Application.streamingAssetsPath + "/Dialogs/" + LANGUAGE);
        dial = JsonUtility.FromJson<Dialogs>(json);

        json = File.ReadAllText(Application.streamingAssetsPath + "/Answer/" + LANGUAGE);
        ans = JsonUtility.FromJson<Answers>(json);
#endif
    }

    public void Update()
    {
        LangLoad();
    }
}

public class UIText
{
    public string OpenDoor;
    public string OpenDialog;
}
public class Dialogs
{
    #region Ernest
    public string[] ErnestDialog_0_0_0 = new string[1];
    public string[] ErnestDialog_0_0_1 = new string[1];
    public string[] ErnestDialog_0_0_2 = new string[2];
    public string[] ErnestDialog_0_1_0 = new string[2];
    public string[] ErnestDialog_0_1_1 = new string[2];
    public string[] ErnestDialog_0_1_2 = new string[2];
    public string[] ErnestDialog_0_2_0 = new string[7];
    public string[] ErnestDialog_0_2_1 = new string[1];
    public string[] ErnestDialog_0_2_2 = new string[1]; 
    #endregion

    public Dictionary<string, string[]> ErnestDialog = new Dictionary<string, string[]>();

    public Dialogs()
    {
        ErnestDialog.Add("ErnestDialog_0_0_0", ErnestDialog_0_0_0);
        ErnestDialog.Add("ErnestDialog_0_0_1", ErnestDialog_0_0_1);
        ErnestDialog.Add("ErnestDialog_0_0_2", ErnestDialog_0_0_2);
        ErnestDialog.Add("ErnestDialog_0_1_0", ErnestDialog_0_1_0);
        ErnestDialog.Add("ErnestDialog_0_1_1", ErnestDialog_0_1_1);
        ErnestDialog.Add("ErnestDialog_0_1_2", ErnestDialog_0_1_2);
        ErnestDialog.Add("ErnestDialog_0_2_0", ErnestDialog_0_2_0);
        ErnestDialog.Add("ErnestDialog_0_2_1", ErnestDialog_0_2_1);
        ErnestDialog.Add("ErnestDialog_0_2_2", ErnestDialog_0_2_2);
    }
}
public class Answers
{
    #region Answer to Ernest
    public string[] AnswerToErnest_0_0 = new string[2];
    public string[] AnswerToErnest_0_1 = new string[2];
    public string[] AnswerToErnest_0_2 = new string[2];
    #endregion

    public Dictionary<string, string[]> AnswerToErnest = new Dictionary<string, string[]>();

    public Answers()
    {
        AnswerToErnest.Add("AnswerToErnest_0_0", AnswerToErnest_0_0);
        AnswerToErnest.Add("AnswerToErnest_0_1", AnswerToErnest_0_1);
        AnswerToErnest.Add("AnswerToErnest_0_2", AnswerToErnest_0_2);
    }
}