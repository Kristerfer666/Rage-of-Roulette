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

    // frames per second the animation keeps playing at while no direction is held
    public float idleFrameRate = 2;

    // seconds the animation takes to ease down to the idle frame rate after input is
    // released, and to ease back up to the walking frame rate when input starts again
    public float slowDownTime = 0.2f;
    public float speedUpTime = 0.1f;

    [HideInInspector] public Facing facing = Facing.Right;
    [HideInInspector] public bool moving = false;

    private int skinIndex = 0;
    private bool flashing = false;
    private float framePos = 0f;
    private float currentFrameRate = -1f;

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
        float walkFrameRate = vertical ? verticalFrameRate : horizontalFrameRate;

        // the animation never freezes: it eases between the walking frame rate while
        // moving and the slow idle frame rate when no direction is held
        float target = moving ? walkFrameRate : idleFrameRate;
        float easeTime = moving ? speedUpTime : slowDownTime;
        if (currentFrameRate < 0f || easeTime <= 0f)
        {
            currentFrameRate = target;
        }
        else
        {
            float change = Mathf.Abs(walkFrameRate - idleFrameRate) / easeTime;
            currentFrameRate = Mathf.MoveTowards(currentFrameRate, target, change * Time.deltaTime);
        }

        // going down plays the vertical animation backwards
        float step = currentFrameRate * Time.deltaTime;
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
