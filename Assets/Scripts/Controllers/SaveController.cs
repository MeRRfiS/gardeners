using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour, IController
{
    public static Save sv = new Save();

    private readonly string saveFileName = "Save.json";
    private string pathSave;

    public LoadStatusEnum Status { get; private set; }

    public void StartUp()
    {
#if !UNITY_EDITOR
        pathSave = Path.Combine(Application.persistentDataPath, saveFileName);

        var readerTC = new WWW(pathTimeCode);
        while(!readerTC.isDone) { }
        var jsonTC = readerTC.text;
        tc = JsonUtility.FromJson<TimeCodes>(jsonTC);
#else
        pathSave = Path.Combine(Application.dataPath, saveFileName);
#endif
        try
        {
            if (File.Exists(pathSave))
            {
                sv = JsonUtility.FromJson<Save>(File.ReadAllText(pathSave));
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Ha ha ha ha ha ha ha ha: {e.Message}");
        }

        Status = LoadStatusEnum.IsLoaded;
    }

#if !UNITY_EDITOR
    private void OnApplicationPause(bool pause)
    {
        if(pause) File.WriteAllText(pathSave, JsonUtility.ToJson(sv));
    }
#endif
    private void Update()
    {
        File.WriteAllText(pathSave, JsonUtility.ToJson(sv));
        if (File.Exists(pathSave))
            sv = JsonUtility.FromJson<Save>(File.ReadAllText(pathSave));
    }
    private void OnApplicationQuit()
    {
        File.WriteAllText(pathSave, JsonUtility.ToJson(sv));
    }
}

[Serializable]
public class Save
{
    public int IndexErnestDialog = 0;
    public int SubIndexErnestDialog = 0;

    public Dictionary<CharactersEnum, int> IndexDialog = new Dictionary<CharactersEnum, int>();
    public Dictionary<CharactersEnum, int> SubIndexDialog = new Dictionary<CharactersEnum, int>();

    public Save()
    {
        IndexDialog.Add(CharactersEnum.Ernest, IndexErnestDialog);

        SubIndexDialog.Add(CharactersEnum.Ernest, SubIndexErnestDialog);
    }
}
