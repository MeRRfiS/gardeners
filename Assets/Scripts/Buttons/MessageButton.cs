using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageButton : MonoBehaviour
{
    public void CloseWarning()
    {
        TextController.GetInstance().warningPanel.SetActive(false);
    }
}
