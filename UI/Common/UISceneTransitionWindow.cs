using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UISceneTransitionWindow : MonoBehaviour
{
    public Image background;
    public UIScriptedAnimations loadingText;
    public void StartTransition()
    {
        StartCoroutine(AtStart());
    }

    public void EndTransition()
    {
        StartCoroutine(AtEnd());
    }

    IEnumerator AtStart()
    {
        StartCoroutine(ChangeBGAlpha(255f, loadingText.animationTime));
        yield return new WaitForSeconds(loadingText.animationTime * 0.15f);
        loadingText.Move();
    }

    IEnumerator AtEnd()
    {
        yield return new WaitForSeconds(1f);
        loadingText.MoveBack();
        yield return new WaitForSeconds(loadingText.animationTime * 0.15f);
        StartCoroutine(ChangeBGAlpha(0f, loadingText.animationTime));
    }

    IEnumerator ChangeBGAlpha(float alpha, float bgFadeAnimationTime)
    {   
        float timeElapsed = 0f;

        while(timeElapsed < bgFadeAnimationTime)
        {
            timeElapsed += Time.deltaTime;
            Color tempColor = background.color;
            float currentAlpha = Mathf.Lerp(tempColor.a, alpha, timeElapsed / bgFadeAnimationTime);
            tempColor.a = alpha > 0 ? currentAlpha / 255f : currentAlpha;
            background.color = tempColor;
            yield return null;
        }

        background.color = new Color(background.color.r, background.color.g, 
            background.color.b, alpha > 0 ? alpha / 255f : alpha);
    }
}
