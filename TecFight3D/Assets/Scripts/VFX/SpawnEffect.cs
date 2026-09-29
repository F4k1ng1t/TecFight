using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    public void SpawnEffectAtPoint(EffectData effect)
    {
        GameObject Temp;
        if (effect.boundBone != null)
        {
            Temp = Instantiate(effect.effect, effect.boundBone);
            Temp.transform.position += effect.offset;
            Destroy(Temp, Temp.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            return;
        }
        Temp = Instantiate(effect.effect);
        Temp.transform.position = transform.position + effect.offset;
        Temp.transform.rotation = transform.rotation;
        Destroy(Temp, Temp.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
}
