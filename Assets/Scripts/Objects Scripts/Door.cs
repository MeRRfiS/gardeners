using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private CameraController camController;

    private void Start()
    {
        camController = GameObject.Find(ControllerConstants.CAMERA_CONTROLLER).GetComponent<CameraController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Player") return;

        var player = collision.gameObject;

        if(player.transform.position.y < transform.position.y)
        {
            player.transform.position = new Vector3(transform.position.x,
                                                    transform.position.y + 2f);
        }
        else
        {
            player.transform.position = new Vector3(transform.position.x,
                                                    transform.position.y - 2f);
        }

        camController.ChangeCameraStatus();
    }
}
