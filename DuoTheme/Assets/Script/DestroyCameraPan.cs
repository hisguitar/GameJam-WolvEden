using UnityEngine;

public class DestroyCameraPan : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // Call this function in the last frame of animation
    public void RemoveAnimatorAndScript()
    {
        if (animator != null)
        {
            Destroy(animator);
        }

        // Remove this script from GameObject that attached it
        Destroy(this);
    }
}