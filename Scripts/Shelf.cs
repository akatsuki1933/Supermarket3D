using UnityEngine;

public class Shelf : MonoBehaviour, IInteractable
{
    [Header("Stock")]
    public int maxStock = 10;
    public int currentStock;

    [Header("Economy")]
    public int productPrice = 5;

    [Header("Restock")]
    public int restockCost = 20;

    private Renderer shelfRenderer;
    private Color originalColor;

    void Start()
    {
        currentStock = maxStock;
        shelfRenderer = GetComponent<Renderer>();
        if (shelfRenderer != null)
            originalColor = shelfRenderer.material.GetColor("_BaseColor");

        UpdateVisual();
    }

    public bool TakeProduct()
    {
        if (currentStock > 0)
        {
            currentStock--;
            UpdateVisual();
            return true;
        }
        return false;
    }

    public void Interact()
    {
        if (currentStock > 0)
            return;

        if (MoneyManager.Instance.money >= restockCost)
        {
            MoneyManager.Instance.money -= restockCost;
            currentStock = maxStock;
            MoneyManager.Instance.UpdateMoneyText();
            UpdateVisual();
            Debug.Log("Shelf reabastecido");
        }
        else
        {
            Debug.Log("No hay suficiente dinero");
        }
    }

    public string GetInteractText()
    {
        if (currentStock <= 0)
            return "Reabastecer ($" + restockCost + ")";
        else
            return "Shelf (" + currentStock + "/" + maxStock + ")";
    }

    void UpdateVisual()
    {
        if (shelfRenderer == null) return;

        if (currentStock <= 0)
            shelfRenderer.material.SetColor("_BaseColor", Color.red);
        else
            shelfRenderer.material.SetColor("_BaseColor", originalColor);
    }
}