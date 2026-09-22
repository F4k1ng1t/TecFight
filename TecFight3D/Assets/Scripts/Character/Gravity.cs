using UnityEngine;
using UnityEngine.InputSystem;

public class Gravity : MonoBehaviour
{
    FighterStateMachine f;
    CharacterObject c;
    Vector3 gravityValue;
    
    void Start()
    {
        f = GetComponent<FighterStateMachine>();
        c = f.charObj;
        gravityValue = new Vector3(0, -f.charObj.fallAccel, 0);
    }

    private void FixedUpdate()
    {
        f.rig.AddForce(gravityValue, ForceMode.Acceleration);
        Debug.Log(f.rig.linearVelocity.y);
        f.rig.linearVelocity = new Vector3(f.rig.linearVelocity.x, Mathf.Max(f.rig.linearVelocity.y, -c.maxfallSpeed));
    }
}
