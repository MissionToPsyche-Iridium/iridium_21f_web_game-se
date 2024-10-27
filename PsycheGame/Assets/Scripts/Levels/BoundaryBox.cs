using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapBoundaryBox : MonoBehaviour
{
    private Tilemap tilemap;
    private BoxCollider2D boxCollider;
    public float boundaryThickness = 0.1f;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        boxCollider = tilemap.AddComponent<BoxCollider2D>();
        CreateBoundary();
    }

    private void CreateBoundary()
    {
        BoundsInt bounds = tilemap.cellBounds;

        Vector3 min = tilemap.CellToWorld(bounds.min);
        Vector3 max = tilemap.CellToWorld(bounds.max + Vector3Int.one);

        CreateBoundaryBox(new Vector3(min.x + (max.x - min.x) / 2, max.y + boundaryThickness / 2, 0), new Vector2(max.x - min.x + boundaryThickness, boundaryThickness)); // Top
        CreateBoundaryBox(new Vector3(min.x + (max.x - min.x) / 2, min.y - boundaryThickness / 2, 0), new Vector2(max.x - min.x + boundaryThickness, boundaryThickness)); // Bottom
        CreateBoundaryBox(new Vector3(min.x - boundaryThickness / 2, min.y + (max.y - min.y) / 2, 0), new Vector2(boundaryThickness, max.y - min.y + boundaryThickness)); // Left
        CreateBoundaryBox(new Vector3(max.x + boundaryThickness / 2, min.y + (max.y - min.y) / 2, 0), new Vector2(boundaryThickness, max.y - min.y + boundaryThickness)); // Right
    }

    private void CreateBoundaryBox(Vector3 position, Vector2 size)
    {
        GameObject boundaryBox = new GameObject("BoundaryBox");
        boundaryBox.transform.position = position;
        BoxCollider2D boxCollider = boundaryBox.AddComponent<BoxCollider2D>();
        boxCollider.size = size;
        boxCollider.isTrigger = false; 
        boundaryBox.transform.parent = this.transform; 
    }
}