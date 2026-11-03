using UnityEngine;
using System.Collections;
using Unity.Mathematics;
using static UnityEngine.Rendering.DebugUI;

public class HitboxSpawner : MonoBehaviour
{
    [SerializeField] Transform[] parentBones;
    [SerializeField] LayerMask targetLayer;
    
    private FighterStateMachine fsm;
    private bool drawGizmo = false;
    private Hitbox activeHitbox;

    private void Start()
    {
        fsm = transform.root.gameObject.GetComponent<FighterStateMachine>();
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmo)
        {
            return;
        }
        drawGizmo = false;
        DebugDrawCapsule(
            parentBones[activeHitbox.bindingIndex].position + (Vector3.up * activeHitbox.offset.y) + (Vector3.right * activeHitbox.offset.x * fsm.direction * -1),
            parentBones[activeHitbox.bindingIndex].position + (Vector3.up * activeHitbox.offset.y) + (Vector3.right * activeHitbox.offset.x * fsm.direction * -1) + parentBones[activeHitbox.bindingIndex].rotation * Vector3.up * activeHitbox.height,
            activeHitbox.radius,
            parentBones[activeHitbox.bindingIndex].rotation);
    }

    public void SpawnHitbox(Hitbox hitbox)
    {
        StartCoroutine(Hitbox(hitbox));
    }

    private IEnumerator Hitbox(Hitbox hitbox)
    {
        //Debug.Log("Checkpoint 1");
        float startTime = Time.time;
        activeHitbox = hitbox;
        while(hitbox.activeFrames/60f > Time.time - startTime)
        {
            drawGizmo = true;
            //Debug.Log("Checkpoint 2");
            Collider[] hitHurtboxes = Physics.OverlapCapsule(
                parentBones[hitbox.bindingIndex].position + (Vector3.up * hitbox.offset.y) + (Vector3.right * hitbox.offset.x * fsm.direction * -1),
                parentBones[hitbox.bindingIndex].position + (Vector3.up * hitbox.offset.y) + (Vector3.right * hitbox.offset.x * fsm.direction * -1) + parentBones[activeHitbox.bindingIndex].rotation * Vector3.up * activeHitbox.height, 
                hitbox.radius, 
                targetLayer, 
                QueryTriggerInteraction.Collide);
            
            if (hitHurtboxes.Length > 0)
            {
                foreach(Collider hurtbox in hitHurtboxes)
                {
                    if(hurtbox.transform.root == transform.root)
                    {
                        continue;
                    }
                    Debug.Log("x: " + Mathf.Sin(Mathf.Deg2Rad * hitbox.sendAngle));
                    FighterStateMachine player = gameObject.GetComponentInParent<FighterStateMachine>();
                    hurtbox.transform.root.gameObject.GetComponent<KnockbackHandler>().OnHit(new Vector3(
                        hitbox.knockback * Mathf.Sin(Mathf.Deg2Rad * hitbox.sendAngle) * player.direction, 
                        hitbox.knockback * Mathf.Cos(Mathf.Deg2Rad * hitbox.sendAngle)),
                        hitbox.damage,
                        startTime, hitbox.scaling);
                    FighterStateMachine enemy = hurtbox.transform.root.gameObject.GetComponent<FighterStateMachine>();
                    if (enemy != null)
                    {
                    }
                    break;
                }
                //we hit a hurtbox
                Debug.Log("hit " + hitHurtboxes[0].gameObject.name);
                //hitHurtboxes[0].transform.root.GetComponent<FighterStateMachine>(); //put a bitch in hitstun
            }
            yield return (new WaitForEndOfFrame());
        }
    }

    private void DebugDrawCapsule(Vector3 point1, Vector3 point2, float radius, Quaternion rotation)
    {
        Debug.Log("draw");
        Gizmos.DrawWireSphere(point1, radius);
        Gizmos.DrawWireSphere(point2, radius);
        Gizmos.DrawLine(point1 + (rotation * Vector3.right), point2 + (rotation * Vector3.right));
        Gizmos.DrawLine(point1 - (rotation * Vector3.right), point2 - (rotation * Vector3.right));
        Gizmos.DrawLine(point1 + (rotation * Vector3.up), point2 + (rotation * Vector3.up));
        Gizmos.DrawLine(point1 - (rotation * Vector3.up), point2 - (rotation * Vector3.up));
    }
}
