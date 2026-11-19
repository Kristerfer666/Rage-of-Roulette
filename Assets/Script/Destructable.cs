using UnityEngine;
using System.Collections;
public class Destructable : MonoBehaviour
{
    public GameObject coinPrefab;
    public int health = 3;
    private SpriteRenderer sr;
    private Rigidbody2D rb2d;
    public Sprite normalSprite;
    public Sprite flashSprite;
    public SpriteRenderer spriteShade;
    public GameObject explodeAnimObject;
    private SimpleSpriteAnimation explodeAnim;
    private int lootMin;
    private int lootMax;

    // runs the moment health reaches 0, before the explosion animation plays
    public event System.Action Died;

    public bool IsDead => health <= 0;

    // set up in Awake rather than Start, so an object can be killed in the same frame it was spawned
    void Awake()
    {
        if (CompareTag("Destructable"))
        {
            lootMin = 2;
            lootMax = 5;
        }
        else if (CompareTag("Enemy"))
        {
            lootMin = 1;
            lootMax = 3;
        }
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = normalSprite;
        explodeAnim = explodeAnimObject.GetComponent<SimpleSpriteAnimation>();
        explodeAnim.enabled = false;
    }

    public void DealDamage(int amount)
    {
        // already dying: further hits must not explode it or drop coins a second time
        if (IsDead) return;

        health -= amount;
        if (health <= 0){
            Die(true);
        }else
        {
            StartCoroutine(FlashWhite());
        }
    }

    // destroy this object straight away (plays the explosion); dropLoot chooses whether coins come out
    public void Kill(bool dropLoot)
    {
        if (IsDead) return;
        health = 0;
        Die(dropLoot);
    }

    private void Die(bool dropLoot)
    {
        if (Died != null) Died();

        if (coinPrefab != null)
        {
            StartCoroutine(Explosion());
        }
        if (dropLoot && coinPrefab != null)
        {
            // coins start at the centre of the dead object and fly out into a ring around it
            int lootNum = Random.Range(lootMin, lootMax + 1);
            Vector2 centre = transform.position;
            Loot prefabLoot = coinPrefab.GetComponent<Loot>();
            float radius = prefabLoot != null ? prefabLoot.ringRadius : 0.8f;
            Vector2[] offsets = Loot.RingOffsets(lootNum, radius);
            for (int i = 0; i < lootNum; i++)
            {
                GameObject newLoot = Instantiate(coinPrefab, centre, Quaternion.identity);
                Loot loot = newLoot.GetComponent<Loot>();
                if (loot != null)
                {
                    loot.FlyOut(centre, centre + offsets[i]);
                }
                else
                {
                    newLoot.transform.position = centre + offsets[i];
                }
            }
        }
    }

    IEnumerator FlashWhite()
    {
        sr.sprite = flashSprite;
        yield return new WaitForSeconds(0.1f);
        sr.sprite = normalSprite; // revert back
    }

    IEnumerator Explosion()
    {
        if (spriteShade != null)
        {
            spriteShade.enabled = false;
        }
        rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        sr.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        explodeAnim.enabled = true;
        float duration = explodeAnim.frames.Length / explodeAnim.frameRate;
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
