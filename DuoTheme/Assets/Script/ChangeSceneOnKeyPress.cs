using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class ChangeSceneOnKeyPress : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        SoundManager.Instance.Play(SoundManager.SoundName.RushIn);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SoundManager.Instance.Play(SoundManager.SoundName.ChangeScene);
            animator.SetTrigger("ChangeScene");
        }
    }

    // Change scene method is used in 'Animation'
    private void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}