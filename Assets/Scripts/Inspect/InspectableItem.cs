using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Put this on an inventory item prefab (e.g. Assets/InvItems/Level1_Wallet.prefab).
/// When the item's Button is clicked, the Inspect popup opens for it.
/// </summary>
public class InspectableItem : MonoBehaviour
{
    [Tooltip("Shown at the top of the Inspect popup.")]
    public string title = "Item";

    [Tooltip("Shown under the picture until a hotspot is clicked.")]
    [TextArea(2, 4)]
    public string description;

    [Tooltip("Prefab with the close-up picture and InspectHotspot buttons (see Assets/Fabs/InspectViews). " +
             "Leave empty to just show Inspect Sprite.")]
    public GameObject inspectView;

    [Tooltip("Used only when Inspect View is empty.")]
    public Sprite inspectSprite;

    [Tooltip("Used to remember which hotspots were found. Defaults to the prefab name if left empty.")]
    [SerializeField] private string itemId;

    [Tooltip("Optional. If the scene has no InspectCanvas, this one is spawned automatically " +
             "(drag in Assets/Fabs/InspectCanvas.prefab).")]
    [SerializeField] private GameObject inspectCanvasPrefab;

    public string ItemKey => string.IsNullOrEmpty(itemId) ? gameObject.name.Replace("(Clone)", "").Trim() : itemId;

    void Start()
    {
        Button button = GetComponentInChildren<Button>();
        if (button != null)
        {
            button.onClick.AddListener(Open);
        }
        else
        {
            Debug.LogWarning($"InspectableItem on '{name}' has no Button to click.", this);
        }
    }

    public void Open()
    {
        if (InspectUI.Instance == null && inspectCanvasPrefab != null)
        {
            Instantiate(inspectCanvasPrefab); // its InspectUI sets InspectUI.Instance in Awake
        }

        if (InspectUI.Instance == null)
        {
            Debug.LogWarning("No InspectCanvas in this scene. Drag Assets/Fabs/InspectCanvas.prefab into the scene.");
            return;
        }

        InspectUI.Instance.Open(this);
    }
}
