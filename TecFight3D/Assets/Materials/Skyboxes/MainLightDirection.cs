using UnityEngine;

[ExecuteInEditMode]
public class MainLightDirection : MonoBehaviour
{
    [SerializeField] private Material skyboxMaterial;

    private void Update()
    {
        skyboxMaterial.SetVector("_MainLightDirection", transform.forward);
    }
}
