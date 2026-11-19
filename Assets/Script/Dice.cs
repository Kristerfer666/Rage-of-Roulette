using UnityEngine;

public class Dice : MonoBehaviour
{
    Rigidbody2D playerRb2d;
    public GameObject dicePrefab;
    public Rigidbody2D rb2d;
    public float speed = 3;
    public Vector2 direction = new Vector2(0, -1);

    // each falling dice shows one of these faces, picked at random
    public Sprite[] faces;

    // damage the explosion deals to the player (once per dice)
    public int explosionDamage = 1;

    // the shadow on the ground where the dice will land. Its position in the prefab is its
    // offset from the landing spot, and its scale and colour there are its full, landed look
    public SpriteRenderer shadow;
    // while the dice is high up the shadow starts this fraction of its full size...
    public float shadowStartScale = 0.3f;
    // ...and this fraction of its full opacity, growing to both as the dice lands
    public float shadowStartOpacity = 0.2f;

    private SimpleSpriteAnimation explodeAnim;
    private Collider2D col;
    private Vector2 landingPos;
    private float startHeight;
    private bool exploding = false;
    private bool hitPlayer = false;

    private Vector3 shadowOffset;
    private Vector3 shadowFullScale;
    private Color shadowFullColor;

    void Awake()
    {
        // the falling dice is only a picture: nothing can touch it, and it hurts nobody
        col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        if (shadow != null)
        {
            shadowOffset = shadow.transform.localPosition;
            shadowFullScale = shadow.transform.localScale;
            shadowFullColor = shadow.color;
        }
    }

    // Start runs after the Roulette boss has placed the dice above the player
    void Start()
    {
        playerRb2d = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        explodeAnim = GetComponent<SimpleSpriteAnimation>();
        explodeAnim.enabled = false;

        // picked in Start, after Destructable's Awake has set its normal sprite, so this one wins
        PickRandomFace();

        // it lands straight below where it starts, at the height the player was standing
        landingPos = new Vector2(transform.position.x, playerRb2d.position.y);
        startHeight = Mathf.Max(transform.position.y - landingPos.y, 0.01f);
        UpdateShadow(transform.position.y);
    }

    private void PickRandomFace()
    {
        if (faces == null || faces.Length == 0) return;

        Sprite face = faces[Random.Range(0, faces.Length)];
        GetComponent<SpriteRenderer>().sprite = face;

        // keep Destructable in step, so anything that resets the dice to its normal sprite shows this face
        Destructable destructable = GetComponent<Destructable>();
        if (destructable != null)
        {
            destructable.normalSprite = face;
        }
    }

    void FixedUpdate()
    {
        if (exploding) return;

        Vector2 newPos = rb2d.position + direction * speed * Time.fixedDeltaTime;
        if (newPos.y <= landingPos.y)
        {
            // land exactly on the shadow, then explode
            rb2d.position = landingPos;
            transform.position = landingPos;
            Explode();
            return;
        }
        rb2d.MovePosition(newPos);
    }

    void LateUpdate()
    {
        if (!exploding)
        {
            UpdateShadow(transform.position.y);
        }
    }

    private void UpdateShadow(float diceY)
    {
        if (shadow == null) return;

        // 0 when the dice is at its starting height, 1 when it touches down
        float progress = 1f - Mathf.Clamp01((diceY - landingPos.y) / startHeight);

        // the shadow stays on the ground under the landing spot, not with the falling dice
        shadow.transform.position = (Vector3)landingPos + shadowOffset;
        shadow.transform.localScale = shadowFullScale * Mathf.Lerp(shadowStartScale, 1f, progress);
        Color c = shadowFullColor;
        c.a = shadowFullColor.a * Mathf.Lerp(shadowStartOpacity, 1f, progress);
        shadow.color = c;
    }

    private void Explode()
    {
        exploding = true;

        if (shadow != null)
        {
            shadow.enabled = false;
        }

        // the explosion is a damage zone the player can walk through, not a solid wall.
        // Untagged, so bullets and melee do not treat it as an enemy to shoot down
        gameObject.tag = "Untagged";
        if (col != null)
        {
            col.isTrigger = true;
            col.enabled = true;
        }

        // plays once and then removes the dice (Destroy On Complete in the prefab)
        explodeAnim.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryHitPlayer(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryHitPlayer(other);
    }

    private void TryHitPlayer(Collider2D other)
    {
        if (!exploding || hitPlayer || !other.CompareTag("Player")) return;

        Health playerHealth = other.GetComponent<Health>();
        if (playerHealth == null) return;

        hitPlayer = true;
        playerHealth.TakeDamage(explosionDamage);
    }
}
