using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BuilderSpriteManager : MonoBehaviour
{
    public static readonly string DATA_PATH =
        Application.dataPath + Path.AltDirectorySeparatorChar +
        "Prefabs" + Path.AltDirectorySeparatorChar +
        "Probe Parts" + Path.AltDirectorySeparatorChar +
        "Custom Sprites";

    public static BuilderSpriteManager Instance { get; private set; }
    private static List<Sprite> sprites = new List<Sprite>();

    private static Texture2D ResizeTexture(Texture2D source, int newWidth, int newHeight)
    {
        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        Graphics.Blit(source, rt);

        RenderTexture.active = rt;
        Texture2D result = new Texture2D(newWidth, newHeight);
        result.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        result.Apply();

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        return result;
    }

    private static void LoadSprites()
    {
        if (!Directory.Exists(DATA_PATH))
        {
            Debug.LogError("Failed to find sprite directory path: " + DATA_PATH);
            return;
        }

        string[] imageFiles = Directory.GetFiles(DATA_PATH, "*.png");

        foreach (string file in imageFiles)
        {
            byte[] bytes = File.ReadAllBytes(file);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes); // Load PNG into texture
            texture.Apply();

            texture = ResizeTexture(texture, 100, 100);

            // Create sprite from texture
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            sprite.name = Path.GetFileName(file);
            sprites.Add(sprite);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        LoadSprites();
    }

    public static Sprite GetComponentSprite(string id) {
        foreach (Sprite sprite in sprites)
        {
            if (sprite.name.Equals(id + ".png"))
            {
                return sprite;
            }
        }
        return null;
    }
}
