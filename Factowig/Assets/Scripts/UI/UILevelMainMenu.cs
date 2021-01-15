using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILevelMainMenu : UILevel
{
    private bool corStarted = false;
    public Animator[] buttonAnimators = null;

    public void OnPlayPressed()
    {
        if(!corStarted)
            StartCoroutine(OnPlayPressedCorroutine());       
    }

    public IEnumerator OnPlayPressedCorroutine()
    {
        corStarted = true;

        //Hide UI
        StartCoroutine(FadeOut());

        yield return new WaitUntil(() => fadeEnded == true);

        //settingsButtonAnimator.set

        //Blend cameras
        VCamReferencer.Instance.vcam2.gameObject.SetActive(true);
        VCamReferencer.Instance.vcam1.gameObject.SetActive(false);

        yield return new WaitForSeconds(2);

        //Transition to UILevelSettings
        UIController.Instance.TransitionFromTo(0, 1);

        corStarted = false;
    }

    private IEnumerator OnSettingsClickCorroutine()
    {
        yield return null;
    }
}
