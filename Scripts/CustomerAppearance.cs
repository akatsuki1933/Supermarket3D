using UnityEngine;

public class CustomerAppearance : MonoBehaviour
{
    public GameObject[] customerModels;

    void Start()
    {
        if (customerModels.Length == 0) return;

        // Elegir modelo al azar
        int index = Random.Range(0, customerModels.Length);

        // Instanciar el modelo elegido como hijo
        GameObject model = Instantiate(
            customerModels[index],
            transform.position,
            transform.rotation,
            transform
        );

        model.transform.localPosition = Vector3.zero;
    }
}