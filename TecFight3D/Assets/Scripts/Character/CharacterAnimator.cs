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
    public void PlayAnimation(string animationName, bool looping, float transitionTime = 0f)
    {
        if (!animator)
        {
            return;
        }
        sameState = false;
        //Debug.Log("Playing " + animationName);
        if (!looping)
        {
            animator.CrossFade(animationName, transitionTime);
            return;
        }
        StartCoroutine(LoopAnimation(animationName));
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

    public bool IsPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length >
         animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}
