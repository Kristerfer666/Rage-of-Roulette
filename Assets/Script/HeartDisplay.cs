using UnityEngine;
using UnityEngine.UI;

// Shows the player's health as a row of poker chips: heart 1 is the 1 HP chip style,
// heart 2 the 2 HP style, and so on. Hearts the player still has are lit, lost ones are faded.
public class HeartDisplay : MonoBehaviour
{
    // leave empty to use the object tagged Player
    public Health playerHealth;

    // the Image for each heart, left to right
    public Image[] hearts;

    // the chip picture for each heart, in the same order as the hearts
    public Sprite[] chipSprites;

    // picture for a lost heart, tinted by emptyColor. Leave empty to show the chip tinted instead
    public Sprite emptySprite;
    public Color emptyColor = Color.white;

    // hearts from the player's current HP up to (not including) this one were just lost and
    // flash between the chip and the empty picture, in step with the player's own flash
    private int flashUntil = 0;
    private int lastHP = -1;

    void Start()
    {
        if (playerHealth == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<Health>();
            }
        }
    }

    void Update()
    {
        if (playerHealth == null) return;

        int hp = playerHealth.currentHP;
        if (lastHP >= 0 && hp < lastHP)
        {
            // hit again while still flashing: keep flashing every heart lost since the first hit
            flashUntil = Mathf.Max(flashUntil, lastHP);
        }
        lastHP = hp;
        if (!playerHealth.IsInvincible || flashUntil <= hp)
        {
            flashUntil = hp;
        }
        // even flash steps (the player's white flash) show the empty picture, odd ones the chip
        bool showChipWhileFlashing = playerHealth.FlashPhase % 2 == 1;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null) continue;

            // only show as many slots as the player's max HP
            hearts[i].gameObject.SetActive(i < playerHealth.maxHP);

            bool full = i < hp || (i < flashUntil && showChipWhileFlashing);
            Sprite chip = i < chipSprites.Length ? chipSprites[i] : null;
            hearts[i].sprite = (full || emptySprite == null) ? chip : emptySprite;
            hearts[i].color = full ? Color.white : emptyColor;
        }
    }
}
