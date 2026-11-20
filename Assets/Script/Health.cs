using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int maxHP = 5;
    public int startHP = 3;
    public int currentHP;

    public float invincibilityDuration = 3f;
    public float flashInterval = 0.125f;
    // blocked damage that ends invincibility early (the hit that started it does not count)
    public int blockedDamageToEndInvincibility = 3;

    public PlayerAnimation playerAnim;

    private bool isInvincible = false;
    private float invincibleTime = 0f;
    private int blockedDamage = 0;

    public bool IsInvincible
    {
        get { return isInvincible; }
    }

    // which flash step the invincibility is on: even steps are the white flash, odd steps show the chip
    public int FlashPhase
    {
        get { return (int)(invincibleTime / flashInterval); }
    }

    void Start()
    {
        currentHP = Mathf.Clamp(startHP, 1, maxHP);
        playerAnim.SetSkin(currentHP);
    }

    void Update()
    {
        if (!isInvincible) return;

        invincibleTime += Time.deltaTime;
        if (invincibleTime >= invincibilityDuration)
        {
            EndInvincibility();
            return;
        }

        // even phases are white, odd phases show the chip
        int phase = (int)(invincibleTime / flashInterval);
        playerAnim.SetFlash(phase % 2 == 0);

        // the old skin shows once (phase 1); the new one is swapped in behind the second flash
        if (phase >= 2)
        {
            playerAnim.SetSkin(currentHP);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible)
        {
            blockedDamage += amount;
            if (blockedDamage >= blockedDamageToEndInvincibility)
            {
                EndInvincibility();
            }
            return;
        }

        currentHP -= amount;
        if (currentHP <= 0)
        {
            SceneManager.LoadScene("Game Over");
            return;
        }

        isInvincible = true;
        invincibleTime = 0f;
        blockedDamage = 0;
        playerAnim.SetFlash(true);
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        if (!isInvincible)
        {
            playerAnim.SetSkin(currentHP);
        }
    }

    private void EndInvincibility()
    {
        isInvincible = false;
        playerAnim.SetFlash(false);
        playerAnim.SetSkin(currentHP);
    }
}
