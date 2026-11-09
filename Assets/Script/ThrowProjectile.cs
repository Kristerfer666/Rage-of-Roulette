using UnityEngine;

public class ThrowProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Inventory playerInventory = GetComponent<Inventory>();
            if (playerInventory.coinNum <= 0)
            {
                return;
            }else
            {
                playerInventory.coinNum -= 1;
                GameObject newProjectile = Instantiate (projectilePrefab);
                newProjectile.transform.position = transform.position;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 throwDirection = mousePos - (Vector2)transform.position;
                Projectile projectileComp = newProjectile.GetComponent<Projectile>();
                projectileComp.direction = throwDirection.normalized;
            }
            
        }
    }
}
