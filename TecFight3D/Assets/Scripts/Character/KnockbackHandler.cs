using UnityEngine;
using UnityEngine.UI;

public class KnockbackHandler : MonoBehaviour
{
    [SerializeField] private float totalHealth = 100;
    [SerializeField] private float damage = 0;
    private Image healthSlider;
    private float currentHealth;

    Rigidbody rb;
    FighterStateMachine fsm;
    private void Start()
    {
        fsm = GetComponent<FighterStateMachine>();
        rb = gameObject.GetComponent<Rigidbody>();

    }

    public void SetHealthBar(Image healthBar)
    {
        healthSlider = healthBar;
        healthSlider.fillAmount = damage / totalHealth;
    }

    public void OnHit(Vector3 knockback, float deltDamage)
    {
        if (!rb)
        {
            Debug.LogError("Rigidbody not found on KnockbackHandler");
            return;
        }
        damage += deltDamage;
        rb.AddForce(knockback * damage, ForceMode.Impulse);
        healthSlider.fillAmount = damage / totalHealth;
    }


    void TakeDamage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;

            if (currentHealth < 0)
            {
                currentHealth = 0;
            }


            Debug.Log("Player Health: " + currentHealth);

            if (currentHealth == 0)
            {
                Die();
            }
        }
    }


    void Die()
    {
        Debug.Log("Player has died!");
    }
}
