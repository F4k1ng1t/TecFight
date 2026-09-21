using UnityEngine;
using System.Collections;

public class SpawnHitbox : MonoBehaviour
{
    [SerializeField] Transform parentBone;
    [SerializeField] int activeFrames = 1;
    [SerializeField] float radius = 0.005f;
    [SerializeField] float height = 0;
    [SerializeField] [Range(0, 360)] float sendAngle = 90;
    [SerializeField] float knockback = 0.1f;
    [SerializeField] float damage = 1f;
    [SerializeField] int priority = 0;
    [SerializeField] LayerMask targetLayer;

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(parentBone.position, radius);
        Gizmos.DrawLine(parentBone.position, parentBone.position + new Vector3(knockback * Mathf.Asin(sendAngle), knockback * Mathf.Acos(sendAngle)));
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
                foreach(Collider hurtbox in hitHurtboxes)
                {
                    if(hurtbox.transform.root == transform.root)
                    {
                        continue;
                    }
                    Debug.Log("x: " + Mathf.Sin(Mathf.Deg2Rad * sendAngle));
                    hurtbox.transform.root.gameObject.GetComponent<KnockbackHandler>().OnHit(new Vector3(knockback * Mathf.Sin(Mathf.Deg2Rad * sendAngle), knockback * Mathf.Cos(Mathf.Deg2Rad * sendAngle)), damage);
                    hurtbox.transform.root.gameObject.GetComponent<FighterStateMachine>()?.SetState(new HitState());
                }
                //we hit a hurtbox
                Debug.Log("hit " + hitHurtboxes[0].gameObject.name);
                //hitHurtboxes[0].transform.root.GetComponent<FighterStateMachine>(); //put a bitch in hitstun
            }
            yield return (new WaitForEndOfFrame());
        }
    }


}
