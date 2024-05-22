using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnKeyPress : MonoBehaviour
{
    public string sceneName; // Name of the scene to load

    private void Update()
    {
        // Check if any key is pressed
        if (Input.anyKeyDown)
        {
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        // Change scene
        SceneManager.LoadScene(sceneName);
    }
}