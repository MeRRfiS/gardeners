using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    private TextMeshProUGUI textObj;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Player") return;

        var player = collision.gameObject;
        textObj = player.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        textObj.text = TextController.uiText.OpenDialog;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Player") return;

        textObj.text = "";
    }
}
