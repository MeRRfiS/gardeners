using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    private TextMeshProUGUI textObj;

    private TextController textController;
    private PlayerController playerController;

    [SerializeField] private CharactersEnum character;

    private void Start()
    {
        textController = GameObject.Find(ControllerConstants.TEXT_CONTROLLER).GetComponent<TextController>();
        playerController = GameObject.Find(ControllerConstants.PLAYER_CONTROLLER).GetComponent<PlayerController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Player") return;

        var player = collision.gameObject;
        textObj = player.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        textObj.text = TextController.uiText.OpenDialog;

        if (Input.GetKey(KeyCode.E) && !DialogueController.GetInstance().dialogueIsPlaying)
        {
            switch (character)
            {
                case CharactersEnum.Ernest:
                    if (((Ink.Runtime.IntValue)DialogueController
                        .GetInstance()
                        .GetVariableState(GlobalVariablesConstants.ERNEST_DIALOG_INDEX)).value != 2)
                    {
                        DialogueController
                            .GetInstance()
                            .SetVariableState(GlobalVariablesConstants.ERNEST_DIALOG_INDEX, new Ink.Runtime.IntValue(1));
                    }
                    break;
                default:
                    break;
            }
            
            playerController.IsCanMove = false;
            DialogueController.GetInstance().EnterDialogueMode(character);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Player") return;

        textObj.text = "";
    }
}
