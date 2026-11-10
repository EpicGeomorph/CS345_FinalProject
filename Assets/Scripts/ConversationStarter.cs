using UnityEngine;
using DialogueEditor;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;
    private GameObject player;

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

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < 1.5 && Input.GetKeyDown(KeyCode.E))
            {
                ConversationManager.Instance.StartConversation(myConversation);
            }
        }
    }
}
