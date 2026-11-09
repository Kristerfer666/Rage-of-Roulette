using UnityEngine;
using System.Collections;

public class Roulette : MonoBehaviour
{
    Rigidbody2D playerRb2d;
    public GameObject projectilePrefab;
    public Rigidbody2D rb2d;
    private bool closeToPlayer = false;
    private SimpleSpriteAnimation attackAnim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerRb2d = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        attackAnim = GetComponent<SimpleSpriteAnimation>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 distance = rb2d.position - playerRb2d.position;
        bool isCloseToPlayer = distance.magnitude < 20f;
        if (isCloseToPlayer && !closeToPlayer){
            InvokeRepeating(nameof(StartBossAttack), 2f, 5f);
            closeToPlayer = true;
        }
    }

    void StartBossAttack()
    {
        StartCoroutine(BossAttackRoutine());
    }

    IEnumerator BossAttackRoutine()
    {
        for (int i = 0; i < 6; i++)
        {
            yield return StartCoroutine(PlayAttackAnimation());

            GameObject newProjectile = Instantiate(projectilePrefab);
            Vector2 fallPos = new Vector2(playerRb2d.position.x, playerRb2d.position.y + 10f);
            newProjectile.transform.position = fallPos;
        }
    }

    IEnumerator PlayAttackAnimation()
    {
        attackAnim.enabled = true; // starts the animation from frame 0

        float duration = attackAnim.frames.Length / attackAnim.frameRate;
        yield return new WaitForSeconds(duration);

        attackAnim.enabled = false; // stops it cleanly
    }
}
