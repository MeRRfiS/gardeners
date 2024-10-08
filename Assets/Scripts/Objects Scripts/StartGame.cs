using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name != "Player") return;

       if(((Ink.Runtime.IntValue)DialogueController
            .GetInstance()
            .GetVariableState(GlobalVariablesConstants.ERNEST_DIALOG_INDEX)).value == 0)
        {
            DialogueController
                .GetInstance()
                .SetVariableState(GlobalVariablesConstants.ERNEST_DIALOG_INDEX, new Ink.Runtime.IntValue(0));
            DialogueController
                .GetInstance()
                .SetVariableState(GlobalVariablesConstants.CHARACTER_INDEX, new Ink.Runtime.IntValue(1));
            DialogueController.GetInstance().EnterDialogueMode(CharactersEnum.Ernest);
        }
        else
        {
            InventarController.GetInstance().OpenInventoryToGame();
        }
    }

    [SerializeField] private GameObject loadingWindow;
    [SerializeField] private Slider slider;

    public void ToLoadGame()
    {
        try
        {
            if (InventarController.GetInstance().weaponSelect == null)
            {
                TextController.GetInstance().warningPanel.SetActive(true);
                return;
            }
            InventarController.GetInstance().SaveInventoryToFile();

            StartCoroutine(TextController.GetInstance().LoadAsync("Level1"));
        }
        catch (System.Exception ex)
        {
            Debug.LogError("ToLoadGame");
            Debug.LogError(ex);
            TextController.GetInstance().WriteErrorMessage(ex.Message);
            throw ex;
        }
    }

    //IEnumerator LoadAsync(string sceneName)
    //{
    //    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

    //    loadingWindow.SetActive(true);

    //    while (!asyncLoad.isDone)
    //    {
    //        slider.value = asyncLoad.progress;

    //        yield return null;
    //    }
    //}
}
