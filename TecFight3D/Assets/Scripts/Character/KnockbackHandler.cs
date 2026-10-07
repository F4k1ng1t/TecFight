using UnityEngine;

public class KnockbackHandler : MonoBehaviour
{
    Rigidbody rb;
    FighterStateMachine fsm;
    float damage = 1f;
    private void Start()
    {
        fsm = GetComponent<FighterStateMachine>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void OnHit(Vector3 knockback, float deltDamage)
    {
        if (!rb)
        {
            Debug.LogError("Rigidbody not found on KnockbackHandler");
            return;
        }
        rb.AddForce(knockback * damage, ForceMode.Impulse);
        damage += deltDamage;
        Debug.Log(damage);
    }
}
