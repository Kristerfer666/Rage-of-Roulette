using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AvoiderNPC : MonoBehaviour
{
    public float speed = 2;
    public Rigidbody2D rb2d;
    Rigidbody2D playerRb2d;
    public Vector2 direction;
    public GameObject projectilePrefab;
    // how far in front of this enemy, toward the player, a shot ball appears
    public float spawnDistance = 1f;
    private bool closeToPlayer = false;

    // starts chasing and shooting once the player comes this close
    public float noticeDistance = 10f;
    // the distance this enemy tries to keep from the player
    public float preferredDistance = 5f;
    // how far either side of the preferred distance still counts as "fine": it stays put there
    public float distanceTolerance = 1f;
    // how many units past the comfortable range it takes to reach full speed,
    // so it moves slowly when only a little off and faster when far off
    public float speedRampDistance = 2f;
    // how quickly it speeds up and slows down (units per second, per second)
    public float acceleration = 4f;
    // top speed when following a player who walks away; "speed" is the top speed
    // when backing off. Match the player's speed (5) so it can keep the distance
    public float followSpeed = 5f;

    private Vector2 currentVelocity = Vector2.zero;

    // when this enemy dies, every white ball it shot dies with it; tick to let those drop coins too
    public bool shotsDropCoinsWhenKilled = false;

    private List<Destructable> shots = new List<Destructable>();
    private bool dead = false;

    void Start()
    {
        playerRb2d = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

        Destructable destructable = GetComponent<Destructable>();
        if (destructable != null)
        {
            destructable.Died += OnDied;
        }
    }

    // called the instant health reaches 0, not when the explosion animation ends
    private void OnDied()
    {
        dead = true;
        CancelInvoke(nameof(ShootPlayer));

        foreach (Destructable shot in shots)
        {
            if (shot != null) // already gone if it bounced 20 times or was shot down
            {
                shot.Kill(shotsDropCoinsWhenKilled);
            }
        }
        shots.Clear();
    }

    void ShootPlayer(){
        if (dead) return;

        GameObject newProjectile = Instantiate(projectilePrefab);
        Vector2 throwDirection = playerRb2d.position - rb2d.position;
        newProjectile.transform.position = transform.position + (Vector3)(throwDirection.normalized * spawnDistance);

        // the shot ball spawns touching this enemy, so without this they collide at once:
        // the ball bounces back into the enemy and gets dragged along with it
        foreach (Collider2D shooterCol in GetComponentsInChildren<Collider2D>())
        {
            foreach (Collider2D ballCol in newProjectile.GetComponentsInChildren<Collider2D>())
            {
                Physics2D.IgnoreCollision(shooterCol, ballCol);
            }
        }

        BouncingNPC projectileComp = newProjectile.GetComponent<BouncingNPC>();
        projectileComp.Launch(throwDirection.normalized);

        Destructable shotDestructable = newProjectile.GetComponent<Destructable>();
        if (shotDestructable != null)
        {
            shots.Add(shotDestructable);
        }
    }

    public void FixedUpdate()
    {
        if (dead) return;

        Vector2 awayFromPlayer = rb2d.position - playerRb2d.position;
        float currentDistance = awayFromPlayer.magnitude;

        if (!closeToPlayer)
        {
            if (currentDistance > noticeDistance) return;
            closeToPlayer = true;
            InvokeRepeating(nameof(ShootPlayer), 1f, 4f);
        }

        // positive when too close (back away), negative when too far (come closer),
        // zero anywhere inside the comfortable range
        float tooClose = (preferredDistance - distanceTolerance) - currentDistance;
        float tooFar = currentDistance - (preferredDistance + distanceTolerance);
        float push = 0f;
        if (tooClose > 0f) push = tooClose;
        else if (tooFar > 0f) push = -tooFar;

        float speedFraction = speedRampDistance > 0f ? Mathf.Clamp(push / speedRampDistance, -1f, 1f) : Mathf.Sign(push);
        float topSpeed = speedFraction < 0f ? followSpeed : speed;
        Vector2 targetVelocity = awayFromPlayer.normalized * topSpeed * speedFraction;

        // ease the actual velocity toward the target, so it glides away and settles
        // instead of snapping to full speed or stopping dead
        currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        rb2d.MovePosition(rb2d.position + currentVelocity * Time.fixedDeltaTime);
    }
}
