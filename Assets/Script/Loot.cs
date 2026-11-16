using UnityEngine;

public class Loot : MonoBehaviour
{
    // how far from the dead enemy's centre the ring of coins sits
    public float ringRadius = 0.8f;
    // seconds a coin takes to fly from the centre out to its spot
    public float flyTime = 0.3f;
    // size of the coin used when checking for walls along its path
    public float wallCheckRadius = 0.3f;

    private Vector2 flyStart;
    private Vector2 flyTarget;
    private float flyTimer = -1f;

    // Where each coin goes, relative to the centre, for a drop of `count` coins:
    // 1 stays in the centre, 2 go left and right, 3 make a triangle pointing up,
    // 4 sit on the corners of a square, 5 are the four corners plus one in the centre.
    // More than 5 are spread evenly round the ring.
    public static Vector2[] RingOffsets(int count, float radius)
    {
        Vector2[] offsets = new Vector2[count];
        if (count <= 1)
        {
            return offsets; // a single coin (or none) stays at the centre
        }

        int ringCount = count == 5 ? 4 : count;
        // odd counts start pointing straight up; even counts are turned half a step,
        // so 2 sit left and right and 4 sit on the corners instead of the edges
        float startAngle = ringCount % 2 == 1 ? 90f : 90f - 180f / ringCount;
        for (int i = 0; i < ringCount; i++)
        {
            float angle = (startAngle + 360f * i / ringCount) * Mathf.Deg2Rad;
            offsets[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        }
        // with 5 coins the last one is left at the centre (offset zero)
        return offsets;
    }

    // Start the coin at `start` and glide it out to `target`, stopping short of any wall in the way
    public void FlyOut(Vector2 start, Vector2 target)
    {
        flyStart = start;
        flyTarget = StopBeforeWall(start, target);
        transform.position = start;
        flyTimer = 0f;
    }

    private Vector2 StopBeforeWall(Vector2 start, Vector2 target)
    {
        Vector2 path = target - start;
        float length = path.magnitude;
        if (length <= 0f)
        {
            return target;
        }

        Vector2 dir = path / length;
        float allowed = length;
        foreach (RaycastHit2D hit in Physics2D.CircleCastAll(start, wallCheckRadius, dir, length))
        {
            if (hit.collider.CompareTag("Wall") && hit.distance < allowed)
            {
                allowed = hit.distance;
            }
        }
        return start + dir * allowed;
    }

    private void Update()
    {
        if (flyTimer < 0f)
        {
            return;
        }

        flyTimer += Time.deltaTime;
        float t = flyTime > 0f ? Mathf.Clamp01(flyTimer / flyTime) : 1f;
        // ease out: fast at first, then gently settling into place
        float eased = 1f - (1f - t) * (1f - t) * (1f - t);
        transform.position = Vector2.Lerp(flyStart, flyTarget, eased);
        if (t >= 1f)
        {
            flyTimer = -1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Inventory playerInventory = collision.gameObject.GetComponent<Inventory>();
            playerInventory.coinNum += 1;
            Destroy(gameObject);
        }
    }
}
