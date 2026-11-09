using UnityEngine;

public class DistanceNPC : MonoBehaviour
{
    public float speed = 2;
    public Rigidbody2D rb2d;
    Rigidbody2D player2d;

    private void Start()
    {
        player2d = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 currentPos = rb2d.position;
        Vector2 playerPos = player2d.position;
        Vector2 direction = currentPos - playerPos;

        float currentDistance = direction.magnitude;

        if (currentDistance < 2.5)
        {
            direction *= 1;
        } else if (currentDistance > 3.5)
        {
            direction *= -1;
        } else
        {
            direction *= 0;
        }
    }
}
