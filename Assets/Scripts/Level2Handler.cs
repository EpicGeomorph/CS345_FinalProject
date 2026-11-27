using TMPro;
using UnityEngine;

public class Level2Handler : MonoBehaviour
{
    public GameObject enter_r;
    public GameObject enter_m;
    public TextMeshProUGUI quest_text;
    public GameObject init_chat;
    public GameObject init_waiter_chat;
    public GameObject mechanic_chat;
    public GameObject final_watier_chat;
    public GameObject inventory;
    public GameObject address_item;
    public bool talkedToMechanic = false;
    public bool talkedToWaiter = false;
    public bool gotAddress = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        quest_text.text = "Talk to the Supervisor";
    }

    public void InitLevel()
    {
        Destroy(init_chat);
        quest_text.text = "Ask the locals for info";

        enter_m.transform.position = new Vector3(-13.55f,4.9f,0);
        enter_r.transform.position = new Vector3(10.4f,5.26f,0);
    }

    public void GotMechanicConfession()
    {
        Destroy(mechanic_chat);
        talkedToMechanic = true;

        final_watier_chat.transform.position = init_waiter_chat.transform.position;
        Destroy(init_waiter_chat);
    }

    public void TakeAddress()
    {
        talkedToWaiter = true;
        Destroy(final_watier_chat);

        Instantiate(address_item, address_item.transform.position, Quaternion.identity, inventory.transform);
    }

    // Called by the Address note's inspect hotspot (Level2AddressInspectView > Hotspot_Address)
    public void GotAddress()
    {
        gotAddress = true;
    }
}
