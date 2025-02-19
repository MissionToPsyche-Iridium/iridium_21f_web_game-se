using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/* 
	Probe builder :: containerManager.cs
	Description: this script is responsible for generating the sandwich chassis that holds the probe components.  

	version: 1.0 candidate (Jan 21)
	:: revise code to meet C# convention for performance and readability
	:: specifics - reduce redundant getcomponent calls
	
	version: 1.1 (Feb 6)
	:: revise code to use the color scheme set in the ContainerManager class by accessing the configuration set 
	in the Control Helper gameobject (script).  

	version: 1.2 (Feb 13)
	:: enhanced code to handle color scheme change from the builder scene CONTROL setting (toggle).  additionally, 
	updated the code with the ability to render the tile without loading from the START scene.
*/

class GridPositionData
{
    public bool IsOccupied { get; set; }
    public GameObject Occupant { get; set; }

    public GridPositionData()
    {
        IsOccupied = false;
        Occupant = null;
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
    public Material tileMaterial2;
    private Sprite tileSprite;
    private Sprite tileSprite2;

    private int totalOccupations = 0;
    private int colorProfile;
    private TileColorScheme colorScheme;
    private Volume volume;

    void Start()
    {
        colorScheme = this.GetColorScheme();
        volume = GameObject.Find("Box Volume").GetComponent<Volume>();
        updateColorScheme();

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
        tileSprite2 = Resources.Load<Sprite>("Standard/T_16_Emissive");

        // set tilematerial to standard material
        tileMaterial = Resources.Load<Material>("Sprites-Default");    
        tileMaterial2 = Resources.Load<Material>("EFX/FogMaterial");

		GenerateContainer();
	}

	private bool profileUpdate()
	{
		int profile;
		try {
			profile = GameObject.Find("ControlHelper").GetComponent<ControlHelper>().GetColorProfile();
		} catch (Exception e) {
			// Debug.Log("Control Helper not found: " + e.Message);
			profile = colorProfile;
		}

		if (profile != colorProfile)
		{
			colorProfile = profile;
			return true;
		}
		else return false;
	}

	public void UpdateColorScheme()
	{
		if (profileUpdate())
		{
			colorScheme = this.GetColorScheme();
			updateColorScheme();
		}
	}

	public void SetColorScheme(int colorScheme)
	{
		Debug.Log("CS++ SCS - Setting color scheme to " + colorScheme);
		if (colorScheme != colorProfile)
		{
			this.colorProfile = colorScheme;
			if (colorScheme == 1)
			{
				this.colorScheme = new TileStdScheme();
				UpdateColorScheme();
			}
			else
			{
				this.colorScheme = new TileAltScheme();
				UpdateColorScheme();
			}
		}
	}

	public (Color, Color, Color, Color) GetTileColors()
	{
		if (profileUpdate())
		{
			colorScheme = this.GetColorScheme();
			updateColorScheme();
		}
		return (colorScheme.GetColor1(), colorScheme.GetColor2(), colorScheme.GetOpenTileColor(), colorScheme.GetOccupiedTileColor());
	}

	public TileColorScheme GetColorScheme()
	{
		Camera mainCamera = Camera.main;

		try {
			GameObject controlHelper = GameObject.Find("ControlHelper");
			colorProfile = controlHelper.GetComponent<ControlHelper>().GetColorProfile();
		} catch (Exception e) {
			Debug.Log("Control Helper not found - testing builder scene only - code:" + e.Message);
			colorProfile = 1;
		}

        if (colorProfile == 1)
        {
            return new TileStdScheme();
        }
        else
        {
            Debug.Log("Using alternate color scheme");
            return new TileAltScheme();
        }
    }

    public void updateColorScheme()
    {
        volume.profile.TryGet<ColorAdjustments>(out var colorAdjustments);
        colorAdjustments.colorFilter.overrideState = true;
        colorAdjustments.postExposure.overrideState = true;
        colorAdjustments.postExposure.value = colorScheme.exposure;
        colorAdjustments.colorFilter.value = colorScheme.BaseSceneColor;
    }

    private Sprite GetMaterial(int x, int y) {
        if (y == 0 || y == height-1) {
            return tileSprite2;
        } else {
            return tileSprite;
        }
    }

    void GenerateContainer()
    {
        RectTransform parentRectTransform = GameObject.Find("MasterCanvas").GetComponent<RectTransform>();

        this.originX = (int)(parentRectTransform.rect.width / 2 * 0.70);
        this.originY = (int)(parentRectTransform.rect.height / 2 * 0.20);
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
                    newTile.name = $"Tile{x}{y}";
                    newTile.tag = "tile";

                    var rigidbody2D = newTile.AddComponent<Rigidbody2D>();
                    rigidbody2D.gravityScale = 0;

                    var boxCollider2D = newTile.GetComponent<BoxCollider2D>();
                    boxCollider2D.isTrigger = true;

                    var spriteRenderer = newTile.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = GetMaterial(x, y);

                    var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                    newTile.Init(isOffset, x, y, targetX, targetY);

                    chassisGrid[x, y] = (targetX, targetY);
                    newTile.transform.SetParent(transform);
                    newTile.transform.localScale = new Vector3(tileScale, tileScale, 100);
                }
            }
        }
    }

    public bool IsInteriorTile(int x, int y)
    {
        return (y == 0 || y == height - 1) ? false : true;
    }

	public String CheckGridOccupied(int x, int y)
	{
		if (gridData[x, y].IsOccupied)
		{
			return gridData[x, y].Occupant.name;
		}
		else
		{
			return String.Empty;
		}
	}

    public bool CanOccupyCell(int x, int y)
    {
        if (gridData[x, y].IsOccupied)
        {
            return false;
        }
        return true;
    }

    public bool IsReadyToSave()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Tile tile = transform.GetChild(i).gameObject.GetComponent<Tile>();

            if (CanOccupyCell(tile.GetCellX(), tile.GetCellY()))
            {
                continue;
            }

            bool hasNeighbors = false;

            for (int j = tile.GetCellX() - 1; i <= tile.GetCellX() + 1; i++)
            {
                if (hasNeighbors)
                {
                    break;
                }
                else if (j < 0 || j >= width)
                {
                    continue;
                }

                for (int k = tile.GetCellY() - 1; j <= tile.GetCellY() + 1; j++)
                {
                    if (hasNeighbors)
                    {
                        break;
                    }
                    else if (k < 0 || k >= height || (j == tile.GetCellX() && k == tile.GetCellY()))
                    {
                        continue;
                    }
                    else if (gridData[j, k].IsOccupied)
                    {
                        hasNeighbors = true;
                    }
                }
            }

            if (!hasNeighbors)
            {
                return false;
            }
        }
        return true;
    }

    private void updateTileMaterial(int x, int y, bool isOccupied)
    {
        // if occupied set material to tileMaterial2 else set to tileMaterial
        if (x == 0 || x == width - 1)
        {
            transform.GetChild(x * width + y).GetComponent<SpriteRenderer>().sprite = tileSprite2;
        }
        else
        {
            transform.GetChild(x * width + y).GetComponent<SpriteRenderer>().sprite = tileSprite;
        }
        transform.GetChild(x * width + y).GetComponent<SpriteRenderer>().material = isOccupied ? tileMaterial2 : tileMaterial;
    }

    public bool ReleaseFromGridPosition(int x, int y, GameObject component)
    {
        if (gridData[x, y].Occupant == component)
        {
            gridData[x, y].IsOccupied = false;
            gridData[x, y].Occupant = null;
            updateTileMaterial(x, y, false);

            totalOccupations--;

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool AssignToGridPosition(int x, int y, GameObject component)
    {
        if (gridData[x, y].IsOccupied == false)
        {
            gridData[x, y].IsOccupied = true;
            gridData[x, y].Occupant = component;
            updateTileMaterial(x, y, true);

            totalOccupations++;

            return true;
        }
        else
        {
            return false;
        }
    }

    public void SwapOccupants(int x1, int y1, int x2, int y2)
    {
        GameObject temp = gridData[x1, y1].Occupant;
        gridData[x1, y1].Occupant = gridData[x2, y2].Occupant;
        gridData[x2, y2].Occupant = temp;

        gridData[x1, y1].Occupant.GetComponent<SpriteDragDrop>().CurrentCell = new Tuple<int, int>(x1, y1);
        gridData[x2, y2].Occupant.GetComponent<SpriteDragDrop>().CurrentCell = new Tuple<int, int>(x2, y2);

        (float x, float y) position1 = GetBeaconPositionGrid(x1, y1);
        gridData[x1, y1].Occupant.transform.position = new Vector3(position1.x, position1.y, -0.01f);

        (float x, float y) position2 = GetBeaconPositionGrid(x2, y2);
        gridData[x2, y2].Occupant.transform.position = new Vector3(position2.x, position2.y, -0.01f);
    }

    public (int, int) FindGridPosition(Vector3 position)
    {
        var x = (int)Math.Round((position.x - originX) / tileScale);
        var y = (int)Math.Round((position.y - originY) / tileScale);

        if (x < 0 || x > width || y < 0 || y > height)
        {
            return (-1, -1);
        }
        return (x, y);
    }

    public (int, int) GetCellAtWorldPosition(Vector3 position)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform tile = transform.GetChild(i);
            if (Math.Abs(tile.position.x - position.x) <= tile.localScale.x / 2 && Math.Abs(tile.position.y - position.y) <= tile.localScale.y / 2)
            {
                Tile tileData = tile.GetComponent<Tile>();
                return (tileData.GetCellX(), tileData.GetCellY());
            }
        }
        return (-1, -1);
    }

    public (float, float) GetBeaconPosition()
    {
        return (this.PosX, this.PosY);
    }

    public (float, float) GetBeaconPositionGrid(int x, int y)
    {
        return (chassisGrid[x, y].x, chassisGrid[x, y].y);
    }
}
