using UnityEngine;
using UnityEditor;

public class RenderCubemapWizard : ScriptableWizard
{
    public Camera camera;

    void OnWizardUpdate()
    {
        helpString = "Select the camera position to render a Cubemap from";
        isValid = (camera != null);
    }

    void OnWizardCreate()
    {
        // Create a Cubemap
        Cubemap cubemap = new Cubemap(512, TextureFormat.ARGB32, false);
        camera.RenderToCubemap(cubemap);
        AssetDatabase.CreateAsset(cubemap, $"Assets/Cubemaps/{camera.name}.cubemap");
    }

    [MenuItem("Toolbox/Cubemap Wizard")]
    static void RenderCubemap()
    {
        DisplayWizard<RenderCubemapWizard>("Render cubemap", "Render");
    }
}
