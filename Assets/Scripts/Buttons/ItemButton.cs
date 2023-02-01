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
}
