using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    private CameraController camController;

    private TextMeshProUGUI textObj;

    private void Start()
    {
        camController = GameObject.Find(ControllerConstants.CAMERA_CONTROLLER).GetComponent<CameraController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Player") return;

        var player = collision.gameObject;
        textObj = player.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        textObj.text = TextController.uiText.OpenDoor;

        if (Input.GetKeyDown(KeyCode.E) && !InventarController.GetInstance().inventarIsOpen)
        {
            if (player.transform.position.y > transform.position.y)
            {
                player.transform.position = new Vector3(transform.position.x,
                                                        transform.position.y - 2f);
            }
            else
            {
                player.transform.position = new Vector3(transform.position.x,
                                                        transform.position.y + 2f);
            }

            camController.ChangeCameraStatus();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Player") return;

        textObj.text = "";
    }
}
