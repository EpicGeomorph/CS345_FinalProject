using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DialogueEditor;

// Remembers the clues found in a level. Dialogue nodes call these methods through their node events.
public class CaseFile : MonoBehaviour
{
    [SerializeField] private int attemptsLeft = 3;
    private List<string> clues = new List<string>();

    public void AddClue(string clue)
    {
        if (!clues.Contains(clue))
        {
            clues.Add(clue);
        }
    }

    public void UseAttempt()
    {
        attemptsLeft--;
        ConversationManager.Instance.SetInt("AttemptsLeft", attemptsLeft);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // The Dialogue Editor resets its parameters every conversation, so copy in what the detective knows.
    public void ShareWith(NPCConversation conversation)
    {
        foreach (string clue in clues)
        {
            if (conversation.GetParameter(clue) != null)
            {
                ConversationManager.Instance.SetBool(clue, true);
            }
        }

        if (conversation.GetParameter("AttemptsLeft") != null)
        {
            ConversationManager.Instance.SetInt("AttemptsLeft", attemptsLeft);
        }
    }
}
