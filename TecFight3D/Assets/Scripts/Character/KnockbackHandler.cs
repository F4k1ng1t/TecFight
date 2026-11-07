
using UnityEngine;
using UnityEngine.UI;

public class KnockbackHandler : MonoBehaviour
{
    const float knockbackDecay = 0.9f;
    [SerializeField] private float totalHealth = 100;
    [SerializeField] private float damage = 0;
    [SerializeField] AudioClip hitSound;
    private Image healthSlider;
    private float currentHealth;

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
            //mayhaps add a sheild hit sound
        }
        //fsm.SetDirection(-player.direction); //sets the direction opposite the hitting players direction
        fsm.SetState(new HitState());
        fsm.fighterAudio.PlayOneShot(hitSound);
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
