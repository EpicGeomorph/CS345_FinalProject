using UnityEngine;
using DialogueEditor;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;
    [SerializeField] private float talkRange = 1.7f;
    private GameObject player;
    private CaseFile caseFile;

    void Start()
    {
        player = GameObject.Find("Player");
        caseFile = FindFirstObjectByType<CaseFile>();
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
            if (Input.GetKeyDown(KeyCode.E) && ClosestInRange() == this)
            {
                ConversationManager.Instance.StartConversation(myConversation);
                if (caseFile)
                {
                    caseFile.ShareWith(myConversation);
                }
            }
        }
    }

    // Only the nearest NPC in range answers E, so overlapping ranges never start two conversations.
    private ConversationStarter ClosestInRange()
    {
        ConversationStarter closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (ConversationStarter starter in FindObjectsByType<ConversationStarter>(FindObjectsSortMode.None))
        {
            float distance = Vector3.Distance(player.transform.position, starter.transform.position);
            if (distance < starter.talkRange && distance < closestDistance)
            {
                closest = starter;
                closestDistance = distance;
            }
        }

        return closest;
    }
}
