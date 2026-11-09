using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed = 10;
    public Vector2 direction = new Vector2(0, 1);
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 currentPos = rb.position;
        Vector2 movement = direction * speed * Time.deltaTime;
        Vector2 newPos = currentPos + movement;
        rb.MovePosition(newPos);
        /* turning projectile direction to cursor
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle + 90;
        */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Player") || (collision.gameObject.tag == "Projectile"))
        {
            return;
        }
        if ((collision.gameObject.tag == "Destructable") || (collision.gameObject.tag == "Enemy"))
        {
            Destructable destComp = collision.gameObject.GetComponent<Destructable>();
            destComp.DealDamage(1);
            Destroy(gameObject);
        }else{
            Destroy(gameObject);
        }
    }
}
