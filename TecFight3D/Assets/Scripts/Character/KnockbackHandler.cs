
using UnityEngine;
using UnityEngine.UI;

public class KnockbackHandler : MonoBehaviour
{
    [SerializeField] private float totalHealth = 100f;
    [SerializeField] private float damage = 0f;

    private Image healthSlider;
    private FighterStateMachine fsm;
    private FighterPhysics physics;

    private float lastHitboxTimestamp;

    private void Start()
    {
        fsm = GetComponent<FighterStateMachine>();
        physics = GetComponent<FighterPhysics>();
    }

    public void SetHealthBar(Image healthBar)
    {
        healthSlider = healthBar;

        if (healthSlider != null)
            healthSlider.fillAmount = damage / totalHealth;
    }

    public void OnHit(Vector3 knockback, float deltDamage, float timeStamp, float scaling)
    {
        if (timeStamp <= lastHitboxTimestamp)
            return;

        if (fsm.isDodging)
            return;

        lastHitboxTimestamp = timeStamp;

        if (fsm.isSheilding)
        {
            fsm.sheildHealth -= deltDamage;
            return;
        }

        damage += deltDamage;

        float knockbackStrength = knockback.magnitude + scaling * damage;
        Vector3 knockbackVelocity = knockback.normalized * knockbackStrength;

        fsm.SetState(new HitState());

        physics.AddForce(knockbackVelocity, ForceMode.VelocityChange);

        if (healthSlider != null)
            healthSlider.fillAmount = damage / totalHealth;
    }

    public void ResetDamage()
    {
        damage = 0f;

        if (healthSlider != null)
            healthSlider.fillAmount = damage / totalHealth;
    }
}
