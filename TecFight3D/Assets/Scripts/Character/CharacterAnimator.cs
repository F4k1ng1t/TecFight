using System.Collections;
using UnityEngine;
using static System.TimeZoneInfo;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;
    private GameObject characterMesh;
    private bool sameState = false;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        characterMesh = gameObject;
    }
    public void PlayAnimation(string animationName)
    {
        if (!animator)
        {
            return;
        }
        Debug.Log("Playing " + animationName);
        animator.CrossFade(animationName, 0);
    }

    private IEnumerator LoopAnimation(string animationName)
    {
        animator.CrossFade(animationName, 0);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        sameState = true;
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            animator.CrossFade(animationName, 0);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    public void SetVisualDirection(int direction)
    {

        if (direction == 1)
        {
            characterMesh.transform.rotation = Quaternion.Euler(0, 180, 0);
            return;
        }
        characterMesh.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void Update()
    {
        //Debug.Log("Playing " + animator.GetCurrentAnimatorStateInfo(0).shortNameHash + " Time: " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    public bool IsPlaying()
    {
        //notes: this doesn't work for the looping animations as they restart from 0 before they ever exceed 1
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f;
    }
}
