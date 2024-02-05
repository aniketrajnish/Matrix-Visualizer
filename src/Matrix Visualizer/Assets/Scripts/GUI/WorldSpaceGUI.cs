using UnityEngine;
using MatrixLibrary;
using TMPro;
using System.Linq;

public class WorldSpaceGUI : MonoBehaviour
{
    /// <summary>
    /// Creates and updates all the matrices for the world space transformation (in order: scaling, rx, ry, rz, translation, identity).
    /// </summary>
    public static WorldSpaceGUI instance;
    [SerializeField] TextMeshProUGUI expandBtnText;
    private void Awake() => instance = this;
    [SerializeField] public TMP_InputField[] sCombInputs, rCombInputs, tCombInputs, sInputs, rInputs, tInputs;
    [SerializeField] GameObject[] operationHolders;
    [SerializeField] GameObject combMatrix, sMatrix, rMatrix, tMatrix, iMatrix;
    bool isExpanded = false;
    private void Start()
    {
        foreach (var input in sCombInputs.Concat(rCombInputs).Concat(tCombInputs))
            input.onValueChanged.AddListener((string value) => UpdateWorldSpaceMatrix());

        iMatrix.GetComponentInChildren<MatrixGUI>().matrix = Matrix.identity(4);

        UpdateWorldSpaceMatrix(); // init world space matrix
        UpdateVisibility(); // init collapsed matrix
    }
    public void UpdateWorldSpaceMatrix()
    {
        /// <summary>
        /// Creates and updates all the matrices for the world space transformation (in order: translation, rz, ry, rx, scaling, identity).
        /// </summary>
        float[] ss = sCombInputs.Select(input => string.IsNullOrEmpty(input.text) ? 1f : float.Parse(input.text)).ToArray();
        float[] rs = rCombInputs.Select(input => string.IsNullOrEmpty(input.text) ? 0f : float.Parse(input.text)).ToArray();
        float[] ts = tCombInputs.Select(input => string.IsNullOrEmpty(input.text) ? 0f : float.Parse(input.text)).ToArray();

        for (int i = 0; i < 3; i++)
        {
            sInputs[i].text = ss[i].ToString(); // update the individual scaling, rotation, and translation fields
            rInputs[i].text = rs[i].ToString(); // automatically updates the indivdual matrices as there are listeners on the input fields
            tInputs[i].text = ts[i].ToString();
        }

        Matrix wstMatrix = MatrixHelpers.WorldSpaceTransformationMatrix(ts, ss, rs);
        combMatrix.GetComponentInChildren<MatrixGUI>().matrix = wstMatrix;
    }
    public void UpdateVisibility()
    {
        /// <summary>
        /// Toggles between the expanded and collapsed view of the world space transformation matrices.
        /// </summary>
        combMatrix.SetActive(!isExpanded);

        foreach (GameObject go in operationHolders)
            go.SetActive(isExpanded);

        sMatrix.SetActive(isExpanded);
        rMatrix.SetActive(isExpanded);
        tMatrix.SetActive(isExpanded);
        iMatrix.SetActive(isExpanded);

        if (isExpanded)
        {
            iMatrix.transform.SetParent(transform.parent, false); // in oreder of multiplication
            iMatrix.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            operationHolders[2].transform.SetParent(transform.parent, false);
            operationHolders[2].transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            sMatrix.transform.SetParent(transform.parent, false);
            sMatrix.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            operationHolders[0].transform.SetParent(transform.parent, false);
            operationHolders[0].transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            rMatrix.transform.SetParent(transform.parent, false);
            rMatrix.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            operationHolders[1].transform.SetParent(transform.parent, false);
            operationHolders[1].transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);

            expandBtnText.text = "Collapse";
        }
        else
        {
            iMatrix.transform.SetParent(transform, false);
            operationHolders[2].transform.SetParent(transform, false);
            sMatrix.transform.SetParent(transform, false);
            operationHolders[0].transform.SetParent(transform, false);
            rMatrix.transform.SetParent(transform, false);
            operationHolders[1].transform.SetParent(transform, false);

            expandBtnText.text = "Expand";
        }

        isExpanded = !isExpanded;
    }
}
