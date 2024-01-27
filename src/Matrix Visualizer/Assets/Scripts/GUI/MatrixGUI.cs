using UnityEngine;
using UnityEngine.UI;
using MatrixLibrary;
using TMPro;
using System;

public class MatrixGUI : MonoBehaviour
{
    [SerializeField] GameObject inputField, plusR, minusR, plusC, minusC;    
    [HideInInspector] public int rows = 4, columns = 4, maxSize = 9;
    GridLayoutGroup glg;
    private Matrix _matrix;
    public Matrix matrix
    {
        get
        {
            _matrix = MatrixFromInput();
            return _matrix;
        }
        set
        {
            InputFromMatrix(value);
            _matrix = value;
        }
    }
    Matrix MatrixFromInput()
    {
        TMP_InputField[] inputFields = GetComponentsInChildren<TMP_InputField>();
        float[,] data = new float[rows, columns];

        try
        {     
            for (int i = 0; i < inputFields.Length; i++)
            {
                int row = i / columns;
                int column = i % columns;
                data[row, column] = string.IsNullOrEmpty(inputFields[i].text) ? 0f : float.Parse(inputFields[i].text);
            }
        }
        catch (ArgumentException ex)
        {
            ErrorGUI.instance.ShowError(ex.Message, 2f);            
        }

        return new Matrix(data);
    }
    void InputFromMatrix(Matrix value)
    {
        try
        {
            rows = value.data.GetLength(0);
            columns = value.data.GetLength(1);

            CreateMatrixUI();

            TMP_InputField[] inputFields = GetComponentsInChildren<TMP_InputField>();

            for (int i = 0; i < rows * columns; i++)
            {
                int row = i / columns;
                int column = i % columns;

                inputFields[i].text = value.data[row, column].ToString();
                inputFields[i].interactable = false;
            }

            plusC.SetActive(false);
            minusC.SetActive(false);
            plusR.SetActive(false);
            minusR.SetActive(false);

        }
        catch (ArgumentException ex)
        {
            ErrorGUI.instance.ShowError(ex.Message, 2f);
        }
    }
    void Awake()
    {
        glg = GetComponent<GridLayoutGroup>();

        CreateMatrixUI();
    }
    private void Update()
    {
        print(rows + " " + columns);
    }
    void CreateMatrixUI()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);            
        }

        glg = GetComponent<GridLayoutGroup>();

        glg.constraintCount = columns;

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)            
                Instantiate(inputField, transform);

        float offset = 40f;

        plusR.transform.localPosition = new Vector3(- (columns + .5f) * (glg.cellSize.x + glg.spacing.x) / 2, offset , plusR.transform.localPosition.z);
        minusR.transform.localPosition = new Vector3(- (columns + .5f) * (glg.cellSize.x + glg.spacing.x) / 2, -offset, minusR.transform.localPosition.z);
        plusC.transform.localPosition = new Vector3(- offset, (rows + .25f) * (glg.cellSize.y + glg.spacing.y) / 2, plusC.transform.localPosition.z);
        minusC.transform.localPosition = new Vector3(offset, (rows + .25f) * (glg.cellSize.y + glg.spacing.y) / 2, minusC.transform.localPosition.z);
    }

    public void AddRow()
    {
        if (rows <= maxSize)
        {
            rows++;
            CreateMatrixUI();
        }
        else
            ErrorGUI.instance.ShowError("Can't have more than 9 rows!", 2f);
    }
    public void AddColumn()
    {
        if (columns <= maxSize)
        {
            columns++;
            CreateMatrixUI();
        }
        else
            ErrorGUI.instance.ShowError("Can't have more than 9 columns!", 2f);
    }
    public void RemoveRow()
    {
        if (rows > 1)
        {
            rows--;
            CreateMatrixUI();
        }
        else
            ErrorGUI.instance.ShowError("Can't have less than 1 row!", 2f);
    }
    public void RemoveColumn()
    {
        if (columns > 1)
        {
            columns--;
            CreateMatrixUI();
        }
        else
            ErrorGUI.instance.ShowError("Can't have less than 1 column!", 2f);
    }    
}
