using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int maxHP = 5;
    public int startHP = 3;
    public int currentHP;

    public float invincibilityDuration = 4f;
    public float flashInterval = 0.25f;

    public PlayerAnimation playerAnim;

    private bool isInvincible = false;

    void Start()
    {
        currentHP = Mathf.Clamp(startHP, 1, maxHP);
        playerAnim.SetSkin(currentHP);
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHP -= amount;
        if (currentHP <= 0)
        {
            SceneManager.LoadScene("Game Over");
            return;
        }
        StartCoroutine(InvincibilityRoutine());
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        if (!isInvincible)
        {
            playerAnim.SetSkin(currentHP);
        }
    }

    IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            // even phases are white, odd phases show the chip
            int phase = (int)(elapsed / flashInterval);
            playerAnim.SetFlash(phase % 2 == 0);

            // the old skin shows once (phase 1); the new one is swapped in behind the second flash
            if (phase >= 2)
            {
                playerAnim.SetSkin(currentHP);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        playerAnim.SetFlash(false);
        playerAnim.SetSkin(currentHP);
        isInvincible = false;
    }
}
