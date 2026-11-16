using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 10.0f;
    public Vector2 playerInput;
    public Rigidbody2D playerPos;
    public PlayerAnimation playerAnim;
    public SpriteRenderer meleeSprite;
    public float attackAngleUp = 90f;
    public float attackAngleDown = -90f;
    private int lastInputX = 0;
    private int lastInputY = 0;
    private bool horizontalPressedLast = true;

    // Update is called once per frame
    void Update()
    {
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        UpdateFacing();
    }

    private void FixedUpdate()
    {
        Vector2 currentPos = playerPos.position;
        Vector2 movement = playerInput.normalized * speed * Time.fixedDeltaTime;
        Vector2 newPos = currentPos + movement;
        playerPos.MovePosition(newPos);
    }

    private void UpdateFacing()
    {
        // raw input, so the direction changes the moment a key is pressed or released
        float rawX = Input.GetAxisRaw("Horizontal");
        float rawY = Input.GetAxisRaw("Vertical");
        int inputX = rawX > 0 ? 1 : (rawX < 0 ? -1 : 0);
        int inputY = rawY > 0 ? 1 : (rawY < 0 ? -1 : 0);

        // remember which axis was pressed most recently, so it wins while both are held
        if (inputX != 0 && inputX != lastInputX)
        {
            horizontalPressedLast = true;
        }
        if (inputY != 0 && inputY != lastInputY)
        {
            horizontalPressedLast = false;
        }
        lastInputX = inputX;
        lastInputY = inputY;

        // the walk animation only plays while a direction is held
        playerAnim.moving = inputX != 0 || inputY != 0;

        // with no input the facing is left alone, so the chip keeps its last direction
        if (inputX != 0 && (inputY == 0 || horizontalPressedLast))
        {
            playerAnim.facing = inputX > 0 ? PlayerAnimation.Facing.Right : PlayerAnimation.Facing.Left;
        }
        else if (inputY != 0)
        {
            playerAnim.facing = inputY > 0 ? PlayerAnimation.Facing.Up : PlayerAnimation.Facing.Down;
        }

        float attackAngle = 0f;
        if (playerAnim.facing == PlayerAnimation.Facing.Up)
        {
            attackAngle = attackAngleUp;
        }
        else if (playerAnim.facing == PlayerAnimation.Facing.Down)
        {
            attackAngle = attackAngleDown;
        }
        meleeSprite.flipX = playerAnim.facing == PlayerAnimation.Facing.Left;
        meleeSprite.transform.localRotation = Quaternion.Euler(0f, 0f, attackAngle);
    }
}
