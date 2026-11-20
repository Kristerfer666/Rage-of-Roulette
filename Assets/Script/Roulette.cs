using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Roulette : MonoBehaviour
{
    Rigidbody2D playerRb2d;
    public GameObject projectilePrefab;
    public Rigidbody2D rb2d;
    private bool closeToPlayer = false;
    private SimpleSpriteAnimation attackAnim;
    private bool dead = false;
    // every dice this boss has dropped that may still be in the air
    private List<Dice> droppedDice = new List<Dice>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerRb2d = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        attackAnim = GetComponent<SimpleSpriteAnimation>();

        Destructable destructable = GetComponent<Destructable>();
        if (destructable != null)
        {
            destructable.Died += OnDied;
        }
    }

    // called the instant the boss's health reaches 0, not when its explosion animation ends
    private void OnDied()
    {
        dead = true;
        CancelInvoke(nameof(StartBossAttack));
        StopAllCoroutines();
        attackAnim.enabled = false;

        foreach (Dice dice in droppedDice)
        {
            if (dice != null) // already gone if it landed and finished exploding
            {
                dice.Cancel();
            }
        }
        droppedDice.Clear();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (dead) return;

        Vector2 distance = rb2d.position - playerRb2d.position;
        bool isCloseToPlayer = distance.magnitude < 20f;
        if (isCloseToPlayer && !closeToPlayer){
            InvokeRepeating(nameof(StartBossAttack), 2f, 5f);
            closeToPlayer = true;
        }
    }

    void StartBossAttack()
    {
        if (dead) return;
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

            Dice dice = newProjectile.GetComponent<Dice>();
            if (dice != null)
            {
                droppedDice.Add(dice);
            }
            droppedDice.RemoveAll(d => d == null); // forget dice that have already exploded
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
