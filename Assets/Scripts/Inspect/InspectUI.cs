using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The Inspect popup. Lives on the InspectCanvas prefab (Assets/Fabs/InspectCanvas.prefab).
/// Drop ONE InspectCanvas into each scene that needs inspecting.
///
/// Flow:
///   inventory item (InspectableItem) clicked  ->  InspectUI.Open(item)
///   hotspot (InspectHotspot) clicked          ->  Levelhandler.&lt;methodName&gt;()
/// </summary>
public class InspectUI : MonoBehaviour
{
    public static InspectUI Instance { get; private set; }

    [Header("References (already wired up in the prefab)")]
    [SerializeField] private GameObject popup;                 // dimmer + window, hidden when closed
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private RectTransform viewArea;           // inspect views get spawned in here
    [SerializeField] private Image fallbackImage;              // used when an item only has a sprite
    [SerializeField] private Button closeButton;
    [SerializeField] private Button backgroundButton;          // the dark area around the window

    [Header("Small picture popup (used by hotspots with a Popup Sprite)")]
    [SerializeField] private GameObject detailPopup;
    [SerializeField] private Image detailImage;
    [SerializeField] private Button detailCloseButton;         // clicking anywhere on the popup closes it

    [Header("Behaviour")]
    [SerializeField] private bool closeWithEscape = true;
    [SerializeField] private bool closeOnBackgroundClick = true;

    [Header("Sound")]
    [Tooltip("Plays the first time each hotspot is clicked. Leave empty to use Resources/Audio/InspectSparkle.")]
    [SerializeField] private AudioClip discoverSound;
    [SerializeField, Range(0f, 1f)] private float discoverVolume = 0.6f;
    [Tooltip("Plays when an inventory item opens this window and when it is closed. Leave empty to use Resources/Audio/UIClick.")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField, Range(0f, 1f)] private float clickVolume = 0.5f;
    private AudioSource sfxSource;
    private AudioSource clickSource;

    private GameObject currentView;
    private InspectableItem currentItem;
    private Levelhandler levelHandler;

    // Remembers which "only once" hotspots were already clicked, so they stay
    // found when the same item is opened again.
    private readonly HashSet<string> usedHotspots = new HashSet<string>();

    // Every hotspot that has been clicked at least once (used for the discover sound).
    private readonly HashSet<string> discoveredHotspots = new HashSet<string>();

    public bool IsOpen => popup != null && popup.activeSelf;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("There is more than one InspectUI in the scene. Only one will be used.");
        }
        Instance = this;

        if (closeButton) closeButton.onClick.AddListener(Close);
        if (backgroundButton) backgroundButton.onClick.AddListener(() => { if (closeOnBackgroundClick) Close(); });
        if (detailCloseButton) detailCloseButton.onClick.AddListener(HideDetail);

        if (discoverSound == null) discoverSound = Resources.Load<AudioClip>("Audio/InspectSparkle");
        if (clickSound == null) clickSound = Resources.Load<AudioClip>("Audio/UIClick");
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.spatialBlend = 0f;
        sfxSource.ignoreListenerPause = true; // still plays if the game is paused while inspecting

        clickSource = gameObject.AddComponent<AudioSource>();
        clickSource.playOnAwake = false;
        clickSource.spatialBlend = 0f;
        clickSource.ignoreListenerPause = true;

        HideDetail();
        popup.SetActive(false);
    }

    void Start()
    {
        levelHandler = FindFirstObjectByType<Levelhandler>();
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void Update()
    {
        if (closeWithEscape && IsOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            // Esc closes the small picture popup first, then the whole Inspect UI.
            if (detailPopup != null && detailPopup.activeSelf) HideDetail();
            else Close();
        }
    }

    /// <summary>Opens the popup for an inventory item.</summary>
    public void Open(InspectableItem item)
    {
        if (item == null) return;

        PlayClick(1.0f);

        ClearView();
        HideDetail();
        currentItem = item;

        titleText.text = item.title;
        descriptionText.text = item.description;

        if (item.inspectView != null)
        {
            // Custom view prefab: image + clickable hotspots laid out in the editor.
            fallbackImage.gameObject.SetActive(false);
            currentView = Instantiate(item.inspectView, viewArea);

            RectTransform rt = currentView.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                rt.localScale = Vector3.one;
            }

            foreach (InspectHotspot hotspot in currentView.GetComponentsInChildren<InspectHotspot>(true))
            {
                string key = item.ItemKey + "/" + hotspot.gameObject.name;
                hotspot.Bind(this, key, usedHotspots.Contains(key));
            }
        }
        else
        {
            // No view prefab: just show the picture.
            fallbackImage.gameObject.SetActive(item.inspectSprite != null);
            fallbackImage.sprite = item.inspectSprite;
        }

        popup.SetActive(true);
    }

    public void Close() => Close(true);

    private void Close(bool withClick)
    {
        if (withClick && IsOpen) PlayClick(0.85f); // slightly lower pitch than opening

        ClearView();
        HideDetail();
        currentItem = null;
        popup.SetActive(false);
    }

    /// <summary>Shows a picture in the small popup on top of the Inspect window.</summary>
    public void ShowDetail(Sprite sprite)
    {
        if (detailPopup == null || sprite == null) return;
        detailImage.sprite = sprite;
        detailPopup.SetActive(true);
    }

    public void HideDetail()
    {
        if (detailPopup != null) detailPopup.SetActive(false);
    }

    /// <summary>Called by InspectHotspot when it is clicked.</summary>
    public void HotspotClicked(InspectHotspot hotspot, string key)
    {
        // Click/sparkle sound, only the first time this box is inspected
        if (discoveredHotspots.Add(key) && discoverSound != null)
        {
            sfxSource.PlayOneShot(discoverSound, discoverVolume);
        }

        if (!string.IsNullOrEmpty(hotspot.description))
        {
            descriptionText.text = hotspot.description;
        }

        if (hotspot.onlyOnce)
        {
            usedHotspots.Add(key);
            hotspot.SetUsed();
        }

        if (!string.IsNullOrEmpty(hotspot.methodName))
        {
            if (string.IsNullOrEmpty(hotspot.targetObject))
                CallLevelHandler(hotspot.methodName, hotspot.argument);
            else
                CallOnObject(hotspot.targetObject, hotspot.methodName, hotspot.argument);
        }

        hotspot.onClicked?.Invoke();

        if (hotspot.closeAfterClick)
        {
            Close(false); // the hotspot already made its own sound
        }
        else if (hotspot.popupSprite != null)
        {
            ShowDetail(hotspot.popupSprite);
        }
    }

    private void PlayClick(float pitch)
    {
        if (clickSound == null || clickSource == null) return;
        // Clicks have their own AudioSource so changing pitch never affects the sparkle
        clickSource.pitch = pitch;
        clickSource.PlayOneShot(clickSound, clickVolume);
    }

    private void CallOnObject(string objectName, string methodName, string argument)
    {
        GameObject target = GameObject.Find(objectName);
        if (target == null)
        {
            Debug.LogWarning($"InspectUI: no active GameObject named '{objectName}' in the scene, " +
                             $"so '{methodName}' could not be called.");
            return;
        }

        if (string.IsNullOrEmpty(argument))
            target.SendMessage(methodName, SendMessageOptions.RequireReceiver);
        else
            target.SendMessage(methodName, argument, SendMessageOptions.RequireReceiver);
    }

    /// <summary>Lets other scripts check whether a hotspot was already found,
    /// e.g. InspectUI.Instance.WasUsed("Level1_Wallet/Hotspot_Photo").</summary>
    public bool WasUsed(string key) => usedHotspots.Contains(key);

    private void CallLevelHandler(string methodName, string argument)
    {
        if (levelHandler == null)
        {
            levelHandler = FindFirstObjectByType<Levelhandler>();
        }

        if (levelHandler == null)
        {
            Debug.LogWarning($"InspectUI: no Levelhandler in this scene, so '{methodName}' could not be called. " +
                             "Add a GameObject with the Levelhandler script to the scene.");
            return;
        }

        // RequireReceiver logs an error if Levelhandler has no public method with that name,
        // which makes typos easy to spot in the Console.
        if (string.IsNullOrEmpty(argument))
        {
            levelHandler.SendMessage(methodName, SendMessageOptions.RequireReceiver);
        }
        else
        {
            levelHandler.SendMessage(methodName, argument, SendMessageOptions.RequireReceiver);
        }
    }

    private void ClearView()
    {
        if (currentView != null)
        {
            Destroy(currentView);
            currentView = null;
        }
    }
}
