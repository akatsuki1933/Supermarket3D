using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    public GameObject shelfPrefab;

    public int shelfCost = 50;

    private bool buildMode = false;

    void Update()
    {
        ToggleBuildMode();

        PlaceShelf();
    }

    void ToggleBuildMode()
    {
        // Tecla B
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            buildMode = !buildMode;

            Debug.Log("Build Mode: " + buildMode);
        }
    }

    void PlaceShelf()
    {
        if (!buildMode)
            return;

        // Click izquierdo
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Revisar dinero
            if (MoneyManager.Instance.money < shelfCost)
            {
                Debug.Log("No hay suficiente dinero");
                return;
            }

            Vector3 mousePos =
                Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            mousePos.z = 0;

            Instantiate(
                shelfPrefab,
                mousePos,
                Quaternion.identity
            );

            MoneyManager.Instance.money -= shelfCost;

            MoneyManager.Instance.UpdateMoneyText();

            Debug.Log("Shelf construido");
        }
    }
}
