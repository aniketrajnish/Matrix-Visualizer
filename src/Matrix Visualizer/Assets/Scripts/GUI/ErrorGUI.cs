using UnityEngine;
using TMPro;
using System.Collections;

public class ErrorGUI : MonoBehaviour
{
    /// <summary>
    /// Created an ErrorGUI class that will be used to show errors in the GUI
    /// </summary>
    public static ErrorGUI instance;
    TextMeshProUGUI errorText;
    private void Awake()
    {
        if (instance != null) // sigleton pattern (as only one instance of this class is needed)
        {
            Destroy(gameObject);
            return;
        }
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
        /// <summary>
        /// Coroutine to show the error for a certain duration (flashing)
        /// </summary>
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
