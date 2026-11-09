using System.Collections;
using UnityEngine;

public class MultidirectionalSpriteAnimation : MonoBehaviour
{
	//This class is for those of you who want to give your characters unique animations in multiple directions, and supports both idle and walking animations.
	//we use a few concepts in this script that are not covered in the class, but I've included a lot of comments if you are curious

	//------------CAUTION CAUTION CAUTION---------------------------------
	//it is NOT recommended that you make any modifications to this script!
	//If you want to use it please get a reference to it from anotehr script and use the SetHeading function

	private SpriteRenderer spriteRenderer;

	public SpriteAnimationState[] animationStatesByDirection;

	SpriteAnimationState currentState;
	Sprite[] currentAnimationFrames;
	public float frameRate = 12;


	//Use this function to set the direction
	//you can use this on both NPCS and PCs!
	public void SetHeading(Vector2 aHeading)
	{
		var newSelectedFrames = SelectAnimationFramesByHeading(aHeading);

		//Don't bother doing anything if the selected frames are the same as before - we don't want to interrupt the animation constantly.
		if (newSelectedFrames == currentAnimationFrames)
			return;

		currentAnimationFrames = newSelectedFrames;

		//stop current coroutines
		StopAllCoroutines();
		//start a new one with the newly selected frames
		StartCoroutine(PlayRoutine(currentAnimationFrames));

	}


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

		if(animationStatesByDirection.Length==0) //a length of zero means there are no elements in the directions array
		{
			Debug.LogError("Make sure you add some animations to the MultidirectionalSpriteAnimation in the editor!");
			enabled = false;
			return;
		}

		currentState = animationStatesByDirection[0];
		SetHeading(Vector2.zero);
	}

	Sprite[] SelectAnimationFramesByHeading(Vector2 aHeading)
	{
		//if the magnitude is zero, don't change direction, just switch to idle.
		//magnitude is the 'length' of the vector. a vector of (0,0) has a length of 0, meaning we aren't moving anywhere.
		
		if (aHeading.magnitude == 0 && currentState.idleFrames.Length != 0)
		{
			spriteRenderer.flipX = currentState.flipX;
			spriteRenderer.flipY = currentState.flipY;
			
			//switch to using the idle frames.
			return currentState.idleFrames;
		}

		//figure out which of the states has the closest direction to the one given
			//we will do this by using Vector2.Angle to check the difference between the given heading and the animation state's direction
			//if we find one that is smaller than the current state, we replace the current state.
		float smallestAngleSoFar = Vector2.Angle(aHeading, currentState.direction);

		foreach(SpriteAnimationState animationState in animationStatesByDirection)
		{
			float angle = Vector2.Angle(aHeading, animationState.direction);
			if(angle<smallestAngleSoFar)
				currentState = animationState;
		}

		spriteRenderer.flipX = currentState.flipX;
		spriteRenderer.flipY = currentState.flipY;

		if (currentState.movingFrames.Length != 0)
			return currentState.movingFrames;
		else
			return currentState.idleFrames;
	}

	IEnumerator PlayRoutine(Sprite[] frames)
	{
		//this is a 'while' loop. It is a little like an if statement; it will execute the code between the {} repeatedly, as long as the conditions inside the () are true.
		//in this case, I want it to run forever, so I use while(true).
		//normally you would NEVER EVER do this, as this will cause an infinite loop, which will cause unity to freeze and crash, and make you lose unsaved work.
		//inside a coroutine it's sometimes okay, as long as you are using yield statements.

		//--------------------------------------------CAUTION CAUTION CAUTION ----------------------------------------------------
		//please be VERY careful if making any modifications to this while loop - it is easy to make your game freeze and crash with while loops.
		//--------------------------------------------CAUTION CAUTION CAUTION--------------------------------------------------------


		while (true)
		{
			yield return new WaitForEndOfFrame(); //yield return means to wait for something, in this case, till the end of the current frame
												  //technically this means the framerate won't be exactly correct sometimes, but I am using this here to be absolutely sure our while(true) will never crash the program.

			//this is a 'foreach' loop. It 'iterates' over each sprite in the array frames
			foreach (Sprite currentFrame in frames)
		
			{
				//we set the spriteRenderer to render the current frame 
				spriteRenderer.sprite = currentFrame;

				//wait one frame based on the framerate
				yield return new WaitForSeconds(1 / frameRate);
			}
		}
	}

	//you can create additional 'classes' to contain related data
	//and even nest them inside other classes.
	//in this case, this class holds all the data for a single animation direction
	[System.Serializable]
	public class SpriteAnimationState
	{
		public Vector2 direction;
		
		public Sprite[] idleFrames;
		public Sprite[] movingFrames;
		public bool flipX;
		public bool flipY;
	}
}