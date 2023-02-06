using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventarController : MonoBehaviour, IController
{
    private static InventarController instance;

    public Dictionary<int, ItemsModel> itemsTool { get; private set; }
    public Dictionary<int, ItemsModel> itemsMaterial { get; private set; }
    public Dictionary<int, GoodsModel> itemsShop { get; private set; }

    //Select Items
    public List<ItemsModel> itemSelect { get; set; }
    public ItemsModel weaponSelect { get; set; }

    private const string SAVE_TOOL_ITEMS_KEY = "ITEMS_TOOL.json";
    private const string SAVE_MATERIAL_ITEMS_KEY = "ITEMS_MATERIAL.json";
    private const string SAVE_SHOP_ITEMS_KEY = "ITEMS_SHOP.json";
    private const string SAVE_SELECT_ITEMS_KEY = "SELECT_ITEMS.json";
    private const string SAVE_SELECT_WEAPON_KEY = "SELECT_WEAPON.json";

    [Header("Components")]
    public GameObject inventoryPanel;
    public GameObject myItemsPanel;
    public GameObject informationPanel;
    public GameObject shopPanel;
    public GameObject infoShopPanel;
    [SerializeField] private GameObject buttonPrefab;

    [Header("Inventory components")]
    [SerializeField] private GameObject toolList;
    [SerializeField] private GameObject materialList;
    [SerializeField] private Transform toolContent;
    [SerializeField] private Transform materialContent;
    [SerializeField] private GameObject toolButton;
    [SerializeField] private GameObject materialButton;
    private List<GameObject> items = new List<GameObject>();

    [Header("My item components")]
    [SerializeField] private GameObject goToGameButton;
    [SerializeField] private GameObject maybeLaterButton;

    [Header("Information components")]
    [SerializeField] private TextMeshProUGUI discription;

    [Header("Shop components")]
    [SerializeField] private Transform shopContent;
    [SerializeField] private GameObject shopButton;
    [SerializeField] private GameObject materialPrefab;
    [SerializeField] private TextMeshProUGUI discriptionGoods;
    private List<GameObject> goods = new List<GameObject>();

    //path to file with items
    private string pathToolItems;
    private string pathMaterialItems;
    private string pathShopItems;
    private string pathSelectItems;
    private string pathWeaponItem;

    private bool isCantOpen = false;
    public bool inventarIsOpen { get; private set; }

    public LoadStatusEnum Status { get; private set; }

    private T SetValueFromJson<T>(T value, string path)
    {
        if (File.Exists(path))
        {
#if !UNITY_EDITOR
            var reader = new WWW(path);
            while(!reader.isDone) { }
            var json = reader.text;
#else
            var json = File.ReadAllText(path);
#endif
            if (json == "null" || json == "{}" || json == "[]" || String.IsNullOrEmpty(json))
                return value;

            value = JsonConvert.DeserializeObject<T>(json);
        }

        return value;
    }

    private void SetItems()
    {
        itemsTool = SetValueFromJson<Dictionary<int, ItemsModel>>(itemsTool, pathToolItems);
        if (itemsTool == null)
        {
            itemsTool = new Dictionary<int, ItemsModel>();
            itemsTool.Add((int)ItemIds.Axe, new ItemsModel((int)ItemIds.Axe,
                                                            TextController.items.ItemName[(int)ItemIds.Axe],
                                                            ItemsTypeEnum.Weapon));
        }

        itemsMaterial = SetValueFromJson<Dictionary<int, ItemsModel>>(itemsMaterial, pathMaterialItems);
        if (itemsMaterial == null)
        {
            itemsMaterial = new Dictionary<int, ItemsModel>();
        }

        itemsShop = SetValueFromJson<Dictionary<int, GoodsModel>>(itemsShop, pathShopItems);
        if (itemsShop == null)
        {
            itemsShop = new Dictionary<int, GoodsModel>();

            AddNewGoods(ItemIds.TechGloves, ItemsTypeEnum.Tool);
            AddNewGoods(ItemIds.SensoryBoots, ItemsTypeEnum.Tool);
            AddNewGoods(ItemIds.Blade, ItemsTypeEnum.Tool);
            AddNewGoods(ItemIds.NATOBulletproofVest, ItemsTypeEnum.Tool);
        }

        itemSelect = SetValueFromJson<List<ItemsModel>>(itemSelect, pathSelectItems);
        if (itemSelect == null)
        {
            itemSelect = new List<ItemsModel>();
        }

        weaponSelect = SetValueFromJson<ItemsModel>(weaponSelect, pathWeaponItem);
        if (weaponSelect == null)
        {
            weaponSelect = null;
        }
    }

    public void StartUp()
    {
        instance = this;
        inventoryPanel.SetActive(false);
        myItemsPanel.SetActive(false);
        informationPanel.SetActive(false);
        shopPanel.SetActive(false);
        infoShopPanel.SetActive(false);

#if !UNITY_EDITOR
        pathToolItems = Path.Combine(Application.persistentDataPath, SAVE_TOOL_ITEMS_KEY);
        pathMaterialItems = Path.Combine(Application.persistentDataPath, SAVE_MATERIAL_ITEMS_KEY);
        pathShopItems = Path.Combine(Application.persistentDataPath, SAVE_SHOP_ITEMS_KEY);
        pathSelectItems = Path.Combine(Application.persistentDataPath, SAVE_SELECT_ITEMS_KEY);
        pathWeaponItem = Path.Combine(Application.persistentDataPath, SAVE_SELECT_WEAPON_KEY);

        SetItems();
#else
        pathToolItems = Path.Combine(Application.dataPath, SAVE_TOOL_ITEMS_KEY);
        pathMaterialItems = Path.Combine(Application.dataPath, SAVE_MATERIAL_ITEMS_KEY);
        pathShopItems = Path.Combine(Application.dataPath, SAVE_SHOP_ITEMS_KEY);
        pathSelectItems = Path.Combine(Application.dataPath, SAVE_SELECT_ITEMS_KEY);
        pathWeaponItem = Path.Combine(Application.dataPath, SAVE_SELECT_WEAPON_KEY);
#endif
        SetItems();

        UpdateSelectItem();

        Status = LoadStatusEnum.IsLoaded;
    }

    public static InventarController GetInstance()
    {
        return instance;
    }

    private void ClearItems(ref List<GameObject> list)
    {
        if(list.Count != 0)
        {
            foreach (GameObject item in list)
            {
                Destroy(item);
            }
            list.Clear();
        }
    }

    public void OpenInformationPanel(int index)
    {
        discription.text = TextController.items.ItemDis[index];

        Button choose = informationPanel.transform.GetChild(2).GetComponent<Button>();
        if (itemsTool.ContainsKey(index))
        {
            choose.gameObject.SetActive(true);
            switch (itemsTool[index].Type)
            {
                case ItemsTypeEnum.Weapon:
                    if (weaponSelect == null)
                    {
                        choose.interactable = true;
                    }
                    break;
                case ItemsTypeEnum.Tool:
                    if (itemSelect.Count != 3)
                    {
                        choose.interactable = true;
                    }
                    else
                    {
                        for (int i = 0; i < itemSelect.Count; i++)
                        {
                            if (itemSelect[i] == null)
                            {
                                choose.interactable = true;
                                break;
                            }
                        }
                    }
                    break;
            }
        }
        else if (itemsMaterial.ContainsKey(index))
        {
            choose.gameObject.SetActive(false);
        }

        informationPanel.transform.GetChild(0).GetComponent<Image>().sprite =
            Resources.Load<Sprite>($"ItemInfoIcon/{index}");
        //Set to "Choose" button index of choose item
        informationPanel.transform.GetChild(2).GetComponent<ItemButton>().ItemIndex = index;

        informationPanel.SetActive(true);
    }

    public void OpenGoodsInfoPanel(int index)
    {
        discriptionGoods.text = TextController.items.ItemDis[index];

        infoShopPanel.transform.GetChild(0).GetComponent<Image>().sprite =
            Resources.Load<Sprite>($"ItemInfoIcon/{index}");
        //Set to "Choose" button index of choose item
        infoShopPanel.transform.GetChild(2).GetComponent<ItemButton>().ItemIndex = index;

        infoShopPanel.SetActive(true);
    }

    public void CloseInformationPanel()
    {
        UpdateSelectItem();
        informationPanel.SetActive(false);
    }

    public void OpenToolList()
    {
        //Clear list with all item in content object
        ClearItems(ref items);

        //Get item's ids that was select by player
        List<int> itemIds = new List<int>();
        foreach (var item in itemSelect)
        {
            itemIds.Add(item.Index);
        }
        if(weaponSelect != null)
        {
            itemIds.Add(weaponSelect.Index);
        }

        //Create buttons for content list
        foreach (var item in itemsTool)
        {
            if (itemIds.Contains(item.Value.Index)) continue;

            var button = Instantiate(buttonPrefab, toolContent);
            button.GetComponent<ItemButton>().ItemIndex = item.Value.Index;
            button.transform.GetChild(0).GetComponent<Image>().sprite =
                Resources.Load<Sprite>($"ItemMiniIcon/{item.Value.Index}");
            button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = TextController.items.ItemName[item.Value.Index];
            button.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.Value.Count.ToString();
            items.Add(button);
        }

        materialList.SetActive(false);
        toolList.SetActive(true);
    }

    public void OpenMaterialList()
    {
        //Clear list with all item in content object
        ClearItems(ref items);

        //Create buttons for content list
        foreach (var item in itemsMaterial)
        {
            var button = Instantiate(buttonPrefab, materialContent);
            button.GetComponent<ItemButton>().ItemIndex = item.Value.Index;
            button.transform.GetChild(0).GetComponent<Image>().sprite =
                Resources.Load<Sprite>($"ItemMiniIcon/{item.Value.Index}");
            button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = TextController.items.ItemName[item.Value.Index];
            button.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.Value.Count.ToString();
            items.Add(button);
        }

        toolList.SetActive(false);
        materialList.SetActive(true);
    }

    public void AddNewGoods(ItemIds ids, ItemsTypeEnum type)
    {
        List<ItemsModel> materials = new List<ItemsModel>();
        foreach (KeyValuePair<ItemIds, int> item in SaveController.price.itemPrice[ids])
        {
            materials.Add(new ItemsModel((int)item.Key, item.Key.ToString(), ItemsTypeEnum.Material, item.Value));
        }

        itemsShop.Add((int)ids, new GoodsModel((int)ids, TextController.items.ItemName[(int)ids], type, materials));
    }

    public void AddNewItemTool(ItemsModel item)
    {
        if (!itemsTool.ContainsKey(item.Index))
        {
            itemsTool.Add(item.Index, item);
        }
    }

    public void AddNewItemMaterial(ItemsModel item)
    {
        if (itemsMaterial.ContainsKey(item.Index))
        {
            itemsMaterial[item.Index] = new ItemsModel(item.Index,
                                                       item.Name,
                                                       item.Type,
                                                       itemsMaterial[item.Index].Count + item.Count);
        }
        else
        {
            itemsMaterial.Add(item.Index, item);
        }
    }

    public void UpdateSelectItem()
    {
        //Update selected items
        var itemIconImages = myItemsPanel.transform.GetChild(0).GetChild(1).GetComponentsInChildren<Image>();
        //If not selected items
        if (itemSelect.Count == 0)
        {
            for (int i = 0; i < itemIconImages.Length; i++)
            {
                itemIconImages[i].sprite = null;
                itemIconImages[i].color = new Color(0.44f, 0.23f, 0);
            }
        }
        for (int i = 0; i < itemSelect.Count; i++)
        {
            if (itemSelect[i] == null)
            {
                itemIconImages[i].sprite = null;
                itemIconImages[i].color = new Color(0.44f, 0.23f, 0);
                continue;
            }

            itemIconImages[i].color = new Color(1, 1, 1);
            itemIconImages[i].sprite = Resources.Load<Sprite>($"ItemFullIcon/{itemSelect[i].Index}");
        }

        var weaponIconImage = myItemsPanel.transform.GetChild(0).GetChild(3).GetComponentInChildren<Image>();
        if (weaponSelect != null)
        {
            weaponIconImage.color = new Color(1, 1, 1);
            weaponIconImage.sprite = Resources.Load<Sprite>($"ItemFullIcon/{weaponSelect.Index}");
        }
        else
        {
            weaponIconImage.color = new Color(0.44f, 0.23f, 0);
            weaponIconImage.sprite = null;
        }

        //Update content object
        OpenToolList();

        //Removing selected item from content object
        var itemButtons = toolContent.GetComponentsInChildren<ItemButton>();
        foreach (ItemButton itemButton in itemButtons)
        {
            if(weaponSelect != null)
            {
                if(itemButton.ItemIndex == weaponSelect.Index)
                {
                    Destroy(itemButton.gameObject);
                    continue;
                }
            }
            for (int i = 0; i < itemSelect.Count; i++)
            {
                if (itemButton.ItemIndex == itemSelect[i].Index)
                {
                    Destroy(itemButton.gameObject);
                    break;
                }
            }
        }
    }

    public void OpenInventoryToGame()
    {
        toolButton.SetActive(false);
        materialButton.SetActive(false);

        materialList.SetActive(false);
        toolList.SetActive(true);

        goToGameButton.SetActive(true);
        maybeLaterButton.SetActive(true);

        UpdateSelectItem();

        inventoryPanel.SetActive(true);
        myItemsPanel.SetActive(true);

        isCantOpen = true;
        PlayerController.GetInstance().IsCanMove = false;
    }

    public void OpenShop()
    {
        isCantOpen = true;
        OpenMaterialList();
        toolButton.SetActive(false);
        materialButton.SetActive(false);

        //Clear list with all item in content object
        ClearItems(ref goods);

        foreach (KeyValuePair <int, GoodsModel> goodsModel in itemsShop)
        {
            var buttonObj = Instantiate(shopButton, shopContent);
            buttonObj.GetComponent<ItemButton>().ItemIndex = goodsModel.Value.IndexItem;
            buttonObj.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite =
                Resources.Load<Sprite>($"ItemMiniIcon/{goodsModel.Value.IndexItem}");
            buttonObj.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = TextController.items.ItemName[goodsModel.Value.IndexItem];
            var price = buttonObj.transform.GetChild(1);

            foreach (ItemsModel item in goodsModel.Value.Materials)
            {
                var itemObj = Instantiate(materialPrefab, price);
                itemObj.transform.GetComponentInChildren<Image>().sprite =
                    Resources.Load<Sprite>($"ItemMiniIcon/{item.Index}");
                itemObj.transform.GetComponentInChildren<TextMeshProUGUI>().text = item.Count.ToString();
            }

            goods.Add(buttonObj);
        }

        inventoryPanel.SetActive(true);
        shopPanel.SetActive(true);
    }

    public void CloseInventoryToGame()
    {
        inventoryPanel.SetActive(false);
        myItemsPanel.SetActive(false);

        toolButton.SetActive(true);
        materialButton.SetActive(true);

        materialList.SetActive(false);
        toolList.SetActive(false);

        goToGameButton.SetActive(false);
        maybeLaterButton.SetActive(false);

        isCantOpen = false;
        PlayerController.GetInstance().IsCanMove = true;
    }

    public void CloseGoodsInfoPanel()
    {
        infoShopPanel.SetActive(false);
    }

    public void CloseShop()
    {
        inventoryPanel.SetActive(false);
        infoShopPanel.SetActive(false);
        shopPanel.SetActive(false);

        isCantOpen = false;
        toolButton.SetActive(true);
        materialButton.SetActive(true);
        PlayerController.GetInstance().IsCanMove = true;
    }

    public void SaveInventoryToFile()
    {
        var jsonTool = JsonConvert.SerializeObject(itemsTool);
        var jsonMaterial = JsonConvert.SerializeObject(itemsMaterial);
        var jsonSelectItem = JsonConvert.SerializeObject(itemSelect);
        var jsonSelectWeapon = JsonConvert.SerializeObject(weaponSelect);
        var jsonShop = JsonConvert.SerializeObject(itemsShop);

        File.WriteAllText(pathToolItems, jsonTool);
        File.WriteAllText(pathMaterialItems, jsonMaterial);
        File.WriteAllText(pathSelectItems, jsonSelectItem);
        File.WriteAllText(pathWeaponItem, jsonSelectWeapon);
        File.WriteAllText(pathShopItems, jsonShop);
    }

    public void ResetData()
    {
        File.WriteAllText(pathToolItems, "");
        File.WriteAllText(pathMaterialItems, "");
        File.WriteAllText(pathSelectItems, "");
        File.WriteAllText(pathWeaponItem, "");
        File.WriteAllText(pathShopItems, "");

        DialogueController.GetInstance().SetVariableState(GlobalVariablesConstants.IS_FIRST_MEET_WITH_ERNEST, new Ink.Runtime.BoolValue(true));
        DialogueController.GetInstance().SetVariableState(GlobalVariablesConstants.ERNEST_DIALOG_INDEX, new Ink.Runtime.IntValue(0));
        DialogueController.GetInstance().SetVariableState(GlobalVariablesConstants.ADELINA_DIALOG_INDEX, new Ink.Runtime.IntValue(0));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && 
            !DialogueController.GetInstance().dialogueIsPlaying &&
            !isCantOpen &&
            !TextController.GetInstance().pauseMenu.activeSelf)
        {
            if(SceneManager.GetActiveScene().name == "Lobbi")
                TextController.GetInstance().openInventar.SetActive(false);

            if (SceneManager.GetActiveScene().name == "Lobbi")
            {
                toolButton.SetActive(true);
                materialButton.SetActive(true);
                OpenToolList();
                UpdateSelectItem();
            }
            else
            {
                toolButton.SetActive(false);
                materialButton.SetActive(false); 
                OpenMaterialList();
            }

            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            myItemsPanel.SetActive(!myItemsPanel.activeSelf);
            PlayerController.GetInstance().IsCanMove = !inventoryPanel.activeSelf;
        }
    }

    private void FixedUpdate()
    {
        if(inventoryPanel.activeSelf ||
           myItemsPanel.activeSelf ||
           shopPanel.activeSelf ||
           informationPanel.activeSelf)
        {
            inventarIsOpen = true;
            PlayerController.GetInstance().IsCanMove = false;
        }
        else
        {
            inventarIsOpen = false;
        }
    }

    private void OnApplicationQuit()
    {
        SaveInventoryToFile();
    }
}
