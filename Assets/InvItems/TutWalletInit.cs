using UnityEngine;
using UnityEngine.UI;

// The Inspect popup for this wallet is now handled by the InspectableItem
// component on the TutorialWallet prefab (title, description and picture are set there).
// This script is kept so you can add extra things to do when the wallet is clicked.
public class TutWalletInit : MonoBehaviour
{
    Button button;

    void Start()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(() => Clicked());
    }

    private void Clicked()
    {
        // Extra wallet-click logic goes here (the popup opens on its own).
    }
}
