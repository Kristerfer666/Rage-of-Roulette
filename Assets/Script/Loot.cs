using UnityEngine;

public class Loot : MonoBehaviour
{
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
