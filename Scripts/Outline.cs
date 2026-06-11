using UnityEngine;

public class Outline : MonoBehaviour
{
    public Material outlineMaterial;
    public float outlineScale = 1.05f;

    private GameObject outlineObject;

    public void ShowOutline(bool show)
    {
        if (show)
        {
            if (outlineObject != null) return;

            outlineObject = new GameObject("Outline");
            outlineObject.transform.SetParent(transform);
            outlineObject.transform.localPosition = Vector3.zero;
            outlineObject.transform.localRotation = Quaternion.identity;
            outlineObject.transform.localScale = Vector3.one * outlineScale;

            MeshFilter mf = outlineObject.AddComponent<MeshFilter>();
            mf.mesh = GetComponent<MeshFilter>().mesh;

            MeshRenderer mr = outlineObject.AddComponent<MeshRenderer>();
            mr.material = outlineMaterial;
        }
        else
        {
            if (outlineObject != null)
            {
                Destroy(outlineObject);
                outlineObject = null;
            }
        }
    }
}