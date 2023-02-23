using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/* 元ネタ↓
 * https://qiita.com/gff02521/items/e37bcf4b4ad6ce152306
 * スペースキーで撮影できます。
 * Alphaの値を生かすので、カメラの背景は透明にしておきます。
 * カメラが一つだけなら、SolidColorにして塗りつぶし色のalphaを0とかにします。
 */

public class ScreenCapture : MonoBehaviour
{
    public string filePath;
    public string fileName;

    private void Awake()
    {
        if (filePath == null) {
            filePath = Application.dataPath;
        }
        if (fileName == null) {
            fileName = "test";
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(captureScreen( fileName +".png",filePath));
        }
    }

    IEnumerator captureScreen(string filename , string filepath)
    {
        yield return new WaitForEndOfFrame();

        //  キャプチャ用Texture2Dを作る
        var captureTex = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);

        //  ReadPixel
        captureTex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        captureTex.Apply();

        //  色を直す（PremultipliedAlphaなので戻す）
        var colors = captureTex.GetPixels(0, 0, captureTex.width, captureTex.height);
        for (int y = 0; y < captureTex.height; y++)
        {
            for (int x = 0; x < captureTex.width; x++)
            {
                Color c = colors[captureTex.width * y + x];
                if (c.a > 0.0f)
                {
                    c.r /= c.a;
                    c.g /= c.a;
                    c.b /= c.a;
                    colors[captureTex.width * y + x] = c;
                }
            }
        }
        captureTex.SetPixels(colors);
        captureTex.Apply();

        //  pngデータ取得
        byte[] pngData = captureTex.EncodeToPNG();

        //  破棄
        Destroy(captureTex);

        //  ファイルに保存
        var filePath = Path.Combine(filepath, filename);
        File.WriteAllBytes(filePath, pngData);
        Debug.Log("captured : " + filePath);
    }
}
