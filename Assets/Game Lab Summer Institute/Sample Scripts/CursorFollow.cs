using UnityEngine;

public class CursorFollow : MonoBehaviour
{
	//use this [SerializeField] tag to make private variables visible in the editor
	[SerializeField] 
    private bool showDefaultCursor = false;

    //I didn't use serializefield here so you don't accidentally change this in the editor :)
    private float ZDepth = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //this is a function which I defined below!
        SetCursorVisibility();
    }

    // Update is called once per frame
    void Update()
    {
		//uncomment this function to make cursor visibility update in real time
        //SetCursorVisibility();
	}

    // LateUpdate is called once per frame after all Update functions have been called.
    // it is useful for things like moving the camera, and helps prevent stuttering.
	private void LateUpdate()
	{
        //get the current position of the mouse in screen coordinates
        Vector2 mousePos = Input.mousePosition;
        
        //ScreenToWorldPoint converts from a screen space coordinate to a worldspace coordinate
        Vector3 cursorWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        //ScreenToWorldPoint doesn't know what "z" or depth we want for the position, so its best to set this ourselves.
        cursorWorldPos.z = ZDepth;

        //finally, we assign 
       transform.position = cursorWorldPos;	
	}

	//set cursor visibility based on showDefaultCursor. In C# you are allowed to write single line functions with => instead of {} which saves a little space.
	void SetCursorVisibility() => Cursor.visible = showDefaultCursor;
	
}
