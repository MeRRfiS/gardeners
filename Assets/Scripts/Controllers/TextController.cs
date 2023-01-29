using UnityEngine;
using System.IO;

public class TextController : MonoBehaviour
{
    private string json;
    private const string LANGUAGE = "ua_UKR.json";

    public static UIText uiText = new UIText();

    void Awake()
    {
        LangLoad();
    }

    void LangLoad()
    {
#if !UNITY_EDITOR
        string path = Path.Combine(Application.streamingAssetsPath, "UI text/" + LANGUAGE);
        WWW reader = new WWW(path);
        while(!reader.isDone) { }
        json = reader.text;
        uiText = JsonUtility.FromJson<UIText>(json);
#endif
#if UNITY_EDITOR
        json = File.ReadAllText(Application.streamingAssetsPath + "/UI text/" + LANGUAGE);
        uiText = JsonUtility.FromJson<UIText>(json);
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