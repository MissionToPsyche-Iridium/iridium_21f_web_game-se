using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.VisualScripting;
//using UnityEditor.iOS;
using UnityEngine;

/* 
	Application: Probe builder
	File: containerManager 
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
	
	BuildManager buildManager;
	
	//[SerializeField] private int xOffset = 100;

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


	// probe will hold the components that are installed on the chassis
	private  Dictionary<Vector2, ProbeComponent> container = new Dictionary<Vector2, ProbeComponent>();

	void Start()
	{
		//these fields are entered in unity when you add this script to an object
		// this.width = 10;
		// this.height = 10;
		// this.tileScale = 100;
		// this.originX = 150;
		// this.originY = 0;

		// get the build manager (parent object) --> expect the mouse and keyboard interactions to be handled by the build manager
		// this container will handle tile interactions with the game shape object colliding with the tile
		this.buildManager = GameObject.Find("BuildManager").GetComponent<BuildManager>();
		Debug.Log($"Container Manager Initialized: {this.buildManager}");

		GenerateContainer();
	}

	void GenerateContainer()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				// instantiate a new tile
				if (tile != null)
				{
					var newTile = Instantiate(tile, new Vector3(originX + (tileScale * x), originY + (tileScale * y)), Quaternion.identity); //instantiates a new tile
					newTile.name = $"Tile {x} {y}"; //names new tile in hierarchy
					var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0); //gets whether tile is even or odd number
					newTile.Init(isOffset); //paints tile
					newTile.transform.SetParent(transform); //sets parent of tile to container
					newTile.transform.localScale = new Vector3(tileScale, tileScale, 100); //sets scale of tile
				}
			}
		}
	}

}
