using UnityEngine;
using MatrixLibrary;

public class TranslationGUI : MonoBehaviour
{
    /// <summary>
    /// Creates and updates a translation matrix based on the user input.
    /// </summary>
    TMPro.TMP_InputField[] tInputs, mInputs;
    MatrixGUI mGUI;
    private void Start()
    {
        tInputs = transform.GetChild(0).GetComponentsInChildren<TMPro.TMP_InputField>();
        mInputs = transform.GetChild(1).GetComponentsInChildren<TMPro.TMP_InputField>();

        foreach (TMPro.TMP_InputField input in tInputs)
            input.onValueChanged.AddListener((string value) => CreateTranlationMatrix()); // update the matrix when the user changes the input

        foreach (TMPro.TMP_InputField input in mInputs) // user can't edit the matrix
            input.interactable = false;

        mGUI = GetComponentInChildren<MatrixGUI>();
        CreateTranlationMatrix(); // init translation matrix
    }
    void CreateTranlationMatrix()
    {
        /// <summary>
        /// Creates a translation matrix based on the user input.
        /// </summary>
        float xVal = string.IsNullOrEmpty(tInputs[0].text) ? 0f : float.Parse(tInputs[0].text);
        float yVal = string.IsNullOrEmpty(tInputs[1].text) ? 0f : float.Parse(tInputs[1].text);
        float zVal = string.IsNullOrEmpty(tInputs[2].text) ? 0f : float.Parse(tInputs[2].text);

        Matrix translationMatrix = MatrixHelpers.translationMatrix(new float[] { xVal, yVal, zVal });
        mGUI.matrix = translationMatrix;

        if (WorldSpaceGUI.instance != null) // update the translation fields of combined world space transformation matrix, if it exists       
            for (int i = 0; i < 3; i++)
                WorldSpaceGUI.instance.tCombInputs[i].text = (string.IsNullOrEmpty(tInputs[i].text) ? 0f : float.Parse(tInputs[i].text)).ToString();

        if (ObjectSpaceGUI.instance != null) // update the translation fields of combined object space transformation matrix, if it exists
            for (int i = 0; i < 3; i++)
                ObjectSpaceGUI.instance.tCombInputs[i].text = (string.IsNullOrEmpty(tInputs[i].text) ? 0f : float.Parse(tInputs[i].text)).ToString();
    }
}
