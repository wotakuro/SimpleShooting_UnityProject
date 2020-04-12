using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureProcessor : AssetPostprocessor
{
    const string Dir = "Assets/1_画像置き場";
    [InitializeOnLoadMethod]
    public static void CreateFolder()
    {
        if (!Directory.Exists(Dir))
        {
            Directory.CreateDirectory(Dir);
            AssetDatabase.Refresh();
        }
    }
    void OnPreprocessTexture()
    {
        if( !this.assetPath.StartsWith(Dir)) { return; }
        TextureImporter textureImporter = (TextureImporter)assetImporter;
        textureImporter.isReadable = true;
        textureImporter.mipmapEnabled = false;
        textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
        textureImporter.textureType = TextureImporterType.Sprite;
    }
    
    void OnPostprocessTexture(Texture2D texture)
    {
        if (!this.assetPath.StartsWith(Dir)) { return; }
        if( !texture.isReadable) { return; }
        var pixels = texture.GetPixels();


        if(IsSameColor(pixels[0] , pixels[texture.width-1]) &&
            IsSameColor(pixels[0] , pixels[pixels.Length -1]) &&
            IsSameColor(pixels[0] , pixels[pixels.Length - texture.width]) &&
            !HasAlpha(pixels) )
        {
            bool dialogFlag = EditorUtility.DisplayDialog("四隅が同じ色です","四隅の色を透明化しますか？","はい","いいえ");
            if (dialogFlag)
            {
                var texturewWithAlpha = GenerateAlphaedTexture(pixels, texture.width, texture.height);
                var bytes = texturewWithAlpha.EncodeToPNG();
                File.Delete(assetPath);
                File.WriteAllBytes(Path.Combine(Dir, Path.GetFileNameWithoutExtension(assetPath) + ".png" ),bytes);
            }
        }
    }
    private bool IsSameColor(Color c1,Color c2)
    {
        return ((c1.r == c2.r) && (c1.g == c2.g) && (c1.b == c2.b));
    }

    private Texture2D GenerateAlphaedTexture(Color[] pixels,int width ,int height)
    {
        Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

        for( int i = 0; i < pixels.Length; ++i)
        {
            if(IsSameColor(pixels[i],pixels[0]) )
            {
                pixels[i].a = 0;
            }

        }
        texture.SetPixels(pixels);
        return texture;
    }


    private bool HasAlpha(Color[] pixels)
    {
        foreach( var pixel in pixels)
        {
            if( pixel.a < 1.0f) { return true; }
        }
        return false;
    }
}
