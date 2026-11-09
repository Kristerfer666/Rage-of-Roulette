using UnityEngine;

public class DialogueExample : MonoBehaviour
{
	//Make sure you assign this in the editor by dragging and dropping the right text box into the field in the inspector.
    public GameObject dialogueBox;

	// Start is called once before Start

	private void Awake()
	{
		//when we have public references to objects, we need to make sure they are assigned
		//check if the dialogue box is not assigned a value by comparing it to null, like this:
		if (dialogueBox == null)
		{
			//In this case, I wrote an error message to help you remember to assign the variable
			Debug.LogError("Make sure to assign a dialoguebox object to the dialogue example script!");
		}
		else
		{
			//if the dialogue box is null, we hide it right away so you don't see any text.
			dialogueBox.SetActive(false);
		}
	}

	//use this function to detect when a Collider2D enters the trigger.
	//this requires that a Collider2D be attached to the same gameobject as this script, (not a regular Collider, but a Collider2D!)
	//and that the Collider2D has the "isTrigger" checkbox ticked.
	private void OnTriggerEnter2D(Collider2D aCollider)
	{
		//CompareTag tells us if the collider that touched the trigger is tagged "Player" in this case.
		//All Components have this function.
		if (aCollider.CompareTag("Player") == false)
			return; // return causes the function to end early, so we don't do anything if a non-player object touched the collider.

		//the SetActive function activates or deactivates a game object without destroying it.
		//it's equivalent to checking the box next to the gameobject's name in the inspector window.
		dialogueBox.SetActive(true);
    }


	//use this function to detect when a Collider2D exits the trigger.
	//this requires that a Collider2D be attached to the same gameobject as this script,
	//and that the collider has the "isTrigger" checkbox ticked.
	private void OnTriggerExit2D(Collider2D aCollider)
	{
		//this way of returning early if a certain condition isn't met is called a "guard clause", by the way!
		if (aCollider.CompareTag("Player") == false)
			return;

		dialogueBox.SetActive(false);

	}
}
