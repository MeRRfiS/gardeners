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

    private List<IController> controllers = new List<IController>();

    private void Awake()
    {
        controllers.Add(saveController);
        controllers.Add(textController);
        controllers.Add(playerController);
        controllers.Add(cameraController);

        StartCoroutine(LoadControllers());
    }

    public IEnumerator LoadControllers()
    {
        foreach (IController controller in controllers)
        {
            controller.StartUp();
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

        Debug.Log("All handlers is ready");
    }
}
