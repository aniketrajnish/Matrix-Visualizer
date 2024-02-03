using UnityEngine;
using TMPro;
using MatrixLibrary;
using System.Linq;

public class RotationGUI : MonoBehaviour
{
    /// <summary>
    /// Creates and updates the three rotation matrices based on the user input.
    /// </summary>
    [SerializeField] TMP_InputField[] rCombInputs, rInputs;
    [SerializeField] TextMeshProUGUI expandBtnText;
    [SerializeField] GameObject combRotMatrix;
    [SerializeField] GameObject[] rotMatrices, operationHolders;
    MatrixGUI combMGUI;
    MatrixGUI[] mGUIs;
    public static RotationGUI instance;
    bool isExpanded = false;
    private void Awake() => instance = this;    
    private void Start()
    {
        rInputs[0].onValueChanged.AddListener((string value) => CreateRotXMatrix()); // update the matrix when the user changes the input
        rInputs[1].onValueChanged.AddListener((string value) => CreateRotYMatrix());
        rInputs[2].onValueChanged.AddListener((string value) => CreateRotZMatrix());

        foreach (var input in rCombInputs)
            input.onValueChanged.AddListener((string value) => CreateRotMatrix());

        var allInputs = GetComponentsInChildren<TMPro.TMP_InputField>().ToList();

        TMP_InputField[] mInputs = allInputs.Except(rInputs.Concat(rCombInputs)).ToArray();

        foreach (TMPro.TMP_InputField input in mInputs)
            input.interactable = false;

        combMGUI = combRotMatrix.GetComponentInChildren<MatrixGUI>();
        mGUIs = rotMatrices.Select(m => m.GetComponentInChildren<MatrixGUI>()).ToArray();

        CreateRotMatrix();

        CreateRotXMatrix();
        CreateRotYMatrix();
        CreateRotZMatrix();

        UpdateVisibiliy();
        /*for (int i = transform.childCount - 1; i >= 0; i--) // unparent to move them within the layout and put the GUI in the right order
        {
            Transform child = transform.GetChild(i);
            child.SetParent(transform.parent, false);
            child.SetSiblingIndex(i);
        }

        transform.SetParent(transform.parent.parent,false);*/

        /*if (WorldSpaceGUI.instance != null) // in case the rot matrices are a part of the world space matrices, update the layout again to put them in the right order
        {
            print(WorldSpaceGUI.instance.name);
            transform.SetParent(transform.parent.parent, false);
            WorldSpaceGUI.instance.WorldSpaceLayout();            
        }*/
    }
    void CreateRotMatrix()
    {
        /// <summary>
        /// Create Rotation Matrix based on the user input.
        /// </summary>
        try
        {
            float xVal = string.IsNullOrEmpty(rCombInputs[0].text) ? 0f : float.Parse(rCombInputs[0].text);
            float yVal = string.IsNullOrEmpty(rCombInputs[1].text) ? 0f : float.Parse(rCombInputs[1].text);
            float zVal = string.IsNullOrEmpty(rCombInputs[2].text) ? 0f : float.Parse(rCombInputs[2].text);

            rInputs[0].text = xVal.ToString();
            rInputs[1].text = yVal.ToString();
            rInputs[2].text = zVal.ToString();

            Matrix rotMatrix = MatrixHelpers.rotation3D(xVal, yVal, zVal);

            combMGUI.matrix = rotMatrix;
            mGUIs[0].matrix = MatrixHelpers.rotation3Dx(xVal);
            mGUIs[1].matrix = MatrixHelpers.rotation3Dy(yVal);
            mGUIs[2].matrix = MatrixHelpers.rotation3Dz(zVal);
        }
        catch (System.Exception ex)
        {
            //ErrorGUI.instance.ShowError(ex.Message, 2f);
        }
    }
    public void UpdateVisibiliy()
    {        
        combRotMatrix.SetActive(!isExpanded);

        foreach (var go in rotMatrices.Concat(operationHolders))
            go.SetActive(isExpanded);

        if (isExpanded)
        {
            rotMatrices[1].transform.SetParent(transform.parent, false);
            rotMatrices[1].transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            operationHolders[0].transform.SetParent(transform.parent, false);
            operationHolders[0].transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            rotMatrices[2].transform.SetParent(transform.parent, false);
            rotMatrices[2].transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            operationHolders[1].transform.SetParent(transform.parent, false);
            operationHolders[1].transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);

            expandBtnText.text = "Collapse";
        }
        else
        {
            rotMatrices[1].transform.SetParent(transform, false);
            rotMatrices[2].transform.SetParent(transform, false);
            operationHolders[0].transform.SetParent(transform, false);
            operationHolders[1].transform.SetParent(transform, false);

            expandBtnText.text = "Expand";
        }

        isExpanded = !isExpanded;
    }
    void CreateRotXMatrix()
    {
        /// <summary>
        /// Creates a rotation matrix around the x-axis based on the user input.
        /// </summary>
        try
        {
            float xVal = string.IsNullOrEmpty(rInputs[0].text) ? 0f : float.Parse(rInputs[0].text);
            rCombInputs[0].text = xVal.ToString();

            Matrix rotXMatrix = MatrixHelpers.rotation3Dx(xVal);

            mGUIs[0].matrix = rotXMatrix;
            combMGUI.matrix = mGUIs[0].matrix * mGUIs[1].matrix * mGUIs[2].matrix;
        }
        catch (System.Exception ex)
        {
            ErrorGUI.instance.ShowError(ex.Message, 2f);
        }
    }
    void CreateRotYMatrix()
    {
        /// <summary>
        /// Creates a rotation matrix around the y-axis based on the user input.
        /// </summary>
        try
        {
            float yVal = string.IsNullOrEmpty(rInputs[1].text) ? 0f : float.Parse(rInputs[1].text);
            rCombInputs[1].text = yVal.ToString();

            Matrix rotYMatrix = MatrixHelpers.rotation3Dy(yVal);

            mGUIs[1].matrix = rotYMatrix;
            combMGUI.matrix = mGUIs[0].matrix * mGUIs[1].matrix * mGUIs[2].matrix;
        }
        catch (System.Exception ex)
        {
            ErrorGUI.instance.ShowError(ex.Message, 2f);
        }
    }
    void CreateRotZMatrix()
    {
        /// <summary>
        /// Creates a rotation matrix around the z-axis based on the user input.
        /// </summary>
        try
        {
            float zVal = string.IsNullOrEmpty(rInputs[2].text) ? 0f : float.Parse(rInputs[2].text);
            rCombInputs[2].text = zVal.ToString();

            Matrix rotZMatrix = MatrixHelpers.rotation3Dz(zVal);

            mGUIs[2].matrix = rotZMatrix;
            combMGUI.matrix = mGUIs[0].matrix * mGUIs[1].matrix * mGUIs[2].matrix;
        }
        catch (System.Exception ex)
        {
            ErrorGUI.instance.ShowError(ex.Message, 2f);
        }
    }
}
