using UnityEngine;
using MatrixLibrary;

public class ScalingGUI : MonoBehaviour
{
    /// <summary>
    /// Creates and updates a scaling matrix based on the user input.
    /// </summary>
    TMPro.TMP_InputField[] sInputs, mInputs;
    MatrixGUI mGUI;
    private void Start()
    {
        sInputs = transform.GetChild(0).GetComponentsInChildren<TMPro.TMP_InputField>();
        mInputs = transform.GetChild(1).GetComponentsInChildren<TMPro.TMP_InputField>();

        foreach (TMPro.TMP_InputField input in sInputs)
            input.onValueChanged.AddListener((string value) => CreateScalingMatrix()); // update the matrix when the user changes the input

        foreach (TMPro.TMP_InputField input in mInputs) // user can't edit the matrix
            input.interactable = false;

        mGUI = GetComponentInChildren<MatrixGUI>();
        CreateScalingMatrix(); // init scaling matrix
    }
    void CreateScalingMatrix()
    {
        /// <summary>
        /// Creates a scaling matrix based on the user input.
        /// </summary>
        float xVal = string.IsNullOrEmpty(sInputs[0].text) ? 1f : float.Parse(sInputs[0].text);
        float yVal = string.IsNullOrEmpty(sInputs[1].text) ? 1f : float.Parse(sInputs[1].text);
        float zVal = string.IsNullOrEmpty(sInputs[2].text) ? 1f : float.Parse(sInputs[2].text);

        Matrix scalingMatrix = MatrixHelpers.scalingMatrix(new float[] { xVal, yVal, zVal });
        mGUI.matrix = scalingMatrix;

        if (WorldSpaceGUI.instance != null)
            for (int i = 0; i < 3; i++)
                WorldSpaceGUI.instance.sCombInputs[i].text = (string.IsNullOrEmpty(sInputs[i].text) ? 1f : float.Parse(sInputs[i].text)).ToString();

        if (ObjectSpaceGUI.instance != null)
            for (int i = 0; i < 3; i++)
                ObjectSpaceGUI.instance.sCombInputs[i].text = (string.IsNullOrEmpty(sInputs[i].text) ? 1f : float.Parse(sInputs[i].text)).ToString();
    }
}
