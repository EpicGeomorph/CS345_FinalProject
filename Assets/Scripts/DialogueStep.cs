using System;
using UnityEngine;

[Serializable]
public class DialogueChoice
{
    public string text;
    [TextArea] public string reply;
    public bool advances;
}

[Serializable]
public class DialogueStep
{
    [TextArea] public string line;
    public DialogueChoice[] choices = new DialogueChoice[3];
}
