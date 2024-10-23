using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

/* 
	Script: containerManager 
	version: 0.1
	Description: this script is responsible for generating the sandwich chassis that holds the probe components.  

	Assumptions (to be validated): this script will be developed in tandem with the (probe builder) user interface script (userInput) that performs
	mouse and keyboard interactions.  Based on the data design, the data structure components that holds the probe component information will be accessed by the
	userInput script to determine the position of the component relative to the chassis and makes the appropriate changes back to the data structure component.


	10/22 - Teague: data dictionary

*/

public class ContainerManager : MonoBehaviour
{
	[SerializeField] private int width, height;
	[SerializeField] private Tile tile;
	[SerializeField] private int originX, originY;
	[SerializeField] private int tileScale;

	/*
		to-do: read/write to access data structure of the probe that describes the following:
			- the specific type of component installed
			- the position of the component relative to the chassis (z, x, y)
			- the orientation of the component (rotation)
			- the scale of the component
			
		the data structure should be a dictionary of the following form:
			- key: Vector3 (z, x, y)
			- value: Component (type, position, orientation, scale)

	*/


	private int xOffset = 800;  // temporary fixed value between sandwiches -- to be dynamically calculated later

	void Start()
	{
		GenerateContainer();
	}

	void GenerateContainer()
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
