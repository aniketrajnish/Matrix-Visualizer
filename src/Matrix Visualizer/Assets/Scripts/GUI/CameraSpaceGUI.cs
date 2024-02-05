using UnityEngine;
using MatrixLibrary;
using TMPro;
using System.Linq;

public class CameraViewSpaceGUI : MonoBehaviour
{
    /// <summary>
    /// Creates and updates all the matrices for the camera space matrix.
    /// No scaling as camera space doesn't have scaling.
    /// </summary>
    [SerializeField] TMP_InputField[] rInputs, tInputs;
    private void Start()
    {        
        foreach (var input in rInputs.Concat(tInputs))
            input.onValueChanged.AddListener((string value) => UpdateCameraViewSpaceMatrix());
        UpdateCameraViewSpaceMatrix(); // init camera view space matrix
    }
    public void UpdateCameraViewSpaceMatrix()
    {
        /// <summary>
        /// Creates a camera view space matrix based on the user input.
        /// </summary>
        float[] rs = rInputs.Select(input => string.IsNullOrEmpty(input.text) ? 0f : float.Parse(input.text)).ToArray();
        float[] ts = tInputs.Select(input => string.IsNullOrEmpty(input.text) ? 0f : float.Parse(input.text)).ToArray();

        Matrix camWSTMatrix = MatrixHelpers.WorldSpaceTransformationMatrix(ts,new float[] { 1, 1, 1 },rs);
        Matrix cvsMatrix = MatrixHelpers.CameraViewSpaceMatrix(camWSTMatrix);
        GetComponentInChildren<MatrixGUI>().matrix = cvsMatrix;
    }
}
