using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    public int sceneIndex = 0;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        //Hacked, animation starts playing on Play
        LoadSceneOnAnimationEnd();
    }

    public void LoadSceneOnAnimationEnd()
    {
        StartCoroutine(LoadSceneOnAnimationEndCr());
    }

    private IEnumerator LoadSceneOnAnimationEndCr()
    {
        yield return new WaitUntil(() => { return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 ? true : false; });
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneIndex);
    }
}
