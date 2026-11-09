using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnKeypress : MonoBehaviour
{
    public int sceneToLoad = 0;

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
