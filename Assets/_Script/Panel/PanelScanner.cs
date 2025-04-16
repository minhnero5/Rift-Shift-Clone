// Script: PanelScanner.cs
using UnityEngine;

public class PanelScanner : MonoBehaviour
{
    public GameObject linkedObject; // Tham chi?u ??n t?m B ho?c A

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TriggerPoint")) // Object c� tag ?? theo d�i
        {
            Debug.Log($"{gameObject.name} ch?m v�o {other.name}");

            // Gi? s? b?n mu?n b?t m?t v?t th? con b�n trong linkedObject:
            Transform linkedChild = linkedObject.transform.Find(other.name);
            if (linkedChild != null)
            {
                linkedChild.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Transform linkedChild = linkedObject.transform.Find(other.name);
        if (linkedChild != null)
        {
            linkedChild.gameObject.SetActive(false);
        }
    }
}
