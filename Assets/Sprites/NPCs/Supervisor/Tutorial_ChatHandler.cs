using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_ChatHandler : MonoBehaviour
{
    public TextMeshProUGUI quest_dialogue;
    public GameObject initial_chat;
    public GameObject wallet_chat;
    public GameObject identified_chat;
    public GameObject body_chat;
    public GameObject body;
    public GameObject inv_wallet;
    public GameObject inventory;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initial_chat.transform.position = transform.position;
        quest_dialogue.text = "Talk to the Supervisor";
    }

    public void FinishInitChat()
    {
        Destroy(initial_chat);
        body_chat.transform.position = body.transform.position;
        quest_dialogue.text = "Inspect the Body";
    }

    public void FoundWallet()
    {
        Destroy(body_chat);
        wallet_chat.transform.position = transform.position;

        GameObject new_wallet = Instantiate(inv_wallet, inv_wallet.transform.position,Quaternion.identity, inventory.transform);
        Button button = new_wallet.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => FoundPerson());

        quest_dialogue.text = "Inspect the Wallet";
    }

    public void FoundPerson()
    {
        Destroy(wallet_chat);

        identified_chat.transform.position = transform.position;
        quest_dialogue.text = "Talk to the Supervisor";
    }
}
