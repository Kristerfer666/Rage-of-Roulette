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

    [HideInInspector] public Facing facing = Facing.Right;

    private int skinIndex = 0;
    private bool flashing = false;
    private float framePos = 0f;

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

        // going down plays the vertical animation backwards
        float step = frameRate * Time.deltaTime;
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
