using UnityEngine;
using TMPro;
using MatrixLibrary;
using System.Linq;
using System.Collections.Generic;

public class RotationGUI : MonoBehaviour
{
    [SerializeField] TMPro.TMP_InputField[] rInputs;
    MatrixGUI[] mGUI;
    private void Start()
    {
        rInputs[0].onValueChanged.AddListener((string value) => CreateRotXMatrix());
        rInputs[1].onValueChanged.AddListener((string value) => CreateRotYMatrix());
        rInputs[2].onValueChanged.AddListener((string value) => CreateRotZMatrix());

        var allInputs = GetComponentsInChildren<TMPro.TMP_InputField>().ToList();

        TMP_InputField[] mInputs = allInputs.Except(rInputs).ToArray();

        foreach (TMPro.TMP_InputField input in mInputs)
            input.interactable = false;

        mGUI = GetComponentsInChildren<MatrixGUI>();

        CreateRotXMatrix();
        CreateRotYMatrix();
        CreateRotZMatrix();

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            child.SetParent(transform.parent, false);
        }

        transform.SetParent(transform.parent.parent,false);
    }
    void CreateRotXMatrix()
    {
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
