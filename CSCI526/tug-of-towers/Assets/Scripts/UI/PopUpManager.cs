using System.Collections;
using UnityEngine;
using TMPro;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] private TMP_Text popupText;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float displayTime = 2f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = popupText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = popupText.gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0;
    }

    public void ShowMessage(string message)
    {
        popupText.text = message;
        StopAllCoroutines();
        canvasGroup.alpha = 1;
        StartCoroutine(FadeOut());
    }

    // Coroutine to fade the message out after displayTime
    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(displayTime);

        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;
    }
}