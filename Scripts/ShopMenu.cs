using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopMenu : MonoBehaviour
{
    public static ShopMenu Instance;

    public GameObject shopPanel;
    public GameObject itemButtonPrefab;
    public Transform itemsPanel;
    public Button[] categoryButtons;
    public Color selectedColor = new Color(0.2f, 0.6f, 1f);
    public Color normalColor = Color.white;

    private bool isOpen = false;

    [System.Serializable]
    public class ShopItem
    {
        public string name;
        public int price;
        public string category;
        public GameObject prefab;
    }

    public ShopItem[] items;

    void Awake()
    {
        Instance = this;
    }

    public void ToggleShop()
    {
        isOpen = !isOpen;
        shopPanel.SetActive(isOpen);

        // Bloquear/desbloquear cursor
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;
    }

    public void ShowCategory(string category)
    {
        // Limpiar items anteriores
        foreach (Transform child in itemsPanel)
            Destroy(child.gameObject);

        // Mostrar items de la categoría
        foreach (ShopItem item in items)
        {
            if (item.category == category)
            {
                GameObject btn = Instantiate(itemButtonPrefab, itemsPanel);
                btn.GetComponentInChildren<TextMeshProUGUI>().text = item.name + "\n$" + item.price;
                ShopItem capturedItem = item;
                btn.GetComponent<Button>().onClick.AddListener(() => BuyItem(capturedItem));
            }
        }
    }

    public void SelectCategory(string category, Button selectedBtn)
    {
        // Resetear todos los botones
        foreach (Button btn in categoryButtons)
            btn.image.color = normalColor;

        // Resaltar el seleccionado
        selectedBtn.image.color = selectedColor;

        // Mostrar items de la categoría
        ShowCategory(category);
    }

    void BuyItem(ShopItem item)
    {
        if (MoneyManager.Instance.money < item.price)
        {
            Debug.Log("No hay suficiente dinero");
            return;
        }

        MoneyManager.Instance.money -= item.price;
        MoneyManager.Instance.UpdateMoneyText();

        // Colocar el objeto frente al jugador
        Camera cam = Camera.main;
        Vector3 spawnPos = cam.transform.position + cam.transform.forward * 2f;
        Instantiate(item.prefab, spawnPos, Quaternion.identity);

        Debug.Log("Compraste: " + item.name);
    }

    void Start()
    {
        shopPanel.SetActive(false);

        // Conectar botones automáticamente
        string[] categories = { "Mobiliarios", "Neveras", "Decoraciones", "Limpieza" };
        for (int i = 0; i < categoryButtons.Length; i++)
        {
            int index = i;
            categoryButtons[i].onClick.AddListener(() => SelectCategory(categories[index], categoryButtons[index]));
        }
    }
}