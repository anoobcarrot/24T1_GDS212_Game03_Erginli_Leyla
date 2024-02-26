using UnityEngine;

public class PenguinAnimationController : MonoBehaviour
{
    public AudioSource audioSource;
    public Animator penguinAnimator;

    void Update()
    {
        Debug.Log("Is audio playing: " + audioSource.isPlaying);

        if (audioSource.isPlaying)
        {
            penguinAnimator.SetBool("PlayAnimation", true);
        }
        else
        {
            penguinAnimator.SetBool("PlayAnimation", false);
        }
    }
}

