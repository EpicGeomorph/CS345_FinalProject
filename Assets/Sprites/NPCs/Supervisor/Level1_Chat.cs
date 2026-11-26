using TMPro;
using UnityEngine;

public class Level1_Chat : MonoBehaviour
{
    public GameObject inventory;
    public TextMeshProUGUI quest_content;
    public GameObject corpse_chat;
    public GameObject init_chat;
    public GameObject inv_wallet;
    public GameObject inv_pass;
    public GameObject car_chat;
    public GameObject conclusion_chat;
    public GameObject inv_picture;

    public bool talkedToCorpse = false;
    public bool talkedToCar = false;
    public bool InspectedWallet = false;
    public bool InspectedPass = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        quest_content.text = "Talk to the Supervisor";
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfAllChecked();
    }

    public void InitCarCorpse()
    {
        Destroy(init_chat);
        quest_content.text = "Search around for clues";

        car_chat.transform.position = new Vector3(1.67f, -1.34f, 0);
        corpse_chat.transform.position = new Vector3(-3.59f, -3.92f, 0);
    }

    public void TakeWallet()
    {
        talkedToCorpse = true;
        Destroy(corpse_chat);

        Instantiate(inv_wallet, inv_wallet.transform.position, Quaternion.identity, inventory.transform);
        Instantiate(inv_pass, inv_pass.transform.position, Quaternion.identity, inventory.transform);
    }

    public void CheckIfAllChecked()
    {
        if (InspectedPass && InspectedWallet && talkedToCar && talkedToCorpse)
        {
            quest_content.text = "Talk to Supervisor";
            conclusion_chat.transform.position = transform.position;
        }
    }

    public void TakeScreenshot()
    {
        Destroy(car_chat);
        Instantiate(inv_picture, inv_picture.transform.position, Quaternion.identity, inventory.transform);
        talkedToCar = true;
    }

    // Called by the clickable text on the Press Pass inspect view.
    public void InspectPass()
    {
        InspectedPass = true;
    }
}
