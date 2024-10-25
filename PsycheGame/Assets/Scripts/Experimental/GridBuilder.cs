using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    // Start is called before the first frame update

    private GridXZ<GridObject> grid;

    public int gridWidth;
    public int gridHeight;
    public float cellSize;

    public class GridObject {
        
        private GridXZ<GridObject> grid;
        private int x;
        private int z;

        public GridObject(GridXZ<GridObject> grid, int x, int z) {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }
    }

    private void Awake()
    {
        gridWidth = 10;
        gridHeight = 10;
        cellSize = 100f;
        grid = new GridXZ<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(0, 0, 0), (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));
    }



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
