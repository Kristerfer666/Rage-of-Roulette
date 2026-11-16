using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int damage = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HurtPlayer(collision);
    }

    // Stay as well as Enter, so an enemy still pressed against the player hurts again once invincibility ends
    private void OnCollisionStay2D(Collision2D collision)
    {
        HurtPlayer(collision);
    }

    private void HurtPlayer(Collision2D collision)
    {
        if (CompareTag("Enemy") && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
