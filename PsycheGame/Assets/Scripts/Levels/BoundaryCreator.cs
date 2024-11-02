using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(EdgeCollider2D))]
public class TilemapBoundary : MonoBehaviour
{
    private Tilemap tilemap;
    private EdgeCollider2D edgeCollider;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        edgeCollider = GetComponent<EdgeCollider2D>();

        SetBoundary();
    }

    private void SetBoundary()
    {
        BoundsInt bounds = tilemap.cellBounds;
        Vector3 bottomLeft = tilemap.CellToWorld(bounds.min);
        Vector3 topRight = tilemap.CellToWorld(bounds.max);

        Vector2[] points = new Vector2[5];
        points[0] = new Vector2(bottomLeft.x, bottomLeft.y); 
        points[1] = new Vector2(bottomLeft.x, topRight.y);   
        points[2] = new Vector2(topRight.x, topRight.y);     
        points[3] = new Vector2(topRight.x, bottomLeft.y);   
        points[4] = points[0];                               

        edgeCollider.points = points;
    }
}
