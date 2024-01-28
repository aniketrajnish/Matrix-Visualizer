using UnityEngine;
using TMPro;
using MatrixLibrary;

public class ScalingGUI : MonoBehaviour
{
    TMPro.TMP_InputField[] sInputs, mInputs;
    MatrixGUI mGUI;
    private void Start()
    {
        sInputs = transform.GetChild(0).GetComponentsInChildren<TMPro.TMP_InputField>();
        mInputs = transform.GetChild(1).GetComponentsInChildren<TMPro.TMP_InputField>();

        foreach (TMPro.TMP_InputField input in sInputs)
            input.onValueChanged.AddListener((string value) => CreateScalingMatrix());

        foreach (TMPro.TMP_InputField input in mInputs)
            input.interactable = false;

        mGUI = GetComponentInChildren<MatrixGUI>();
        CreateScalingMatrix();
    }
    void CreateScalingMatrix()
    {
        float xVal = string.IsNullOrEmpty(sInputs[0].text) ? 0f : float.Parse(sInputs[0].text);
        float yVal = string.IsNullOrEmpty(sInputs[1].text) ? 0f : float.Parse(sInputs[1].text);
        float zVal = string.IsNullOrEmpty(sInputs[2].text) ? 0f : float.Parse(sInputs[2].text);

        Matrix scalingMatrix = MatrixHelpers.scalingMatrix(new float[] { xVal, yVal, zVal });
        mGUI.matrix = scalingMatrix;
    }
}
