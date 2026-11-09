using UnityEngine;

public class MeleeDetect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Player") || (collision.gameObject.tag == "Projectile"))
        {
            return;
        }
        if (collision.gameObject.tag == "Destructable" || (collision.gameObject.tag == "Enemy"))
        {
            Destructable destComp = collision.gameObject.GetComponent<Destructable>();
            destComp.DealDamage(1);
        }
    }
}
