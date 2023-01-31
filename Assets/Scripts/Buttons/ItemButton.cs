using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public int _index;

    public int ItemIndex 
    { 
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
        InventarController.GetInstance().informationPanel.SetActive(false);
    }
}
