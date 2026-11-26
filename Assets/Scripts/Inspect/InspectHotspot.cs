using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// A clickable spot inside an inspect view. Needs a Button on the same GameObject.
/// When clicked it calls a public method by name: on the scene's Levelhandler by default,
/// or on any GameObject you name in Target Object (e.g. "Supervisor").
/// </summary>
[RequireComponent(typeof(Button))]
public class InspectHotspot : MonoBehaviour
{
    [Tooltip("Name of a GameObject in the scene to call the method on, e.g. Supervisor. " +
             "Every script on that object gets the call. Leave empty to use the Levelhandler.")]
    public string targetObject;

    [Tooltip("Name of a public method to call, e.g. FoundPerson. Leave empty to call nothing.")]
    public string methodName;

    [Tooltip("Optional. If set, it is passed to the method, so the method must take one string parameter.")]
    public string argument;

    [Tooltip("Optional. If set, this picture pops up in a small window on top of the Inspect UI.")]
    public Sprite popupSprite;

    [Tooltip("Replaces the popup's description text when clicked. Leave empty to keep the current text.")]
    [TextArea(2, 4)]
    public string description;

    [Tooltip("Can only be clicked once; stays greyed out when the item is opened again.")]
    public bool onlyOnce = true;

    [Tooltip("Close the popup after this is clicked.")]
    public bool closeAfterClick = false;

    [Tooltip("Extra things to do on click, for things inside the view itself (e.g. show another image).")]
    public UnityEvent onClicked;

    private Button button;
    private InspectUI owner;
    private string key;

    /// <summary>Called by InspectUI when the view is opened.</summary>
    public void Bind(InspectUI ui, string hotspotKey, bool alreadyUsed)
    {
        owner = ui;
        key = hotspotKey;

        button = GetComponent<Button>();
        button.onClick.RemoveListener(OnClick);
        button.onClick.AddListener(OnClick);

        if (alreadyUsed && onlyOnce)
        {
            SetUsed();
        }
    }

    public void SetUsed()
    {
        if (button == null) button = GetComponent<Button>();
        button.interactable = false;
    }

    private void OnClick()
    {
        if (owner != null)
        {
            owner.HotspotClicked(this, key);
        }
    }
}
