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
}
