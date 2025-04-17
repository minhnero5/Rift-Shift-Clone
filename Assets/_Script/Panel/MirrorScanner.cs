using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Gắn script này vào Panel A hoặc B. Script sẽ tự động quét vùng panel liên kết
/// và sinh ra clone các object có gắn CloneableObject trong vùng.
/// </summary>
public class MirrorPanelCloner : MonoBehaviour
{
    public Transform linkedPanel;                 // Panel còn lại để quét
    public LayerMask targetLayer;                 // Layer của object gốc
    public GameObject objectClonePrefab;          // Prefab dùng để clone (có collider)

    private BoxCollider2D scanCollider;           // Vùng quét
    private Dictionary<Transform, GameObject> clones = new(); // clone theo object gốc

    void Awake()
    {
        scanCollider = GetComponent<BoxCollider2D>();
        if (!scanCollider)
            Debug.LogError("Cần gắn BoxCollider2D lên panel để làm vùng quét!");
    }

    void Update()
    {
        if (!linkedPanel || !objectClonePrefab || scanCollider == null) return;

        SyncClones();
    }

    void SyncClones()
    {
        Vector2 scanSize = scanCollider.size;
        Vector2 scanCenter = linkedPanel.position;

        Collider2D[] hits = Physics2D.OverlapBoxAll(scanCenter, scanSize, 0f, targetLayer);
        HashSet<Transform> currentVisible = new();

        foreach (var hit in hits)
        {
            var cloneable = hit.GetComponentInParent<CloneableObject>();
            if (cloneable == null) continue;

            Transform original = cloneable.transform;
            if (currentVisible.Contains(original)) continue; // Tránh lặp

            currentVisible.Add(original);

            // Vị trí mirrored
            Vector2 offset = (Vector2)original.position - (Vector2)linkedPanel.position;
            Vector2 mirroredPos = (Vector2)transform.position + offset;

            if (!clones.ContainsKey(original) || clones[original] == null)
            {
                GameObject clone = Instantiate(objectClonePrefab, mirroredPos, original.rotation);
                clone.transform.localScale = original.lossyScale;
                clones[original] = clone;
            }
            else
            {
                GameObject clone = clones[original];
                clone.transform.position = mirroredPos;
                clone.transform.rotation = original.rotation;
                clone.transform.localScale = original.lossyScale;
            }
        }

        // Huỷ clone không còn trong vùng
        List<Transform> toRemove = new();
        foreach (var pair in clones)
        {
            if (!currentVisible.Contains(pair.Key))
            {
                Destroy(pair.Value);
                toRemove.Add(pair.Key);
            }
        }
        foreach (var key in toRemove) clones.Remove(key);
    }

    void OnDrawGizmosSelected()
    {
        if (scanCollider != null && linkedPanel != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(linkedPanel.position, scanCollider.size);
        }
    }
}
