using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageButton : MonoBehaviour
{
    public void CloseWarning()
    {
        TextController.GetInstance().warningPanel.SetActive(false);
    }

    public void CloseDialog()
    {
        TextController.GetInstance().newDialogPanel.SetActive(false);
    }

    public void ReturnToGame()
    {
        Time.timeScale = 1;
        TextController.GetInstance().pauseMenu.SetActive(false);
        SoundController.GetInctanse().UnMuteSound();
    }

    public void ExitToMenu()
    {
        try
        {
            Time.timeScale = 1;
            InventarController.GetInstance().SaveInventoryToFile();
            SoundController.GetInctanse().UnMuteSound();
            StartCoroutine(TextController.GetInstance().LoadAsync("Menu"));
        }
        catch (System.Exception ex)
        {
            Debug.LogError("ExitToMenu");
            Debug.LogError(ex);
            TextController.GetInstance().WriteErrorMessage(ex.Message);
            throw ex;
        }
    }
}
