using UnityEngine;
using MatrixLibrary;
using TMPro;
using System.Linq;

public class ObjectSpaceGUI : MonoBehaviour
{
    /// <summary>
    /// Creates and updates all the matrices for the object space transformation (in order: translation, rz,ry,rx, scaling, identity).
    /// </summary>
    public static ObjectSpaceGUI instance;
    private void Awake() => instance = this;
    [SerializeField] TMP_InputField[] sInputs, rInputs, tInputs;
    private void Start()
    {
        /*MatrixGUI identityGUI = transform.GetChild(transform.childCount - 1).GetComponentInChildren<MatrixGUI>();
        identityGUI.matrix = Matrix.identity(4);*/        
        foreach (var input in sInputs.Concat(rInputs).Concat(tInputs))
            input.onValueChanged.AddListener((string value) => UpdateObjectSpaceMatrix());
        UpdateObjectSpaceMatrix();
    }
    public void UpdateObjectSpaceMatrix()
    {
        float[] ss = sInputs.Select(input => string.IsNullOrEmpty(input.text) ? 0f : float.Parse(input.text)).ToArray();
        float[] rs = rInputs.Select(input => string.IsNullOrEmpty(input.text) ? 0f : float.Parse(input.text)).ToArray();
        float[] ts = tInputs.Select(input => string.IsNullOrEmpty(input.text) ? 0f : float.Parse(input.text)).ToArray();

        Matrix ostMatrix = MatrixHelpers.ObjectSpaceTransformationMatrix(ts,ss,rs);
        GetComponentInChildren<MatrixGUI>().matrix = ostMatrix;
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
