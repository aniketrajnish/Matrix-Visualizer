using UnityEngine;
using TMPro;
using MatrixLibrary;

public class TranslationGUI : MonoBehaviour
{
    TMPro.TMP_InputField[] tInputs, mInputs;
    MatrixGUI mGUI;
    private void Start()
    {
        tInputs = transform.GetChild(0).GetComponentsInChildren<TMPro.TMP_InputField>();
        mInputs = transform.GetChild(1).GetComponentsInChildren<TMPro.TMP_InputField>();

        foreach (TMPro.TMP_InputField input in tInputs)
            input.onValueChanged.AddListener((string value) => CreateTranlationMatrix());

        foreach (TMPro.TMP_InputField input in mInputs)
            input.interactable = false;

        mGUI = GetComponentInChildren<MatrixGUI>();
        CreateTranlationMatrix();
    }
    void CreateTranlationMatrix()
    {
        float xVal = string.IsNullOrEmpty(tInputs[0].text) ? 0f : float.Parse(tInputs[0].text);
        float yVal = string.IsNullOrEmpty(tInputs[1].text) ? 0f : float.Parse(tInputs[1].text);
        float zVal = string.IsNullOrEmpty(tInputs[2].text) ? 0f : float.Parse(tInputs[2].text);

        Matrix translationMatrix = MatrixHelpers.translationMatrix(new float[] { xVal, yVal, zVal });
        mGUI.matrix = translationMatrix;
    }
}
