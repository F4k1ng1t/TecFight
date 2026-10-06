using UnityEngine;
using UnityEngine.VFX;

public class SpawnEffect : MonoBehaviour
{
    [SerializeField] private GameObject effectObject;
    private Animator effectAnimator;
    public void SpawnEffectAtPoint(EffectData effect)
    {
        GameObject Temp = Instantiate(effectObject);
        effectAnimator = Temp.GetComponent<Animator>();
        Temp.transform.localScale = Vector3.one * effect.scale;
        if (effect.boundBone != "")
        {
            Temp.transform.SetParent(transform.Find(effect.boundBone));
            Temp.transform.position += effect.offset;
            effectAnimator.Play(effect.effect.name);
            Destroy(Temp, Temp.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            return;
        }
        Temp.transform.position = transform.position + effect.offset;
        if(transform.rotation.y != 0)
        {
            Temp.transform.position -= Vector3.right * effect.offset.x * 2;
        }
        Temp.transform.rotation = transform.rotation;
        effectAnimator.Play(effect.effect.name);
        Destroy(Temp, Temp.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
}
