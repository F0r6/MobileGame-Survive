using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer texRender;

    public void DrawNoiseMap(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D tex = new Texture2D (width, height);

        Color[] colourMap = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap [x,y]);
            }
        }

        tex.SetPixels(colourMap);
        tex.Apply ();

        texRender.sharedMaterial.mainTexture = tex;
        texRender.transform.localScale = new Vector3(width, 1, height);
    }
}
