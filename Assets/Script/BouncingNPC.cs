using UnityEngine;

public class BouncingNPC : MonoBehaviour
{
    public float speed = 2;
    public Rigidbody2D rb2d;
    public Vector2 direction;
    Rigidbody2D player2d;
    private bool attack = false;
    private int bounceCount = 0;

    private void Start()
    {
        // attack is not reset here: Start runs a frame after spawning, so resetting it
        // would undo Launch() and leave a freshly shot ball waiting for the player
        player2d = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // called by a shooter right after spawning this ball: it flies off immediately,
    // instead of waiting until the player is within range like a ball placed in the level
    public void Launch(Vector2 launchDirection)
    {
        direction = launchDirection;
        attack = true;
    }

    public void FixedUpdate()
    {
        Vector2 currentPos = rb2d.position;
        Vector2 playerPos = player2d.position;
        Vector2 distance = currentPos - playerPos;
        float currentDistance = distance.magnitude;
        if (currentDistance <= 10f || attack == true){
            Vector2 movement = direction * speed * Time.fixedDeltaTime;
            Vector2 newPos = currentPos + movement;
            rb2d.MovePosition(newPos);
            attack = true;
        }
    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != ("Player"))
        {
            Vector2 normal = collision.contacts[0].normal;
            direction = Vector2.Reflect(direction, normal);
            bounceCount++;
            if (bounceCount >= 20)
            {
                Destroy(gameObject);
            }
        }
    }
}
