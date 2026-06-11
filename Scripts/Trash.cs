using UnityEngine;

public class Trash : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        CleanManager.Instance.RemoveTrash(this);
        Destroy(gameObject);
    }

    public string GetInteractText()
    {
        return "Limpiar";
    }
}