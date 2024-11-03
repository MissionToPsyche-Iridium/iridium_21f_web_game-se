using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class ChassisController : MonoBehaviour
{
    public Tilemap tilemap;
    TileBase[] allTiles;
    public GridInformation[,] gridInformation;
    public string probeTag = "ProbeItem";

    void Awake()
    {

        Debug.Log("Chassis Controller Initialized");
    }

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GameObject.Find("HappyTilemap").GetComponent<Tilemap>();

        BoundsInt bounds = tilemap.cellBounds;
        allTiles = tilemap.GetTilesBlock(bounds);

        GridLayout gridLayout = transform.parent.GetComponentInParent<GridLayout>();
        Vector3Int cellPosition = gridLayout.WorldToCell(transform.position);
        Vector3 tilePosition = tilemap.GetCellCenterLocal(cellPosition);

        gridInformation = new GridInformation[bounds.size.x, bounds.size.y];

        Debug.Log("Grid Size: " + gridLayout.cellSize);
        Debug.Log("Grid Gap: " + gridLayout.cellGap);
        Debug.Log("Grid Origin: " + gridLayout.cellLayout);
        Debug.Log("Cell Position: " + cellPosition);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];

                if (tile != null)
                {
                    Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);

                    gridInformation[x, y] = new GridInformation(new Vector3Int(x, y, 0), tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0)), tilemap.GetCellCenterLocal(new Vector3Int(x, y, 0)), false, null);
                    Debug.Log("Grid Position: " + gridInformation[x, y].GridPosition + " World Position: " + gridInformation[x, y].WorldPosition + " Tile position: " + tilePosition);
                }
                else
                {
                    //Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    // return cell position of the probe item
    public Vector3Int GetProbeItemCellPosition(GameObject item)
    {
        //GridLayout gridLayout = transform.parent.GetComponentInParent<GridLayout>();
        // find the tile position of the probe item
        GridLayout gridLayout = transform.parent.GetComponentInParent<GridLayout>();

        Vector3Int cellPosition = gridLayout.WorldToCell(item.transform.position);
        return cellPosition;
    }

    public void MoveProbeItemToGridPosition(GameObject item, Vector3 position)
    {
        Debug.Log("Incoming probe vector3 is: " + position + " and the item is: " + item);
        GridLayout gridLayout = transform.parent.GetComponentInParent<GridLayout>();
        Vector3Int cellPosition = gridLayout.WorldToCell(position);

        item.transform.position = gridLayout.CellToWorld(cellPosition) + new Vector3(0, 0, -0.01f);
    }
}
