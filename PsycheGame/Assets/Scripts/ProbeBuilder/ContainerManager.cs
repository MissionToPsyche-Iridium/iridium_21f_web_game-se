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
	Probe builder :: containerManager.cs
	Description: this script is responsible for generating the sandwich chassis that holds the probe components.  

	version: 1.0 candidate (Jan 21)
	:: revise code to meet C# convention for performance and readability
	:: specifics - reduce redundant getcomponent calls
	
*/

class GridPositionData {
    public bool IsOccupied { get; set; }
    public string Occupant { get; set; }

	public GridPositionData()
	{
		IsOccupied = false;
		Occupant = string.Empty;
	}
}


public class ContainerManager : MonoBehaviour
{
	[SerializeField] private int width, height;
	[SerializeField] private Tile tile;
	[SerializeField] private int originX;
	[SerializeField] private int originY;
	[SerializeField] private int tileScale;
	
	private float PosX, PosY;

	private (float x, float y)[,] chassisGrid;
	private GridPositionData[,] gridData;
	public Material tileMaterial;
	private Sprite tileSprite;

	private int totalOccupations = 0;

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

		tileSprite = Resources.Load<Sprite>("Standard/T_02_Specular");

		GenerateContainer();
	}

	void GenerateContainer()
	{
		RectTransform parentRectTransform = GameObject.Find("MasterCanvas").GetComponent<RectTransform>();
		
		this.originX = (int)(parentRectTransform.rect.width / 2 * 0.65);
		this.originY = (int)(parentRectTransform.rect.height / 2 * 0.4);
		this.tileScale = (int)(parentRectTransform.rect.width / 18);

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (tile != null)
				{
					var targetX = originX + (tileScale * x * 0.93f);
					var targetY = originY + (tileScale * y * 0.93f);
					var newTile = Instantiate(tile, new Vector3(targetX, targetY, 0), Quaternion.identity);
					newTile.name = $"Tile {x} {y}";
					newTile.tag = "tile";

					var rigidbody2D = newTile.AddComponent<Rigidbody2D>();
					rigidbody2D.gravityScale = 0;

					var boxCollider2D = newTile.GetComponent<BoxCollider2D>();
					boxCollider2D.isTrigger = true;

					var spriteRenderer = newTile.GetComponent<SpriteRenderer>();
					spriteRenderer.sprite = tileSprite;

					var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
					newTile.Init(isOffset, x, y, targetX, targetY);

					chassisGrid[x, y] = (targetX, targetY);
					newTile.transform.SetParent(transform);
					newTile.transform.localScale = new Vector3(tileScale, tileScale, 100);
				}
			}
		}
	}

	public String CheckGridOccupied(int x, int y)
	{
		if (gridData[x, y].IsOccupied)
		{
			return gridData[x, y].Occupant;
		}
		else
		{
			return "";
		}
	}

	public bool CheckOccupationEligibility(int x, int y)
	{
		if (gridData[x, y].IsOccupied)
		{
			return false;
		}
		else if (totalOccupations < 1)
		{
			return true;
		}

		for (int i = x - 1; i <= x + 1; i++)
		{
			if (i < 0 || i >= width)
			{
				continue;
			}
			for (int j = y - 1; j <= y + 1; j++)
			{
				if (j < 0 || j >= height || (i == x && j == y))
				{
					continue;
				}
				else if (gridData[i, j].IsOccupied)
				{
					return true;
				}
			}
		}

		return false;
	}

	public bool ReleaseFromGridPosition(int x, int y, String objTag)
	{
		if (gridData[x, y].Occupant == objTag)
		{
			gridData[x, y].IsOccupied = false;
			gridData[x, y].Occupant = string.Empty;

			totalOccupations--;

            return true;
		}
		else
		{
			return false;
		}
	}

	public bool AssignToGridPosition(int x, int y, String objTag)
	{
		if (gridData[x, y].IsOccupied == false)
		{
			gridData[x, y].IsOccupied = true;
			gridData[x, y].Occupant = objTag;

			totalOccupations++;

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
