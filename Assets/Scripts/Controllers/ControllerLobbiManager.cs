using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerLobbiManager : MonoBehaviour, IControllerManager
{
    [Header("Controllers")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private TextController textController;
    [SerializeField] private SaveController saveController;
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private InventarController inventarController;
    [SerializeField] private SoundController soundController;

    private List<IController> controllers = new List<IController>();

    private void Awake()
    {
        controllers.Add(saveController);
        controllers.Add(dialogueController);
        controllers.Add(textController);
        controllers.Add(inventarController);
        controllers.Add(playerController);
        controllers.Add(cameraController);
        controllers.Add(soundController);

        StartCoroutine(LoadControllers());
    }

    public IEnumerator LoadControllers()
    {
        foreach (IController controller in controllers)
        {
            while (controller.Status != LoadStatusEnum.IsLoaded)
            {
                controller.StartUp();
                Debug.Log("Loading...");
            }
        }

        yield return null;

        var allControllers = controllers.Count;
        var readyControllers = 0;

        while (readyControllers < allControllers)
        {
            var lastReady = readyControllers;
            readyControllers = 0;

            foreach (IController controller in controllers)
            {
                if (controller.Status == LoadStatusEnum.IsLoaded)
                {
                    readyControllers++;
                }
            }

            if (readyControllers > lastReady)
            {
                Debug.Log($"Progress: {readyControllers}/{allControllers}");
            }
            else
            {
                for (int i = 0; i < controllers.Count; i++)
                {
                    if (controllers[i].Status == LoadStatusEnum.NotReady)
                    {
                        Debug.Log($"Index of not ready handler: {i}");
                    }
                }
            }

            yield return null;
        }

        PlayerController.GetInstance().SetValueToPlayer();
        Debug.Log("All contollers is ready");
    }
}
