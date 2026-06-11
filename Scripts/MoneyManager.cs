using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public int money = 100;

    public TextMeshProUGUI moneyText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Debug.Log("MoneyText asignado: " + (moneyText != null));
        UpdateMoneyText();
    }

    public void AddMoney(int amount)
    {
        money += amount;

        UpdateMoneyText();
    }

    public void UpdateMoneyText()
    {
        Debug.Log("Actualizando texto: $" + money);
        moneyText.text = "$" + money;
        
    }
}
