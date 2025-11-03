using UnityEngine;
using TMPro;

public class InventoryItemUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text quantityText;
    public TMP_Text weightText;

    public void SetData(Inventory inv)
    {
        if (inv == null) return;
        
        nameText.text = inv.itemName;
        quantityText.text = "Qty: " + inv.quantity.ToString();
        weightText.text = "Wt: " + inv.weight.ToString();
    }
}