using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.VisualScripting;
//using UnityEditor.iOS;
using UnityEngine;
using UnityEngine.UIElements;

/* 
	Application: Probe builder
	File: containerManager 
	version: 0.1
	Description: this script is responsible for generating the sandwich chassis that holds the probe components.  

	Assumptions (to be validated): this script will be developed in tandem with the (probe builder) user interface script (userInput) that performs
	mouse and keyboard interactions.  Based on the data design, the data structure components that holds the probe component information will be accessed by the
	userInput script to determine the position of the component relative to the chassis and makes the appropriate changes back to the data structure component.


	10/22 - Teague: data dictionary
	11/02 - Shawn: added code to position calculation of the grid container and size based on the parent rect transform
	11/04 - Added insertion of additional tile attributes during instantiaion -- [x,y] grid position and the target position on the canvas

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

	void Awake() 
	{
		Reset();
	}
	
	void Reset()
	{		// get parent rect transform size
		RectTransform parentRectTransform = GetComponent<RectTransform>();
		this.originX = (int)(parentRectTransform.rect.width / 2 * 0.8);
		this.originY = (int)(parentRectTransform.rect.height / 2 * 0.8);
	}

	void Start()
	{
		// get the build manager (parent object) --> expect the mouse and keyboard interactions to be handled by the build manager
		// this container will handle tile interactions with the game shape object colliding with the tile
		this.buildManager = GameObject.Find("BuildManager").GetComponent<BuildManager>();
		Debug.Log($"Container Manager Initialized: {this.buildManager}");

		GenerateContainer();
	}

	void GenerateContainer()
	{
		// dynamically generate the container based on the width and height
		RectTransform parentRectTransform = GameObject.Find("MasterCanvas").GetComponent<RectTransform>();
		Debug.Log($"Parent rect transform: {parentRectTransform.rect.width} {parentRectTransform.rect.height}");
		this.originX = (int)(parentRectTransform.rect.width / 2 * 0.7);
		this.originY = (int)(parentRectTransform.rect.height / 2 * 0.5);

		// set size proportionally to the parent rect transform -- need to adjust the spawn probe parts based on this also
		this.tileScale = (int)(parentRectTransform.rect.width / 20);

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				// instantiate a new tile
				if (tile != null)
				{
					// calculate the target position of the tile
					var targetX = originX + (tileScale * x);
					var targetY = originY + (tileScale * y);
					var newTile = Instantiate(tile, new Vector3(targetX, targetY, 0), Quaternion.identity); //instantiates a new tile
					newTile.name = $"Tile {x} {y}"; 		//names new tile in hierarchy
					newTile.transform.tag = "tile"; 		//tags tile as tile
					newTile.AddComponent<Rigidbody2D>(); 	//adds rigidbody to tile
					newTile.GetComponent<Rigidbody2D>().gravityScale = 0; //sets gravity scale to 0
					newTile.GetComponent<BoxCollider2D>().isTrigger = true; //sets box collider to trigger
					var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0); //gets whether tile is even or odd number
					newTile.Init(isOffset, x, y, targetX, targetY); //paints tile
					newTile.transform.SetParent(transform); //sets parent of tile to container
					// newTile.transform.localPosition = new Vector3(targetX, targetY, 0); //sets position of tile
					newTile.transform.localScale = new Vector3(tileScale, tileScale, 100); //sets scale of tile
				}
			}
		}
	}

}
