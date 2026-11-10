using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.WSA;

public class DialogueHandler : MonoBehaviour
{
    public string character;
    public GameObject startingNode;
    public GameObject dialogueUI;
    public string nodesPath;
    private List<string> optionsSelect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitDialogue()
    {
        Debug.Log("Started Dialogue with " + character);

        //DialogueNode currentNode = startingNode.GetComponent<DialogueNode>();
        //GameObject ui = Instantiate(dialogueUI, new Vector3(0, 0, 0), Quaternion.identity);

        //GameObject border = ui.transform.Find("Border").gameObject;
        //TextMeshProUGUI char_text = border.transform.Find("Character").gameObject.GetComponent<TextMeshProUGUI>();
        //TextMeshProUGUI dialogue_text = border.transform.Find("Dialogue").gameObject.GetComponent<TextMeshProUGUI>();
        //TextMeshProUGUI options_text = border.transform.Find("Options").gameObject.GetComponent<TextMeshProUGUI>();

        //char_text.text = character;
        //dialogue_text.text = currentNode.dialogueText;

        //optionsSelect = new List<string>();
        //string options_display = "";
        //int counter = 1;
        //foreach (string option in currentNode.choices)
        //{
        //    options_display = options_display + "\n[" + counter + "] " + option;
        //    optionsSelect.Add(option);
        //    counter++;
        //}

        //options_text.text = options_display;
    }
}
