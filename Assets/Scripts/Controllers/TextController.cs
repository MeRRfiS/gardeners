using UnityEngine;
using System.IO;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class TextController : MonoBehaviour, IController
{
    private string json;
    private const string LANGUAGE = "ua_UKR.json";

    public static UIText uiText = new UIText();
    public static Items items = new Items();

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
    }
}

public class UIText
{
    public string OpenDoor;
    public string OpenDialog;
}
public class Items
{
    public string[] ItemName = new string[1];
    public string[] ItemDis = new string[2];
}