using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class SpriteMerger : MonoBehaviour
{

    [SerializeField] private Sprite[] spritesToMerge = null;
    [SerializeField] private SpriteRenderer finalSpriteRenderer = null;

    private void Start() {
        Merge();
    }
    
    private void Merge() {
        Resources.UnloadUnusedAssets();
        var newTexture = new Texture2D(500,500); //change size here based on intiial sprites
        for(int x = 0; x < newTexture.width; x++) {
            for(int y = 0; y < newTexture.height; y++) {
                newTexture.SetPixel(x,y,new Color(1,1,1,0)); //transparent background
            }
        }

        for(int i = 0; i < spritesToMerge.Length; i++) {
            for(int x = 0; x < spritesToMerge[i].texture.width; x++) {
                for(int y = 0; y < spritesToMerge[i].texture.width; y++) {
                    //if current pixel is not transparent, draw it
                    var color = spritesToMerge[i].texture.GetPixel(x,y).a == 0 ? newTexture.GetPixel(x,y) : spritesToMerge[i].texture.GetPixel(x,y);
                    newTexture.SetPixel(x,y,color);
                }
            }
        }
    
    newTexture.Apply();
    var finalSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f));
    finalSprite.name = "New Sprite";
    finalSpriteRenderer.sprite = finalSprite;

    }
}
