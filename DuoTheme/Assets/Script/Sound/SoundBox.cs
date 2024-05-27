using UnityEngine;

public class SoundBox : MonoBehaviour
{
    public SoundManager.SoundName soundName;
    public bool isOnClick;

    private void Start()
    {
        // If not on click, all sound will stop
        // and this sound will play at start (BGM)
        if (!isOnClick)
        {
            if (SoundManager.Instance.GetComponent<AudioSource>() != null)
            {
                SoundManager.Instance.GetComponent<AudioSource>().Stop();
            }
            SoundManager.Instance.Play(soundName);
        }
    }

    public void PlaySoundOnClick()
    {
        SoundManager.Instance.Play(soundName);
    }
}