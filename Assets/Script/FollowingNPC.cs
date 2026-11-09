using UnityEngine;
using UnityEngine.InputSystem;

public class FollowingNPC : MonoBehaviour
{
    public float speed = 2;
    public Rigidbody2D rb2d;
    Rigidbody2D playerRb2d;
    public Vector2 direction;
    private bool attack = false;

    void Start()
    {
        playerRb2d = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        attack = false;
    }

    public void FixedUpdate()
    {
        Vector2 currentPos = rb2d.position;
        Vector2 playerPos = playerRb2d.position;
        Vector2 distance = currentPos - playerPos;
        float currentDistance = distance.magnitude;
        if (currentDistance <= 10f || attack == true){
            direction = playerRb2d.position - currentPos;
            Vector2 movement = direction.normalized * speed * Time.fixedDeltaTime;
            Vector2 newPos = currentPos + movement;
            rb2d.MovePosition(newPos);
            attack = true;
        }

    }

}
