using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject loadingWindow;
    [SerializeField] private Slider slider;

    public void LoadGame()
    {
        StartCoroutine(LoadAsync("Lobbi"));
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    private IEnumerator LoadAsync(string sceneName)
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
