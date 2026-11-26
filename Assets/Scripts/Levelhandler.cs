using UnityEngine;
using UnityEngine.SceneManagement;

public class Levelhandler : MonoBehaviour
{
    public void NextLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        switch(currentScene.name)
        {
            case "Tutorial":
                SceneManager.LoadScene("Level_1");
                break;
            case "Level_1":
                SceneManager.LoadScene("Level_2");
                break;
            default:
                Debug.Log("Couldn't find scene");
                break;
        }
    }

    // ---------------------------------------------------------------
    // Inspect UI hotspots call these by name (InspectHotspot > Method Name).
    // Add a new public method here for every new hotspot you make.
    // ---------------------------------------------------------------

    public void Inspect_IDPhoto()
    {
        Debug.Log("Inspected the ID photo");
    }

    public void Inspect_IDName()
    {
        Debug.Log("Inspected the name on the ID");
    }

    public void Inspect_IDStar()
    {
        Debug.Log("Inspected the star on the ID");
    }
}
