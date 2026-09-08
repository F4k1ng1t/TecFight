using UnityEngine;

public class LStickPosition : MonoBehaviour
{
    public FighterInput f;
    Transform origin;
    public GameObject stick;
    Renderer stickRenderer;
    Color defaultColor;
    private void Start()
    {
        origin = this.transform;
        stick.transform.position = origin.position;
        stickRenderer = stick.GetComponent<Renderer>();
        defaultColor = stickRenderer.material.color;
    }
    private void Update()
    {
        float x = f.MoveInput.x + origin.position.x;
        float y = f.MoveInput.y + origin.position.y;
        stick.transform.position = new Vector3(x, y, origin.position.z);
        if(f.Flick)
        {
            stickRenderer.material.color = Color.red;
            Debug.Log("red");
        }
        else if(stickRenderer.material.color != defaultColor)
        {
            stickRenderer.material.color = defaultColor;
        }
    }
}
