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

    [Header("Components")]
    public GameObject inventoryPanel;
    public GameObject informationPanel;

    [Header("Inventory components")]
    [SerializeField] private GameObject toolList;
    [SerializeField] private GameObject materialList;

    [Header("Information components")]
    [SerializeField] private TextMeshProUGUI discription;

    public LoadStatusEnum Status { get; private set; }

    public void StartUp()
    {
        instance = this;
        inventoryPanel.SetActive(false);
        informationPanel.SetActive(false);

        Status = LoadStatusEnum.IsLoaded;
    }

    public static InventarController GetInstance()
    {
        return instance;
    }

    public void OpenInformationPanel(int index)
    {
        discription.text = TextController.items.ItemDis[index];
        informationPanel.SetActive(true);
    }

    public void OpenToolList()
    {
        materialList.SetActive(false);
        toolList.SetActive(true);
    }

    public void OpenMaterialList()
    {
        toolList.SetActive(false);
        materialList.SetActive(true);
    }

    public void AddNewItemTool(ItemsStruct item)
    {
        itemsTool.Add(itemsTool.Count, item);
    }

    public void AddNewItemMaterial(ItemsStruct item)
    {
        itemsMaterial.Add(itemsMaterial.Count, item);
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
}
