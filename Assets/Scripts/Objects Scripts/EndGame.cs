using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public int ScenarioIndex { get; set; }

    public void EndGameButton()
    {
        print("1");

        switch (ScenarioIndex)
        {
            case (0):
                InventarController.GetInstance().SaveInventoryToFile();
                var tropItems = GameObject.FindGameObjectsWithTag("Item");
                foreach (var item in tropItems)
                {
                    Destroy(item);
                }

                SceneManager.LoadScene("Lobbi");
                break;
            case (1):
                InventarController.GetInstance().ResetData();

                SceneManager.LoadScene("Menu");
                break;
            case (2):
                InventarController.GetInstance().ResetData();

                SceneManager.LoadScene("Menu");
                break;
        }
    }
}
