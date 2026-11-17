using UnityEngine;
using DialogueEditor;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;
    private GameObject player;
    private bool canChat = true;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        if (Input.GetKeyDown(KeyCode.E))
    //        {
    //            ConversationManager.Instance.StartConversation(myConversation);
    //        }
    //    }
    //}

    public void InitChat()
    {
        canChat = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (player && canChat)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < 1.7 && Input.GetKeyDown(KeyCode.E))
            {
                ConversationManager.Instance.StartConversation(myConversation);
            }
        }
    }
}
