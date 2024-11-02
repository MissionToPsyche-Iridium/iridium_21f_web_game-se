using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(BoxCollider2D))]
public class TilemapBoundaryBox : MonoBehaviour
{
    private Tilemap tilemap;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        boxCollider = GetComponent<BoxCollider2D>();
        SetBoundary();
    }

    private void SetBoundary()
    {
        BoundsInt bounds = tilemap.cellBounds;

        Vector3 min = tilemap.CellToWorld(bounds.min);
        Vector3 max = tilemap.CellToWorld(bounds.max + Vector3Int.one);

        Vector3 colliderCenter = (min + max) / 2;
        Vector3 colliderSize = max - min;

        boxCollider.offset = colliderCenter - transform.position; 
        boxCollider.size = new Vector2(colliderSize.x, colliderSize.y);
        
        boxCollider.isTrigger = false;
    }
}