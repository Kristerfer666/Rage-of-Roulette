using UnityEngine;

public class QuitOnEscape : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //I didn't want escape key to quit the game in edit mode,
            //since you need to hit "esc" to show the cursor!
            if (Application.isEditor)
            {
                Debug.Log("Would quit game if build");
            }
            else
            {
                //Quit the game
                Application.Quit();
            }
        }
    }
}
