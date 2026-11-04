using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    public static bool IsOpen { get; private set; }

    [SerializeField] GameObject panel;
    [SerializeField] Text speakerText;
    [SerializeField] Text bodyText;
    [SerializeField] Text promptText;

    InputAction interact;
    DialogueStep[] steps;
    int stepIndex;
    bool showingReply;
    int openedFrame;

    void Awake()
    {
        interact = InputSystem.actions.FindAction("Interact");
        panel.SetActive(false);
        IsOpen = false;
    }

    public void ShowPrompt(string prompt)
    {
        promptText.text = prompt;
    }

    public void Open(string speaker, DialogueStep[] dialogueSteps)
    {
        steps = dialogueSteps;
        stepIndex = 0;
        openedFrame = Time.frameCount;
        speakerText.text = speaker;
        promptText.enabled = false;
        panel.SetActive(true);
        IsOpen = true;
        ShowStep();
    }

    void Update()
    {
        if (!IsOpen || Time.frameCount == openedFrame) return;

        if (showingReply)
        {
            if (interact.WasPressedThisFrame()) Continue();
            return;
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame) Choose(0);
        else if (Keyboard.current.digit2Key.wasPressedThisFrame) Choose(1);
        else if (Keyboard.current.digit3Key.wasPressedThisFrame) Choose(2);
    }

    void ShowStep()
    {
        showingReply = false;
        DialogueStep step = steps[stepIndex];
        bodyText.text = step.line + "\n";
        for (int i = 0; i < step.choices.Length; i++)
        {
            bodyText.text += $"\n{i + 1}. {step.choices[i].text}";
        }
    }

    void Choose(int index)
    {
        DialogueChoice choice = steps[stepIndex].choices[index];
        if (choice.advances) stepIndex++;
        showingReply = true;
        bodyText.text = choice.reply + "\n\n[E] Continue";
    }

    void Continue()
    {
        if (stepIndex < steps.Length)
        {
            ShowStep();
            return;
        }

        panel.SetActive(false);
        promptText.enabled = true;
        IsOpen = false;
    }
}
