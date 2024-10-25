using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridMaker : MonoBehaviour
{

    [SerializeField] public int[,] grid;
    [SerializeField] int vertical, horizontal, col, row;

    private Sprite sprite;

    GameObject parent = new GameObject("MasterCanvas");
    // Start is called before the first frame update

    void Awake() {
       
    }
    void Start()
    {
        vertical = (int)Camera.main.orthographicSize;
        horizontal = vertical * (Screen.width / Screen.height);
        col = horizontal * 2;
        row = vertical * 2;
        grid = new int[vertical, horizontal];
        for (int i = 0; i < vertical; i++) {
            for (int j = 0; j < horizontal; j++) {
                grid[i, j] = Random.Range(0, 100);
                spawnTile(i, j, grid[i, j]);
            }
        }

    }

    private void spawnTile(int x, int y, int value) {
        GameObject g = new GameObject("X: " + x + " Y: " + y);
        g.transform.parent = parent.transform;
        g.transform.position = new Vector3(x - (horizontal - 0.5f), y - (vertical - 0.5f), 0);
        var s = g.AddComponent<SpriteRenderer>();
        s.sprite = sprite;
        s.color = new Color(1, 1, 1, 0.5f);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
