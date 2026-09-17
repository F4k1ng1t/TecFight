using UnityEngine;

public class SpawnHitbox : MonoBehaviour
{
    [SerializeField] Transform parentBone;
    [SerializeField] int activeFrames = 1;
    [SerializeField] float radius = 0.005f;
    [SerializeField] float height = 0;
    [SerializeField] [Range(0, 360)] float sendAngle = 90;
    [SerializeField] int priority = 0;
    [SerializeField] LayerMask targetLayer;

    public void Spawn()
    {
        //Okay i was probably thinking of this wrong
        //the hitbox isn't a gameobject, its just a check that pops up sometimes
        //using physics.checkcapsule would probably be best
        //use the parent bone to find the top bottom and angle
        // radius translates directly
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == targetLayer.value)
        {
            //deal damage
            //need some way to reference the character controller, most obvious is to put a script on each hurtbox that has some reference to the character
        }
    }
}
