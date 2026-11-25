using UnityEngine;
using UnityEngine.UI;

public class Level1_Picture : MonoBehaviour
{
    Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(() => Clicked());
    }

    // Update is called once per frame
    void Clicked()
    {
        GameObject sup = GameObject.Find("Supervisor");
        if (sup)
        {
            Level1_Chat chat = sup.GetComponent<Level1_Chat>();
            if (chat && !chat.InspectedWallet)
            {
                
            }
        }
    }
}
