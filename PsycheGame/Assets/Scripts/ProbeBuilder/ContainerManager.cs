using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ContainerManager : MonoBehaviour
{
	[SerializeField] private int width, height;
	[SerializeField] private Tile tile;
	[SerializeField] private int originX, originY;
	[SerializeField] private int tileScale;

	private int xOffset = 800;  // temporary fixed value between sandwiches -- to be dynamically calculated later

	//private Dictionary<Vector2, Tile> container;

	//generate container tiles

	void Start()
	{
		GenerateContainer();
	}

	public void GenerateContainer()
	{
		//Debug.Log("Generating Container: Tile is" + tile.name);
		for (int z = 0; z <3; z++)
		{
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					// instantiate a new tile
					if (tile != null)
					{
						var newTile = Instantiate(tile, new Vector3(originX + (tileScale * x) + xOffset * z, originY + (tileScale * y)), Quaternion.identity); //instantiates a new tile
						newTile.name = $"Tile {z} {x} {y}"; //names new tile in hierarchy
						var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0); //gets whether tile is even or odd number
						newTile.Init(isOffset); //paints tile
						newTile.transform.SetParent(transform); //sets parent of tile to container
						newTile.transform.localScale = new Vector3(tileScale, tileScale, 100); //sets scale of tile
					}
					//save to Dictionary container
					// container[new Vector2(x,y)] = newTile;
				}
			}
		}
	}


	//retreives a tile at a given position from container
	// public Tile GetTile(Vector2 position) {
	//     if(Container.TryGetValue(position, out var tile)) {
	//         return tile;
	//     }return null;
	// }

}
