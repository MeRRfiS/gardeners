using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour, IController
{
    public static Save sv = new Save();
    public static Price price = new Price();

    private readonly string saveFileName = "Save.json";
    private string pathSave;

    public LoadStatusEnum Status { get; private set; }

    public void StartUp()
    {
#if !UNITY_EDITOR
        pathSave = Path.Combine(Application.persistentDataPath, saveFileName);

        var reader = new WWW(pathSave);
        while(!reader.isDone) { }
        var json = reader.text;
        sv = JsonUtility.FromJson<Save>(json);
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
    
}

[Serializable]
public class Price
{
    public Dictionary<ItemIds, Dictionary<ItemIds, int>> itemPrice = new Dictionary<ItemIds, Dictionary<ItemIds, int>>();

    public Price()
    {
        itemPrice.Add(ItemIds.TechGloves, new Dictionary<ItemIds, int>()
        { 
            { ItemIds.Web, 20 },
            { ItemIds.TreeSap, 5 }
        });
        itemPrice.Add(ItemIds.SensoryBoots, new Dictionary<ItemIds, int>()
        {
            { ItemIds.MonsterApplePits, 150 },
            { ItemIds.Web, 10 }
        });
        itemPrice.Add(ItemIds.Blade, new Dictionary<ItemIds, int>()
        {
            { ItemIds.TreeSap, 20 },
            { ItemIds.APieceOfForestRoot, 5 },
            { ItemIds.MonsterApplePits, 100 },
        });
        itemPrice.Add(ItemIds.NATOBulletproofVest, new Dictionary<ItemIds, int>()
        {
            { ItemIds.APieceOfForestRoot, 10 },
            { ItemIds.MonsterApplePits, 75 },
            { ItemIds.Web, 30 },
        });
    }
}
