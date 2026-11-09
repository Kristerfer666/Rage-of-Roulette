using UnityEngine;
using System.Collections;

public class AvoiderNPC : MonoBehaviour
{
    public float speed = 2;
    public Rigidbody2D rb2d;
    Rigidbody2D playerRb2d;
    public Vector2 direction;
    public GameObject projectilePrefab;
    private bool closeToPlayer = false;

    void Start()
    {
        playerRb2d = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    void ShootPlayer(){
        GameObject newProjectile = Instantiate(projectilePrefab);
        Vector2 throwDirection = playerRb2d.position - rb2d.position;
        newProjectile.transform.position = transform.position + (Vector3)(throwDirection.normalized * 1f);
        BouncingNPC projectileComp = newProjectile.GetComponent<BouncingNPC>();
        projectileComp.direction = throwDirection.normalized;
    }

    public void FixedUpdate()
    {
        Vector2 distance = rb2d.position - playerRb2d.position;
        float currentDistance = distance.magnitude;
        if (currentDistance <= 10f || closeToPlayer == true){
            if (!closeToPlayer)
            {
                InvokeRepeating(nameof(ShootPlayer), 1f, 4f);
            }
            Vector2 movement = distance.normalized * speed * Time.fixedDeltaTime;
            Vector2 newPos = rb2d.position + movement;
            rb2d.MovePosition(newPos);
            closeToPlayer = true;
        }
    }
}