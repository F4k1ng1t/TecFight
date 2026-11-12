using UnityEngine;

public class FighterAudio : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [Header("Sounds")]
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip dashSound;
    [SerializeField] AudioClip sheildEnterSound;
    [SerializeField] AudioClip sheildExitSound;
    [SerializeField] AudioClip dodgeSound;

    private void Start()
    {
        if (!audioSource)
            audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayOneShot(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
    }
    public void PlayOneShot(AudioClip audio, float volume)
    {
        audioSource.PlayOneShot(audio, volume);
    }

    public void PlayJumpSound()
    {
        if (jumpSound)
            audioSource.PlayOneShot(jumpSound);
    }
    public void PlayDashSound()
    {
        if (dashSound)
            audioSource.PlayOneShot(dashSound);
    }
    public void PlaySheildEnterSound()
    {
        if (sheildEnterSound)
            audioSource.PlayOneShot(sheildEnterSound);
    }
    public void PlaySheildExitSound()
    {
        if (sheildExitSound)
            audioSource.PlayOneShot(sheildExitSound);
    }
    public void PlayDodgeSound()
    {
        if (dodgeSound)
            audioSource.PlayOneShot(dodgeSound);
    }

}
