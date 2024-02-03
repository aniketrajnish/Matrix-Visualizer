using UnityEngine;
using TMPro;
using MatrixLibrary;
using System.Linq;

public class RotationGUI : MonoBehaviour
{
    /// <summary>
    /// Creates and updates the three rotation matrices based on the user input.
    /// </summary>
    [SerializeField] TMPro.TMP_InputField[] rInputs;
    MatrixGUI[] mGUI;
    public static RotationGUI instance;
    private void Awake() => instance = this;    
    private void Start()
    {
        rInputs[0].onValueChanged.AddListener((string value) => CreateRotXMatrix()); // update the matrix when the user changes the input
        rInputs[1].onValueChanged.AddListener((string value) => CreateRotYMatrix());
        rInputs[2].onValueChanged.AddListener((string value) => CreateRotZMatrix());

        var allInputs = GetComponentsInChildren<TMPro.TMP_InputField>().ToList();

        TMP_InputField[] mInputs = allInputs.Except(rInputs).ToArray();

        foreach (TMPro.TMP_InputField input in mInputs)
            input.interactable = false;

        mGUI = GetComponentsInChildren<MatrixGUI>();

        CreateRotXMatrix(); // init rotation matrices
        CreateRotYMatrix();
        CreateRotZMatrix();

        for (int i = transform.childCount - 1; i >= 0; i--) // unparent to move them within the layout and put the GUI in the right order
        {
            Transform child = transform.GetChild(i);
            child.SetParent(transform.parent, false);
            child.SetSiblingIndex(i);
        }

        transform.SetParent(transform.parent.parent,false);

        /*if (WorldSpaceGUI.instance != null) // in case the rot matrices are a part of the world space matrices, update the layout again to put them in the right order
        {
            print(WorldSpaceGUI.instance.name);
            transform.SetParent(transform.parent.parent, false);
            WorldSpaceGUI.instance.WorldSpaceLayout();            
        }*/
    }
    void CreateRotXMatrix()
    {
        /// <summary>
        /// Creates a rotation matrix around the x-axis based on the user input.
        /// </summary>
        try
        {
            float xVal = string.IsNullOrEmpty(rInputs[0].text) ? 0f : float.Parse(rInputs[0].text);

            Matrix rotXMatrix = MatrixHelpers.rotation3Dx(xVal);
            mGUI[0].matrix = rotXMatrix;
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

            Matrix rotYMatrix = MatrixHelpers.rotation3Dy(yVal);
            mGUI[1].matrix = rotYMatrix;
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

            Matrix rotZMatrix = MatrixHelpers.rotation3Dz(zVal);
            mGUI[2].matrix = rotZMatrix;
        }
        catch (System.Exception ex)
        {
            ErrorGUI.instance.ShowError(ex.Message, 2f);
        }
    }
}