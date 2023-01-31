using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventarController : MonoBehaviour, IController
{
    private static InventarController instance;

    private Dictionary<int, ItemsStruct> itemsTool = new Dictionary<int, ItemsStruct>();
    private Dictionary<int, ItemsStruct> itemsMaterial = new Dictionary<int, ItemsStruct>();

    private const string SAVE_TOOL_ITEMS_KEY = "ITEMS_TOOL";
    private const string SAVE_MATERIAL_ITEMS_KEY = "ITEMS_MATERIAL";

    [Header("Components")]
    public GameObject inventoryPanel;
    public GameObject informationPanel;
    [SerializeField] private GameObject buttonPrefab;

    [Header("Inventory components")]
    [SerializeField] private GameObject toolList;
    [SerializeField] private GameObject materialList;
    [SerializeField] private Transform toolContent;
    [SerializeField] private Transform materialContent;
    private List<GameObject> items = new List<GameObject>();

    [Header("Information components")]
    [SerializeField] private TextMeshProUGUI discription;

    public LoadStatusEnum Status { get; private set; }

    public void StartUp()
    {
        instance = this;
        inventoryPanel.SetActive(false);
        informationPanel.SetActive(false);

        if (PlayerPrefs.HasKey(SAVE_TOOL_ITEMS_KEY))
        {
            itemsTool = JsonConvert.DeserializeObject<Dictionary<int, ItemsStruct>>
                (PlayerPrefs.GetString(SAVE_TOOL_ITEMS_KEY));
        }
        if (PlayerPrefs.HasKey(SAVE_MATERIAL_ITEMS_KEY))
        {
            itemsMaterial = JsonConvert.DeserializeObject<Dictionary<int, ItemsStruct>>
                (PlayerPrefs.GetString(SAVE_MATERIAL_ITEMS_KEY));
        }

        Status = LoadStatusEnum.IsLoaded;
    }

    public static InventarController GetInstance()
    {
        return instance;
    }

    private void ClearItems()
    {
        if(items.Count != 0)
        {
            foreach (GameObject item in items)
            {
                Destroy(item);
            }
            items.Clear();
        }
    }

    public void OpenInformationPanel(int index)
    {
        discription.text = TextController.items.ItemDis[index];
        informationPanel.SetActive(true);
    }

    public void OpenToolList()
    {
        ClearItems();

        foreach (var item in itemsTool)
        {
            var button = Instantiate(buttonPrefab, toolContent);
            button.GetComponent<ItemButton>().ItemIndex = item.Value.Index;
            button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = 
                String.IsNullOrEmpty(item.Value.Name) ? TextController.items.ItemName[item.Value.Index] : item.Value.Name;
            button.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.Value.Count.ToString();
            items.Add(button);
        }

        materialList.SetActive(false);
        toolList.SetActive(true);
    }

    public void OpenMaterialList()
    {
        ClearItems();

        foreach (var item in itemsMaterial)
        {
            var button = Instantiate(buttonPrefab, materialContent);
            button.GetComponent<ItemButton>().ItemIndex = item.Value.Index;
            button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.Value.Name;
            button.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.Value.Count.ToString();
            items.Add(button);
        }

        toolList.SetActive(false);
        materialList.SetActive(true);
    }

    public void AddNewItemTool(ItemsStruct item)
    {
        if (!itemsTool.ContainsKey(item.Index))
        {
            itemsTool.Add(item.Index, item);
        }
    }

    public void AddNewItemMaterial(ItemsStruct item)
    {
        if (itemsMaterial.ContainsKey(item.Index))
        {
            itemsMaterial[item.Index] = new ItemsStruct(item.Index,
                                                        item.Name,
                                                        item.Type,
                                                        itemsMaterial[item.Index].Count + item.Count);
        }
        else
        {
            itemsMaterial.Add(item.Index, item);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (SceneManager.GetActiveScene().name == "Lobbi")
            {
                OpenToolList();
            }
            else
            {
                OpenMaterialList();
            }
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            PlayerController.GetInstance().IsCanMove = !inventoryPanel.activeSelf;
        }
    }

    private void OnApplicationQuit()
    {
        var jsonTool = JsonConvert.SerializeObject(itemsTool);
        var jsonMaterial = JsonConvert.SerializeObject(itemsMaterial);

        PlayerPrefs.SetString(SAVE_TOOL_ITEMS_KEY, jsonTool);
        PlayerPrefs.SetString(SAVE_MATERIAL_ITEMS_KEY, jsonMaterial);
    }
}
