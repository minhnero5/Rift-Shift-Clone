// Script: PanelScanner.cs
using UnityEngine;

public class PanelScanner : MonoBehaviour
{
    public PanelScanner linkedPanel; // K?t n?i t?i panel c�n l?i
    public LayerMask detectableLayer;

    private void Update()
    {
        Collider[] hits = Physics.OverlapBox(transform.position, transform.localScale / 2f, Quaternion.identity, detectableLayer);

        foreach (var hit in hits)
        {
            // Copy ho?c di chuy?n object sang panel c�n l?i
            Vector3 localOffset = hit.transform.position - transform.position;
            Vector3 targetPos = linkedPanel.transform.position + localOffset;

            // T?o b?n sao n?u ch?a c�
            if (!linkedPanel.HasObject(hit.gameObject))
            {
                GameObject clone = Instantiate(hit.gameObject, targetPos, hit.transform.rotation);
                clone.name = hit.name + "_Clone";
                // C� th? ?�nh d?u clone n?u mu?n tr�nh clone ch?ng clone
            }
        }
    }

    public bool HasObject(GameObject original)
    {
        // C�ch ki?m tra xem ?� c� b?n sao ch?a (tu? logic, c� th? so name, tag, ho?c ?�nh d?u)
        return GameObject.Find(original.name + "_Clone") != null;
    }
}
