using UnityEngine;
using System.Collections;

public class SpawnHitbox : MonoBehaviour
{
    [SerializeField] Transform parentBone;
    [SerializeField] int activeFrames = 1;
    [SerializeField] float radius = 0.005f;
    [SerializeField] float height = 0;
    [SerializeField] [Range(0, 360)] float sendAngle = 90;
    [SerializeField] float hitStrength = 0.1f;
    [SerializeField] int priority = 0;
    [SerializeField] LayerMask targetLayer;

    private void OnDrawGizmos()
    {
#if (UNITY_EDITOR) //only draw debug gizmos in the unity editor
        Gizmos.DrawSphere(parentBone.position, radius);
        Gizmos.DrawLine(parentBone.position, parentBone.position + new Vector3(hitStrength * Mathf.Asin(sendAngle), hitStrength * Mathf.Acos(sendAngle)));
#endif
    }

    public void StartSpawn()
    {
        //Okay i was probably thinking of this wrong
        //the hitbox isn't a gameobject, its just a check that pops up sometimes
        //using physics.checkcapsule would probably be best
        //use the parent bone to find the top bottom and angle
        // radius translates directly
        StartCoroutine(Hitbox());
    }

    private IEnumerator Hitbox()
    {
        Debug.Log("Checkpoint 1");
        float startTime = Time.time;
        while(activeFrames/60f > Time.time - startTime)
        {

            Debug.Log("Checkpoint 2");
            Collider[] hitHurtboxes = Physics.OverlapCapsule(parentBone.position, parentBone.rotation.eulerAngles.normalized * height, radius, targetLayer, QueryTriggerInteraction.Collide);
            if (hitHurtboxes.Length > 0)
            {
                //we hit a hurtbox
                Debug.Log("hit " + hitHurtboxes[0].gameObject.name);
                //hitHurtboxes[0].transform.root.GetComponent<FighterStateMachine>(); //put a bitch in hitstun
            }
            yield return (new WaitForEndOfFrame());
        }
    }


}
