using TMPro;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    public Inventory playerInventory;
    public TextMeshProUGUI inventoryNum;
    
    // Update is called once per frame
    void Update()
    {
        inventoryNum.text = playerInventory.coinNum.ToString();
    }
}
