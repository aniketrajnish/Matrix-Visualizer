using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class ErrorGUI : MonoBehaviour
{
    public static ErrorGUI instance;
    TextMeshProUGUI errorText;
    private void Awake()
    {
        instance = this;
        errorText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void ShowError(string error, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ShowErrorCoroutine(error, duration));
    }

    private IEnumerator ShowErrorCoroutine(string error, float duration)
    {
        for (float i = 0; i < 5 * duration; i++)
        {
            errorText.text = error;
            yield return new WaitForSeconds(.1f);
            errorText.text = "";
            yield return new WaitForSeconds(.1f);
        }        
        errorText.text = "";
    }
}
