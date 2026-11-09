using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour
{
    Rigidbody2D playerRb2d;
    public GameObject dicePrefab;
    public Rigidbody2D rb2d;
    public float speed = 3;
    public Vector2 direction = new Vector2(0, -1);
    private Vector2 playerPos;
    private SimpleSpriteAnimation explodeAnim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb2d = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerPos = playerRb2d.position;
        explodeAnim = GetComponent<SimpleSpriteAnimation>();
        explodeAnim.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 currentPos = rb2d.position;
        if (currentPos.y >= playerPos.y){
            Vector2 movement = direction * speed * Time.fixedDeltaTime;
            Vector2 newPos = currentPos + movement;
            rb2d.MovePosition(newPos);
        }else{
            //GetComponent<Destructable>().DealDamage(gameObject.GetComponent<Destructable>().health);
            StartCoroutine(ExplodeRoutine());
        }
        
    }

    IEnumerator ExplodeRoutine()
    {
        explodeAnim.enabled = true;
        float duration = explodeAnim.frames.Length / explodeAnim.frameRate;
        yield return new WaitForSeconds(duration);
    }
}
