using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceIndex : MonoBehaviour
{
    private int _index;

    public int Index { 
        get { return _index; }
        set { _index = value; }
    }

    public void SetIndexToChoice()
    {
        DialogueController.GetInstance().choiceIndex = _index;
        DialogueController.GetInstance().MakeChoice();
    }
}
