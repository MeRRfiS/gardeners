using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraController : MonoBehaviour, IController
{
    [Header("Components")]
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject player;

    [Header("Settings")]
    [SerializeField] private float offsetSmoothing = 1f;
    [SerializeField] private Vector2 maxPosition;
    [SerializeField] private CameraStatusEnum status = CameraStatusEnum.InHouse;

    public LoadStatusEnum Status { get; private set; }

    public void StartUp()
    {
        Status = LoadStatusEnum.IsLoaded;
    }

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

    void FixedUpdate()
    {
        var newPosition = new Vector3();

        switch (status)
        {
            case CameraStatusEnum.InHouse:
                newPosition = new Vector3(0, 6, -10);
                break;
            case CameraStatusEnum.Outdoor:
                newPosition = new Vector3(Mathf.Clamp(player.transform.position.x, -maxPosition.x, maxPosition.x),
                                          Mathf.Clamp(player.transform.position.y, -maxPosition.y, maxPosition.y),
                                          -10);
                break;
        }

        camera.transform.position = Vector3.Lerp(camera.transform.position,
                                                         newPosition,
                                                         offsetSmoothing * Time.deltaTime);
    }
}
