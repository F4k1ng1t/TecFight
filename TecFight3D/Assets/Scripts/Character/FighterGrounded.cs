using UnityEngine;

public class FighterGrounded : MonoBehaviour
{
    public bool isGrounded = false;
    bool wasGrounded = false;
    FighterStateMachine fsm;

    float checkDistance = 1f;
    void Start()
    {
        fsm = GetComponent<FighterStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, checkDistance);
        if (!isGrounded && wasGrounded)
        {
            fsm.SetState(new AirState());
        }
        else if(isGrounded && !wasGrounded)
        {
            fsm.SetState(new GroundedState());
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(
            transform.position,
            transform.position + Vector3.down * checkDistance
        );
    }
}
