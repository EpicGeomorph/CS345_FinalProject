using UnityEngine;

public class Level2_Position : MonoBehaviour
{
    public GameObject player;
    public GameObject enter_r_chat;
    public GameObject leave_r_chat;
    public GameObject enter_m_chat;
    public GameObject leave_m_chat;

    public void EnterRestaurant()
    {
        player.transform.position = leave_r_chat.transform.position;
    }

    public void LeaveRestaurant()
    {
        player.transform.position = enter_r_chat.transform.position;
    }

    public void EnterMechanic()
    {
        player.transform.position = leave_m_chat.transform.position;
    }

    public void LeaveMechanic()
    {
        player.transform.position = enter_m_chat.transform.position;
    }
}
