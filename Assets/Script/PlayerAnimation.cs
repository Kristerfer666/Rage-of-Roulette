using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public enum Facing { Right, Left, Up, Down }

    [System.Serializable]
    public class ChipSkin
    {
        public Sprite[] horizontalFrames;
        public Sprite[] verticalFrames;
    }

    public SpriteRenderer playerSprite;

    // Element 0 is the skin for 1 HP, element 1 for 2 HP, and so on
    public ChipSkin[] skins;
    public Sprite horizontalFlash;
    public Sprite verticalFlash;

    public float horizontalFrameRate = 12;
    public float verticalFrameRate = 12;

    // seconds the walk animation takes to ease to a stop after input is released,
    // and to ease back up to full speed when input starts again
    public float slowDownTime = 0.2f;
    public float speedUpTime = 0.1f;

    [HideInInspector] public Facing facing = Facing.Right;
    [HideInInspector] public bool moving = false;

    private int skinIndex = 0;
    private bool flashing = false;
    private float framePos = 0f;
    private float animSpeed = 0f;

    public void SetSkin(int hp)
    {
        skinIndex = Mathf.Clamp(hp - 1, 0, skins.Length - 1);
    }

    public void SetFlash(bool isFlashing)
    {
        flashing = isFlashing;
    }

    void Update()
    {
        bool vertical = facing == Facing.Up || facing == Facing.Down;
        ChipSkin skin = skins[skinIndex];
        Sprite[] frames = vertical ? skin.verticalFrames : skin.horizontalFrames;
        float frameRate = vertical ? verticalFrameRate : horizontalFrameRate;

        // ease the animation speed toward full while moving and toward zero when idle,
        // so the chip settles on a frame instead of freezing instantly
        float target = moving ? 1f : 0f;
        float easeTime = moving ? speedUpTime : slowDownTime;
        if (easeTime <= 0f)
        {
            animSpeed = target;
        }
        else
        {
            animSpeed = Mathf.MoveTowards(animSpeed, target, Time.deltaTime / easeTime);
        }

        // going down plays the vertical animation backwards
        float step = frameRate * animSpeed * Time.deltaTime;
        framePos += facing == Facing.Down ? -step : step;
        framePos = Mathf.Repeat(framePos, frames.Length);
        int frame = Mathf.Min((int)framePos, frames.Length - 1);

        playerSprite.flipX = facing == Facing.Left;
        if (flashing)
        {
            playerSprite.sprite = vertical ? verticalFlash : horizontalFlash;
        }
        else
        {
            playerSprite.sprite = frames[frame];
        }
    }
}
