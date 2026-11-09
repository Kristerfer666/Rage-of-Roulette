using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	//I made this a reference to the Transform directly, since I don't care about any of the other components of the player game object
	public Transform player;

	//change the values of this to control where the camera looks relative to the player.
	public Vector2 offset;

	//this controls how quickly the camera moves towards the player position.
	public float lerpRate = 10f;

	public bool UseFixedUpdate = true;
	public bool UseLateUpdate = false;

	//we need to make sure the camera isn't sitting at exactly the same z position as the player (or it won't be able to render anything)
	//this is because even though we are making a 2D game, Unity always works with 3D coordinates. They're just kind of hidden in a 2D project.
	Vector3 depthOffset = new(0, 0, -10);

	private void Start()
	{
		//if the player hasn't been assigned,
		if(player == null)
		{
			//disable this script
			enabled = false;
		}
	}


	//Often we want the camera to move in LateUpdate, but since our player is moved in FixedUpdate, I modified this script to allow the camera to move in FixedUpdate as well to avoid glitchy looking movement.
	private void FixedUpdate()
	{
		if(UseFixedUpdate)
			Follow();
	}

	//LateUpdate is called after all Update() functions, and immediately before the camera renders the next frame
	//It's a good idea to use this function when moving the camera to prevent glitchy-looking camera movement.
	//(we let the player character move first in Update, then move the camera towards it)
	//However I realised we are moving the player in FixedUpdate, so I set up the script to allow both
	private void LateUpdate()
	{
		if(UseLateUpdate)
			Follow();
	}

	//I put all my logic for making the camera follow the player into this function,
	//so that I could reuse it easily in different places (as you can see above in LateUpdate and FixedUpdate)
	private void Follow()
	{
		//First, we calculate the new position the camera is moving towards.
		//I do this by adding all our offsets to the player position.
		Vector3 targetCameraPosition = player.position + depthOffset + (Vector3)offset;

		//I made offset a Vector2 so you don't accidentaly move the camera in the z axis.
		//however we need a 3D position for the camera.
		//In certain cases, you can convert one type of variable into another. This is called "casting".
		//I can cast the Vector2D offset to a 3D vector by adding (Vector3) right before it.
		

		//finally, we use Vector3.Lerp to move the camera.
		//this technique gives us a smooth movement that slows down as we approach the target position.
		transform.position = Vector3.Lerp(transform.position, targetCameraPosition, Time.deltaTime * lerpRate);

		//If you want to lock the camera to the player position, comment out the above line, and uncomment this:
		//transform.position = targetCameraPosition;
	}
}
