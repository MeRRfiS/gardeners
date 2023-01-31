using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueVariables
{
    public Dictionary<string, Ink.Runtime.Object> variables;

    private Story globalVariablesStory;

    private const string SAVE_VARIABLES_KEY = "INK_VARIABLES";

    public DialogueVariables(TextAsset loadGlobalJSON)
    {
        globalVariablesStory = new Story(loadGlobalJSON.text);
        if (PlayerPrefs.HasKey(SAVE_VARIABLES_KEY))
        {
            string jsonState = PlayerPrefs.GetString(SAVE_VARIABLES_KEY);
            globalVariablesStory.state.LoadJson(jsonState);
        }

        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalVariablesStory.variablesState)
        {
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
        }
    }

    public void SaveVariables()
    {
        if(globalVariablesStory != null)
        {
            VariablesToStory(globalVariablesStory);
            PlayerPrefs.SetString(SAVE_VARIABLES_KEY, globalVariablesStory.state.ToJson());
        }
    }

    public void StartListening(Story story)
    {
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
    }

    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    public void VariableChanged(string name, Ink.Runtime.Object value)
    {
        if (variables.ContainsKey(name))
        {
            variables.Remove(name);
            variables.Add(name, value);
        }
    }

    private void VariablesToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}
