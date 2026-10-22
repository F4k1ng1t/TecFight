using UnityEngine;
public class ShieldState : IFighterState
{
    const float sheildHealRate = 5f; //sheild restored per second
    const float sheildDecayRate = 5f;

    FighterStateMachine fsm;
    CharacterObject co;
    FighterInput fi;
    FighterInputProcesser fip;
    Rigidbody rig;

    public void Enter(FighterStateMachine f)
    {
        fsm = f;
        co = f.charObj;
        fi = f.input;
        fip = f.fip;
        rig = f.rig;
        fsm.sheild.SetActive(true);
        UpdateSheildHealth();
        //need to play sheild enter
        fsm.animator?.PlayAnimation("SheildHold", true, fsm.direction);
        fsm.isSheilding = true;
    }
    public void Exit()
    {
        fsm.isSheilding = false;
        fsm.animator?.PlayAnimation("SheildExit", false, fsm.direction);
        //delay for the exit frames
        fsm.sheild.SetActive(false);
        fsm.timeSinceSheild = Time.time;
    }

    public void FixedUpdate()
    {

    }
    void IFighterState.Update()
    {
        //tick down sheild health
        fsm.sheildHealth -= sheildDecayRate * Time.deltaTime;
        fsm.sheild.transform.localScale = GetSheildScale();
        if(fsm.sheildHealth <= 0)
        {
            fsm.sheildHealth = 0;
            //stun
        }
        if (!fi.Shield)
        {
            Exit();
            fsm.SetState(new IdleState());
        }
    }

    private void UpdateSheildHealth()
    {
        Debug.Log("A: " + (Time.time - fsm.timeSinceSheild) + "\nB: " + ((Time.time - fsm.timeSinceSheild) * sheildHealRate));
        fsm.sheildHealth = Mathf.Clamp(fsm.sheildHealth + (Time.time - fsm.timeSinceSheild) * sheildHealRate, 0, fsm.charObj.shieldHealth);
        Debug.Log("sheild health: " + fsm.sheildHealth);
        fsm.sheild.transform.localScale = GetSheildScale();
    }

    //returns a number that approaches 0 the less sheild health the player has and then adjusts it for astetics
    private Vector3 GetSheildScale()
    {
        return (((fsm.sheildHealth / fsm.charObj.shieldHealth) + 0.1f) * 2.5f) * Vector3.one;
    }
}
