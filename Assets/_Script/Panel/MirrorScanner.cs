using UnityEngine;
using System.Collections.Generic;

public class MirrorPanelClone : MonoBehaviour
{
    public Transform linkedPanel;
    public LayerMask targetLayer;
    public GameObject objectClonePrefab; // prefab c� SpriteRenderer + Collider

    private BoxCollider2D col;
    private Dictionary<Transform, GameObject> clones = new Dictionary<Transform, GameObject>();

    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        UpdateMirrorClones();
    }

    void UpdateMirrorClones()
    {
        // 1. X�c ??nh c�c object n?m trong v�ng qu�t c?a linkedPanel
        Collider2D[] hits = Physics2D.OverlapBoxAll(linkedPanel.position, col.size, 0f, targetLayer);
        HashSet<Transform> found = new HashSet<Transform>();

        foreach (var hit in hits)
        {
            Transform original = hit.transform;
            found.Add(original);

            // N?u clone ch?a t?n t?i ? t?o m?i
            if (!clones.ContainsKey(original))
            {
                Vector2 offset = original.position - linkedPanel.position;
                Vector2 mirroredPos = (Vector2)transform.position + offset;

                GameObject clone = Instantiate(objectClonePrefab, mirroredPos, original.rotation, transform);
                clones[original] = clone;
            }
            else
            {
                // Update v? tr� clone n?u c?n
                Vector2 offset = original.position - linkedPanel.position;
                Vector2 mirroredPos = (Vector2)transform.position + offset;
                clones[original].transform.position = mirroredPos;
            }
        }

        // 2. Xo� nh?ng clone kh�ng c�n t?n t?i trong v�ng
        List<Transform> toRemove = new List<Transform>();
        foreach (var pair in clones)
        {
            if (!found.Contains(pair.Key))
            {
                Destroy(pair.Value);
                toRemove.Add(pair.Key);
            }
        }

        foreach (var key in toRemove)
        {
            clones.Remove(key);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (col != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(linkedPanel.position, col.size);
        }
    }
}
