using UnityEngine;

public class FighterStateMachine : MonoBehaviour
{
    public Rigidbody rig;
    public FighterInput input;

    public float direction = 1f;

    public IFighterState currentState;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        input = GetComponent<FighterInput>();
        SetState(new AirState());
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update();
    }
    void FixedUpdate()
    {
        currentState.FixedUpdate();
    }
    public void SetState(IFighterState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(this);
        Debug.Log(newState.GetType().Name);
    }
    public void SetDirection(bool right)
    {
        if(right)
        {
            direction = 1f;
        }
        else
        {
            direction = -1f;
        }
    }
    public void FlipDirection()
    {
        direction *= -1;
    }
}
