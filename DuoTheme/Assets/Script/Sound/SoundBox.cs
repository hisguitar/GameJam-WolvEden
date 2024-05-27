using UnityEngine;

public class SoundBox : MonoBehaviour
{
    public SoundManager.SoundName soundName;
    public bool isOnClick;

    private void Start()
    {
        // If not on click, the sound will play at start (BGM)
        if (!isOnClick)
        {
            SoundManager.Instance.Play(soundName);
        }
    }

    public void PlaySound()
    {
        SoundManager.Instance.Play(soundName);
    }
}