using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/* 
	Application: Probe builder
	File: containerManager 
	version: 0.1
	Description: this script is responsible for generating the sandwich chassis that holds the probe components.  

	Assumptions (to be validated): this script will be developed in tandem with the (probe builder) user interface script (userInput) that performs
	mouse and keyboard interactions.  Based on the data design, the data structure components that holds the probe component information will be accessed by the
	userInput script to determine the position of the component relative to the chassis and makes the appropriate changes back to the data structure component.
*/

class GridPositionData {
	public bool is_occupied;
	public String occupant;

	public GridPositionData()
	{
		this.is_occupied = false;
		this.occupant = "";
	}
}


public class ContainerManager : MonoBehaviour
{
	[SerializeField] private int width, height;
	[SerializeField] private Tile tile;
	[SerializeField] private int originX, originY;
	[SerializeField] private int tileScale;
	
	private float PosX, PosY;

	private (float x, float y)[,] chassisGrid;
	private GridPositionData[,] gridData;  

	void Start()
	{
		chassisGrid = new (float x, float y)[width, height];
		gridData = new GridPositionData[width, height];
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				gridData[i, j] = new GridPositionData();
			}
		}

		GenerateContainer();
	}

	void GenerateContainer()
	{
		RectTransform parentRectTransform = GameObject.Find("MasterCanvas").GetComponent<RectTransform>();
		this.originX = (int)(parentRectTransform.rect.width / 2 * 0.7);
		this.originY = (int)(parentRectTransform.rect.height / 2 * 0.5);
		this.tileScale = (int)(parentRectTransform.rect.width / 20);

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (tile != null)
				{
					var targetX = originX + (tileScale * x);
					var targetY = originY + (tileScale * y);
					var newTile = Instantiate(tile, new Vector3(targetX, targetY, 0), Quaternion.identity);  
					newTile.name = $"Tile {x} {y}"; 		 
					newTile.transform.tag = "tile";
					newTile.AddComponent<Rigidbody2D>(); 	 
					newTile.GetComponent<Rigidbody2D>().gravityScale = 0;  
					newTile.GetComponent<BoxCollider2D>().isTrigger = true;

					var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);  
					newTile.Init(isOffset, x, y, targetX, targetY);    
					chassisGrid[x, y] = (targetX, targetY);
					newTile.transform.localScale = new Vector3(tileScale, tileScale, 100);
				}
			}
		}
	}

	public String CheckGridOccupied(int x, int y)
	{
		if (gridData[x, y].is_occupied)
		{
			return gridData[x, y].occupant;
		}
		else
		{
			return "";
		}
	}



	public bool ReleaseFromGridPosition(int x, int y, String objTag)
	{
		if (gridData[x, y].occupant == objTag)
		{
			gridData[x, y].is_occupied = false;
			gridData[x, y].occupant = "";
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool AssignToGridPosition(int x, int y, String objTag)
	{
		if (gridData[x, y].is_occupied == false)
		{
			gridData[x, y].is_occupied = true;
			gridData[x, y].occupant = objTag;
			return true;
		}
		else
		{
			return false;
		}
	}

	public (int, int) FindGridPosition(Vector3 position)
	{
		var x = (int) Math.Round((position.x - originX) / tileScale);
		var y = (int) Math.Round((position.y - originY) / tileScale);

		if (x < 0 || x > width || y < 0 || y > height) 
		{
			return (-1, -1);
		}
		return (x, y);
	}

	public (float, float) GetBeaconPosition()
	{
		return (this.PosX, this.PosY);
	}

	public (float, float) GetBeaconPositionGrid(int x, int y)
	{
		return (chassisGrid[x, y].x, chassisGrid[x, y].y);
	}

	public String SeedUniquId()
	{
		Time time = new Time();

		TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time); 
		string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

		int seedExt = UnityEngine.Random.Range(0, 100);
		String seedValue = timeText + seedExt.ToString();

		return seedValue;
	}

}
