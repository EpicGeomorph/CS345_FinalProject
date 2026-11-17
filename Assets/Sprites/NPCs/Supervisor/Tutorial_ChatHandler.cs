using UnityEngine;

public class Tutorial_ChatHandler : MonoBehaviour
{
    public GameObject initial_chat;
    public GameObject wallet_chat;
    public GameObject identified_chat;
    public GameObject body_chat;
    public GameObject body;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initial_chat.transform.position = transform.position;
    }

    public void FinishInitChat()
    {
        Destroy(initial_chat);
        body_chat.transform.position = body.transform.position;
    }

    public void FoundWallet()
    {
        Destroy(body_chat);
        wallet_chat.transform.position = transform.position;
    }

    public void FoundPerson()
    {
        Destroy(wallet_chat);

        identified_chat.transform.position = transform.position;
    }
}
