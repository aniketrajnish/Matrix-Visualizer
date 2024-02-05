using UnityEngine;
using MatrixLibrary;
using TMPro;
using System.Linq;

public class ObjectSpaceGUI : MonoBehaviour
{
    /// <summary>
    /// Redefinition of the WorldSpaceGUI class, but to handle the object space transformation.
    /// </summary>
    public static ObjectSpaceGUI instance;
    [SerializeField] TextMeshProUGUI expandBtnText;
    private void Awake() => instance = this;
    [SerializeField] public TMP_InputField[] sCombInputs, rCombInputs, tCombInputs, sInputs, rInputs, tInputs;
    [SerializeField] GameObject[] operationHolders;
    [SerializeField] GameObject combMatrix, sMatrix, rMatrix, tMatrix;
    bool isExpanded = false;
    private void Start()
    {
        foreach (var input in sCombInputs.Concat(rCombInputs).Concat(tCombInputs))
            input.onValueChanged.AddListener((string value) => UpdateObjectSpaceMatrix());

        UpdateObjectSpaceMatrix();
        UpdateVisibility();
    }
    public void UpdateObjectSpaceMatrix()
    {
        float[] ss = sCombInputs.Select(input => string.IsNullOrEmpty(input.text) ? 1f : float.Parse(input.text)).ToArray();
        float[] rs = rCombInputs.Select(input => string.IsNullOrEmpty(input.text) ? 0f : float.Parse(input.text)).ToArray();
        float[] ts = tCombInputs.Select(input => string.IsNullOrEmpty(input.text) ? 0f : float.Parse(input.text)).ToArray();

        for (int i = 0; i < 3; i++)
        {
            sInputs[i].text = ss[i].ToString();
            rInputs[i].text = rs[i].ToString();
            tInputs[i].text = ts[i].ToString();
        }

        Matrix wstMatrix = MatrixHelpers.ObjectSpaceTransformationMatrix(ts, ss, rs);
        combMatrix.GetComponentInChildren<MatrixGUI>().matrix = wstMatrix;
    }
    public void UpdateVisibility()
    {
        combMatrix.SetActive(!isExpanded);

        foreach (GameObject go in operationHolders)
            go.SetActive(isExpanded);

        sMatrix.SetActive(isExpanded);
        rMatrix.SetActive(isExpanded);
        tMatrix.SetActive(isExpanded);

        if (isExpanded)
        {
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
            sMatrix.transform.SetParent(transform, false);
            operationHolders[0].transform.SetParent(transform, false);
            rMatrix.transform.SetParent(transform, false);
            operationHolders[1].transform.SetParent(transform, false);

            expandBtnText.text = "Expand";
        }

        isExpanded = !isExpanded;
    }
}
