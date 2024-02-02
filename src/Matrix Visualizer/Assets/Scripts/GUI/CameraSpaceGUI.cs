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
    public static CameraViewSpaceGUI instance;
    private void Awake() => instance = this;
    [SerializeField] TMP_InputField[] rInputs, tInputs;
    private void Start()
    {
        /*MatrixGUI identityGUI = transform.GetChild(transform.childCount - 1).GetComponentInChildren<MatrixGUI>();
        identityGUI.matrix = Matrix.identity(4);*/        
        foreach (var input in rInputs.Concat(tInputs))
            input.onValueChanged.AddListener((string value) => UpdateCameraViewSpaceMatrix());
        UpdateCameraViewSpaceMatrix();
    }
    public void UpdateCameraViewSpaceMatrix()
    {
        float[] rs = rInputs.Select(input => string.IsNullOrEmpty(input.text) ? 0f : float.Parse(input.text)).ToArray();
        float[] ts = tInputs.Select(input => string.IsNullOrEmpty(input.text) ? 0f : float.Parse(input.text)).ToArray();

        Matrix camWSTMatrix = MatrixHelpers.WorldSpaceTransformationMatrix(ts,new float[] { 1, 1, 1 },rs);
        Matrix cvsMatrix = MatrixHelpers.CameraViewSpaceMatrix(camWSTMatrix);
        GetComponentInChildren<MatrixGUI>().matrix = cvsMatrix;
    }
    /*public void WorldSpaceLayout()
    {
        transform.GetChild(3).SetParent(transform.parent, false); // lots of magic numbers here, but it works and gets the GUI in the right order
        transform.GetChild(2).SetParent(transform.parent, false);

        transform.GetChild(4).SetParent(transform.parent, false);
        transform.GetChild(4).SetParent(transform.parent, false);
        transform.GetChild(4).SetParent(transform.parent, false);
        transform.GetChild(4).SetParent(transform.parent, false);
        transform.GetChild(4).SetParent(transform.parent, false);

        transform.GetChild(1).SetParent(transform.parent, false);
        transform.GetChild(0).SetParent(transform.parent, false);
        transform.GetChild(0).SetParent(transform.parent, false);
        transform.GetChild(0).SetParent(transform.parent, false);

        transform.SetParent(transform.parent.parent, false);
    }*/    
}
