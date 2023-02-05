using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public int ScenarioIndex { get; set; }
    [SerializeField] private GameObject loadingWindow;
    [SerializeField] private Slider slider;

    public void EndGameButton()
    {
        switch (ScenarioIndex)
        {
            case (0):
                InventarController.GetInstance().SaveInventoryToFile();
                var tropItems = GameObject.FindGameObjectsWithTag("Item");
                foreach (var item in tropItems)
                {
                    Destroy(item);
                }
                Time.timeScale = 1;

                if(((Ink.Runtime.IntValue)DialogueController
                    .GetInstance()
                    .GetVariableState(GlobalVariablesConstants.FIRST_GAME)).value != 2)
                {
                    DialogueController
                    .GetInstance()
                    .SetVariableState(GlobalVariablesConstants.FIRST_GAME, new Ink.Runtime.IntValue(1));
                }
                if (((Ink.Runtime.IntValue)DialogueController
                    .GetInstance()
                    .GetVariableState(GlobalVariablesConstants.WHAT_I_MUST_DO)).value != 2)
                {
                    DialogueController
                    .GetInstance()
                    .SetVariableState(GlobalVariablesConstants.WHAT_I_MUST_DO, new Ink.Runtime.IntValue(1));
                }

                StartCoroutine(LoadAsync("Lobbi"));
                break;
            case (1):
                InventarController.GetInstance().ResetData();
                Time.timeScale = 1;
                StartCoroutine(LoadAsync("Menu"));
                break;
            case (2):
                InventarController.GetInstance().ResetData();
                Time.timeScale = 1;
                StartCoroutine(LoadAsync("Menu"));
                break;
        }
    }

    IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        loadingWindow.SetActive(true);

        while (!asyncLoad.isDone)
        {
            slider.value = asyncLoad.progress;

            yield return null;
        }
    }
}
