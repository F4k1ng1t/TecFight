using UnityEngine;
using UnityEngine.InputSystem;

public class Gravity : MonoBehaviour
{
    FighterStateMachine f;
    CharacterObject c;
    Vector3 gravityValue;
    public bool active = true;
    
    void Start()
    {
        f = GetComponent<FighterStateMachine>();
        c = f.charObj;
        gravityValue = new Vector3(0, -f.charObj.fallAccel, 0);
    }

    private void FixedUpdate()
    {
        if (active)
        {
            f.rig.AddForce(gravityValue, ForceMode.Acceleration);
        }
        //else
        //{
        //    f.rig.linearVelocity = Vector3.forward * f.rig.linearVelocity.x;
        //}
    }
}
