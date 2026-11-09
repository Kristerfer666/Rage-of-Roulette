using System.Collections;
using UnityEngine;

public class SimpleSpriteAnimation : MonoBehaviour 
{

	//======================CAUTION CAUTION CAUTION==========================================================================
	// It is not recommended that you modify this script! Please ask us for help if you want to modify it's behavior
	//==============================================================================================================

	//this class provides a simple way to add animation to your characters and objects.
	//Unity also has an animation system, but we wanted to avoid covering that in the class.
	//we use a few concepts in this script that are not covered in the class, but I've included a lot of comments if you are curious!

	//Assign the sprites you want to use as frames to the Sprite[] frames in the editor.
	//that variable is an array, denoted by [], meaning that it is a variable containing multiple objects - useful for all kinds of things!

	//the class then uses a Coroutine to run the animation at a reduced framerate.
	//a coroutine is a special kind of function, which returns IEnumerator. It allows us to 'yield' (basically, wait) for some amount of time before continuing.

	//this is an array of Sprites
	//the [] after the type denotes an array.
	public Sprite[] frames;

    public float frameRate = 12;
	public bool loop = true;
	public bool destroyOnComplete = false;

	//I didn't make this public, and instead use GetComponent to assign a reference.
	new SpriteRenderer renderer;
    
	private void OnEnable()
	{
		//make sure the renderer is assigned if we haven't gotten it already
        if(renderer == null)
		    renderer = GetComponent<SpriteRenderer>();

		//we use the StartCoroutine method to run a coroutine.
		StartCoroutine(PlayRoutine()); 
	}

	private void OnDisable()
	{
		//it's a little awkward to stop a specific coroutine, but in this case, we can use the simpler StopAllCoroutines, since there is only one.
        StopAllCoroutines();
	}

	IEnumerator PlayRoutine()
    {
		//this is a 'while' loop. It is a little like an if statement; it will execute the code between the {} repeatedly, as long as the conditions inside the () are true.
		//in this case, I want it to run forever, so I use while(true).
		//normally you would NEVER EVER do this, as this will cause an infinite loop, which will cause unity to freeze and crash, and make you lose unsaved work.
		//inside a coroutine it's sometimes okay, as long as you are using yield statements.
        
		///////////////CAUTION CAUTION CAUTION///////////////////////////
		// be VERY careful if modifying this function! It's easy to cause your game to crash with a while(true) loop!

		while (true)
        {
			yield return new WaitForEndOfFrame(); //yield return means to wait for something, in this case, till the end of the current frame
			//technically this means the framerate won't be exactly correct sometimes, but I am using this here to be absolutely sure our while(true) will never crash the program.

            //this is a 'foreach' loop. It 'iterates' over each sprite in the array frames
			foreach (var currentFrame in frames)
            {
				//we set the spriteRenderer to render the current frame 
                renderer.sprite = currentFrame;
                yield return new WaitForSeconds(1 / frameRate);
            }

			//if we do not want the animation to loop, break out of the while loop.
			if (loop == false) 
				break;	//break exits a loop early.
        }
		if (destroyOnComplete)
			Destroy(gameObject);
    }
}
