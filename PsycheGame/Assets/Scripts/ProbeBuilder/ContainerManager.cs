using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class ContainerManager : MonoBehaviour
{
  [SerializeField] private int width, height;
  [SerializeField] private Tile tile;
  [SerializeField] private int originX, originY;
  [SerializeField] private int tileScale;
//   private Dictionary<Vector2, Tile> container;

  //generate container tiles
  void Start(){
    GenerateContainer();
  }
  void GenerateContainer() {
    for(int x = 0; x < width; x++) {
        for(int y = 0; y < height; y++) {
            var newTile = Instantiate(tile, new Vector3(originX+(tileScale*x),originY+(tileScale*y)), Quaternion.identity); //instantiates a new tile
            newTile.name = $"Tile {x} {y}"; //names new tile in hierarchy
            var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0); //gets whether tile is even or odd number
            newTile.Init(isOffset); //paints tile

            //save to Dictionary container
            // container[new Vector2(x,y)] = newTile;
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
