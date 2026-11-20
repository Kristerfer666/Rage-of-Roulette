using UnityEngine;
using UnityEngine.UI;

// Shows the player's health as a row of poker chips. Every heart the player still has shows the
// chip style the player is wearing at that HP, and lost hearts show the empty picture.
public class HeartDisplay : MonoBehaviour
{
    // leave empty to use the object tagged Player
    public Health playerHealth;

    // the Image for each heart, left to right
    public Image[] hearts;

    // the chip picture for each HP level: element 0 is the 1 HP chip, element 1 the 2 HP chip...
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
        // even flash steps (the player's white flash) show the empty picture on every flashing
        // heart, odd ones the chip
        bool flashing = flashUntil > hp;
        bool showChipWhileFlashing = playerHealth.FlashPhase % 2 == 1;

        // the chip style follows the player's skin: just after a hit the old style shows once
        // (flash step 1), then the new one, the same moment the player's own skin swaps
        int styleHP = (flashing && playerHealth.FlashPhase < 2) ? flashUntil : hp;
        Sprite chip = null;
        if (chipSprites.Length > 0)
        {
            chip = chipSprites[Mathf.Clamp(styleHP - 1, 0, chipSprites.Length - 1)];
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null) continue;

            // only show as many slots as the player's max HP
            hearts[i].gameObject.SetActive(i < playerHealth.maxHP);

            bool full;
            if (playerHealth.IsInvincible && i < Mathf.Max(flashUntil, hp))
            {
                // while the player flashes, the hearts left and the ones just lost all flash together
                full = showChipWhileFlashing;
            }
            else
            {
                full = i < hp;
            }
            hearts[i].sprite = (full || emptySprite == null) ? chip : emptySprite;
            hearts[i].color = full ? Color.white : emptyColor;
        }
    }
}
