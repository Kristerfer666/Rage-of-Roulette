using UnityEngine;

// Runs after the NPC movement scripts, so the bounce MovePosition below replaces theirs for that physics step
[DefaultExecutionOrder(100)]
public class Obstacle : MonoBehaviour
{
    public int damage = 1;
    public float bounceSpeed = 8f;
    public float bounceDuration = 0.2f;

    private Rigidbody2D rb2d;
    private Vector2 bounceDirection;
    private float bounceTimeLeft = 0f;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (bounceTimeLeft <= 0f) return;

        bounceTimeLeft -= Time.fixedDeltaTime;
        rb2d.MovePosition(rb2d.position + bounceDirection * bounceSpeed * Time.fixedDeltaTime);
        if (bounceTimeLeft <= 0f)
        {
            // drop whatever speed the impact left behind, so the enemy does not keep sliding
            rb2d.linearVelocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HitPlayer(collision, true);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HitPlayer(collision, false);
    }

    private void HitPlayer(Collision2D collision, bool isNewCollision)
    {
        if (!CompareTag("Enemy") || collision.gameObject.tag != "Player") return;

        // an enemy that is still bouncing off has already been counted for this hit
        if (bounceTimeLeft > 0f) return;

        // an enemy that is already dying neither hurts nor gets hurt again
        Destructable destructable = GetComponent<Destructable>();
        if (destructable != null && destructable.health <= 0) return;

        Health playerHealth = collision.gameObject.GetComponent<Health>();

        // Stay fires every physics step, so while the player is invincible only a fresh collision counts.
        // When the player is not invincible, an enemy still pressed against them hits again.
        if (playerHealth.IsInvincible && !isNewCollision) return;

        playerHealth.TakeDamage(damage);

        if (destructable != null)
        {
            destructable.DealDamage(1);
            if (destructable.health <= 0) return;
        }

        bounceDirection = (rb2d.position - collision.rigidbody.position).normalized;
        bounceTimeLeft = bounceDuration;
    }
}
