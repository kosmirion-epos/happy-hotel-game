using UnityEditor;
using UnityEngine;

public class ShaderToPNG : EditorWindow
{
    private Material material;
    private Vector2Int dimensions;
    private Texture image;
    private bool tileDisplay;

    [MenuItem("Window/Shader to .png")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ShaderToPNG));
    }

    private void OnEnable()
    {
        material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Editor/Utility/TextureGeneration/pngMaterial.mat");

        if (dimensions == Vector2Int.zero)
            dimensions = new Vector2Int(32, 32);
    }

    void OnGUI()
    {
        dimensions = EditorGUILayout.Vector2IntField(new GUIContent("Dimensions"), dimensions);

        if (GUILayout.Button(new GUIContent("Generate .png")))
        {
            Texture2D outputTex = new Texture2D(dimensions.x, dimensions.y, TextureFormat.ARGB32, false);
            RenderTexture buffer = new RenderTexture(dimensions.x, dimensions.y, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            Graphics.Blit(outputTex, buffer, material);
            RenderTexture.active = buffer;
            outputTex.ReadPixels(new Rect(0, 0, dimensions.x, dimensions.y), 0, 0, false);
            System.IO.File.WriteAllBytes(Application.dataPath + "/Editor/Utility/TextureGeneration/blebb.png", outputTex.EncodeToPNG());
            AssetDatabase.ImportAsset("Assets/Editor/Utility/TextureGeneration/blebb.png");
            image = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Utility/TextureGeneration/blebb.png");
        }

        tileDisplay = EditorGUILayout.Toggle(new GUIContent("Tile Display?"), tileDisplay);

        if (image != null)
        {
            GUI.DrawTexture(
                  new Rect(
                      (Screen.width - (!tileDisplay).ToInt() * dimensions.x) / 2,
                      (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2 + (Screen.height - dimensions.y) / 2,
                      dimensions.x,
                      dimensions.y
                  ),
                  image
              );

            if (tileDisplay)
                GUI.DrawTexture(
                      new Rect(
                          Screen.width / 2 - dimensions.x,
                          (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2 + (Screen.height - dimensions.y) / 2,
                          dimensions.x,
                          dimensions.y
                      ),
                      image
                  );
        }
    }
}
