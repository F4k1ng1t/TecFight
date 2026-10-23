using UnityEngine;
using System.Collections;

public class HitboxSpawner : MonoBehaviour
{
    [SerializeField] Transform[] parentBones;
    [SerializeField] LayerMask targetLayer;

    public void SpawnHitbox(Hitbox hitbox)
    {
        StartCoroutine(Hitbox(hitbox));
    }

    private IEnumerator Hitbox(Hitbox hitbox)
    {
        Debug.Log("Checkpoint 1");
        float startTime = Time.time;
        while(hitbox.activeFrames/60f > Time.time - startTime)
        {

            Debug.Log("Checkpoint 2");
            Collider[] hitHurtboxes = Physics.OverlapCapsule(
                parentBones[hitbox.bindingIndex].position + hitbox.offset, 
                parentBones[hitbox.bindingIndex].rotation.eulerAngles.normalized * hitbox.height, 
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
                        hitbox.damage);
                    FighterStateMachine enemy = hurtbox.transform.root.gameObject.GetComponent<FighterStateMachine>();
                    if (enemy != null)
                    {
                        enemy.SetDirection(-player.direction);
                        enemy.SetState(new HitState());
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


}
