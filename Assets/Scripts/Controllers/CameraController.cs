using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject player;

    [Header("Settings")]
    [SerializeField] private float offsetSmoothing = 1f;
    private CameraStatusEnum status = CameraStatusEnum.InHouse;

    public void ChangeCameraStatus()
    {
        if(status == CameraStatusEnum.InHouse)
        {
            status = CameraStatusEnum.Outdoor;
        }
        else
        {
            status = CameraStatusEnum.InHouse;
        }
    }

    void Update()
    {
        var newPosition = new Vector3();

        switch (status)
        {
            case CameraStatusEnum.InHouse:
                newPosition = new Vector3(0, 0, -10);
                break;
            case CameraStatusEnum.Outdoor:
                newPosition = new Vector3(player.transform.position.x,
                                          player.transform.position.y,
                                          -10);
                break;
        }

        camera.transform.position = Vector3.Lerp(camera.transform.position,
                                                         newPosition,
                                                         offsetSmoothing * Time.deltaTime);
    }
}
