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

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                }
                else
                {
                    Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveProbeItemToGridPosition(GameObject item, Vector3 position)
    {
        Debug.Log("Incoming probe vector3 is: " + position + " and the item is: " + item);
        GridLayout gridLayout = transform.parent.GetComponentInParent<GridLayout>();
        Vector3Int cellPosition = gridLayout.WorldToCell(position);
        
        // get the tilebase object's coordinates
        // Vector3 tilePosition = tilemap.GetCellCenterLocal(cellPosition);
        
        Debug.Log("Item size is: " + item.transform.localScale);
        Debug.Log("Grid size is: " + gridLayout.cellSize + " and the cell position is: " + cellPosition);   
        Debug.Log("Grid Gap: " + gridLayout.cellGap);
        Debug.Log("Grid Origin: " + gridLayout.cellLayout);

        // compute new position of the probe based on the grid size and the cell position
        float newX = (cellPosition.x * 8f * 1.4f) + 700;
        float newY = (cellPosition.y * 8f * 1.4f) + 150;
        float newZ = 0;
        Vector3 newPosition = new Vector3(newX, newY, newZ);

        Debug.Log("[Cell Position: " + cellPosition + "] [New Position: " + newPosition + "]");

        item.transform.position = newPosition + new Vector3(0, 0, -0.01f);
    }

}
