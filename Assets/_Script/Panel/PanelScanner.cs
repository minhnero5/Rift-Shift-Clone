// Script: PanelScanner.cs
using UnityEngine;

public class PanelScanner : MonoBehaviour
{
    public PanelScanner linkedPanel; // K?t n?i t?i panel còn l?i
    public LayerMask detectableLayer;

    private void Update()
    {
        Collider[] hits = Physics.OverlapBox(transform.position, transform.localScale / 2f, Quaternion.identity, detectableLayer);

        foreach (var hit in hits)
        {
            // Copy ho?c di chuy?n object sang panel còn l?i
            Vector3 localOffset = hit.transform.position - transform.position;
            Vector3 targetPos = linkedPanel.transform.position + localOffset;

            // T?o b?n sao n?u ch?a có
            if (!linkedPanel.HasObject(hit.gameObject))
            {
                GameObject clone = Instantiate(hit.gameObject, targetPos, hit.transform.rotation);
                clone.name = hit.name + "_Clone";
                // Có th? ?ánh d?u clone n?u mu?n tránh clone ch?ng clone
            }
        }
    }

    public bool HasObject(GameObject original)
    {
        // Cách ki?m tra xem ?ã có b?n sao ch?a (tu? logic, có th? so name, tag, ho?c ?ánh d?u)
        return GameObject.Find(original.name + "_Clone") != null;
    }
}
