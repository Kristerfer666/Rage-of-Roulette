using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 10.0f;
    public Vector2 playerInput;
    public Rigidbody2D playerPos;
    public SpriteRenderer playerSprite;
    public SpriteRenderer meleeSprite;
    private bool flipBool = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerSprite.flipX = flipBool;
        meleeSprite.flipX = flipBool;
    }

    private void FixedUpdate()
    {
        Vector2 currentPos = playerPos.position;
        Vector2 movement = playerInput.normalized * speed * Time.fixedDeltaTime;
        Vector2 newPos = currentPos + movement;
        playerPos.MovePosition(newPos);
        if (playerInput.x < 0)
        {
            flipBool = true;
        }
        else if (playerInput.x > 0)
        {
            flipBool = false;
        }
    }
}
