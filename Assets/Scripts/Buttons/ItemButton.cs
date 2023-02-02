using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    private int _index;

    public int ItemIndex 
    { 
        get { return _index; }
        set 
        { 
            _index = value; 
        } 
    }

    public void OpenInformation()
    {
        InventarController.GetInstance().OpenInformationPanel(_index);
    }

    public void CloseInformation()
    {
        InventarController.GetInstance().CloseInformationPanel();
    }

    public void SelectItem()
    {
        var item = InventarController.GetInstance().itemsTool[_index];
        switch (item.Type)
        {
            case ItemsTypeEnum.Weapon:
                InventarController.GetInstance().weaponSelect = item;
                break;
            case ItemsTypeEnum.Tool:
                if (InventarController.GetInstance().itemSelect.Count != 3)
                {
                    InventarController.GetInstance().itemSelect.Add(item);
                }
                else
                {
                    for (int i = 0; i < InventarController.GetInstance().itemSelect.Count; i++)
                    {
                        if (InventarController.GetInstance().itemSelect[i] == null)
                        {
                            InventarController.GetInstance().itemSelect[i] = item;
                        }
                    }
                }
                break;
        }

        InventarController.GetInstance().CloseInformationPanel();
    }

    public void RemoveSelectItem()
    {
        if (SceneManager.GetActiveScene().name != "Lobbi") return;

        var buttonNumber = name[name.Length - 1];

        try
        {
            switch (buttonNumber)
            {
                case 'W':
                    InventarController.GetInstance().weaponSelect = null;
                    break;
                case '0':
                case '1':
                case '2':
                    int index = int.Parse(buttonNumber.ToString());
                    InventarController.GetInstance().itemSelect.RemoveAt(index);
                    break;
            }
        }
        catch (System.Exception)
        {
            Debug.LogWarning("Button is empty");
        }

        InventarController.GetInstance().UpdateSelectItem();
    }

    public void OpenInfoGoods()
    {
        InventarController.GetInstance().OpenGoodsInfoPanel(_index);
    }

    public void CloseInfoGoods()
    {
        InventarController.GetInstance().CloseGoodsInfoPanel();
    }

    public void BuyItem()
    {
        var goods = InventarController.GetInstance().itemsShop[_index];

        foreach (var material in goods.Materials)
        {
            if (InventarController.GetInstance().itemsMaterial.ContainsKey(material.Index))
            {
                if(InventarController.GetInstance().itemsMaterial[material.Index].Count < material.Count)
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        foreach (var material in goods.Materials)
        {
            InventarController.GetInstance().itemsMaterial[material.Index].Count -= material.Count;
            if(InventarController.GetInstance().itemsMaterial[material.Index].Count == 0)
            {
                InventarController.GetInstance().itemsMaterial.Remove(material.Index);
            }
        }

        var item = new ItemsModel(_index,
                                  TextController.items.ItemName[_index],
                                  ItemsTypeEnum.Tool);
        InventarController.GetInstance().AddNewItemTool(item);

        InventarController.GetInstance().itemsShop.Remove(_index);
        InventarController.GetInstance().CloseGoodsInfoPanel();
        InventarController.GetInstance().OpenShop();
    }
}
