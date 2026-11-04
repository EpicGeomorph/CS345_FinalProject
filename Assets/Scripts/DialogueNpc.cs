using UnityEngine;
using UnityEngine.InputSystem;

// Runs before DialogueBox so the E press that closes a dialogue can't reopen it the same frame.
[DefaultExecutionOrder(-1)]
public class DialogueNpc : MonoBehaviour
{
    [SerializeField] string npcName;
    [SerializeField] Transform player;
    [SerializeField] DialogueBox dialogueBox;
    [SerializeField] float talkRange = 2f;
    [SerializeField] DialogueStep[] steps;

    InputAction interact;
    bool inRange;

    void Awake()
    {
        interact = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        if (DialogueBox.IsOpen) return;

        bool nowInRange = Vector3.Distance(player.position, transform.position) <= talkRange;
        if (nowInRange != inRange)
        {
            inRange = nowInRange;
            dialogueBox.ShowPrompt(inRange ? $"[E] Talk to {npcName}" : "");
        }

        if (inRange && interact.WasPressedThisFrame())
        {
            dialogueBox.Open(npcName, steps);
        }
    }
}
