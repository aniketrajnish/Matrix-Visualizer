using UnityEngine;
using UnityEngine.UI;
using MatrixLibrary;
using TMPro;
using System;

public class MatrixGUI : MonoBehaviour
{
    /// <summary>
    /// The class to handle the matrix input from the user.
    /// It converts the input into a Matrix object as well as converts a Matrix object to be displayed as disbaled input fields.
    /// </summary>
    [SerializeField] GameObject inputField, plusR, minusR, plusC, minusC;    
    [HideInInspector] public int rows = 4, columns = 4, maxSize = 9;
    GridLayoutGroup glg;
    private Matrix _matrix, _vector;
    public Matrix matrix
    {
        /// <summary>
        /// Getters and setters for the matrix.
        /// It also updates the UI when the matrix is set.
        /// </summary>
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
    public Matrix vector
    {
        /// <summary>
        /// Getters and setters for a n x 1 matrix.
        /// </summary>
        get
        {
            _vector = MatrixFromInput();
            return _vector;
        }
        set
        {
            InputFromVector(value);
            _vector = value;
        }
    }
    Matrix MatrixFromInput()
    {
        //// <summary>
        /// Creates a matrix from the input fields.
        /// </summary>
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
        //// <summary>
        /// Updates the matrix GUI from a Matrix object.
        /// </summary>
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
                inputFields[i].interactable = false; // to just show the matrix and not allow the user to change it
            }

            plusC.SetActive(false); // to just show the matrix and not allow the user to change it
            minusC.SetActive(false);
            plusR.SetActive(false);
            minusR.SetActive(false);

        }
        catch (ArgumentException ex)
        {
            ErrorGUI.instance.ShowError(ex.Message, 2f);
        }
    }
    void InputFromVector(Matrix value)
    {
        /// <summary>
        /// Creates a n x 1 matrix from the input fields. (just for more clarity)
        /// </summary>
        try
        {
            rows = 1;
            columns = value.data.GetLength(1);

            CreateVectorUI();

            TMP_InputField[] inputFields = GetComponentsInChildren<TMP_InputField>();

            for (int i = 0; i < columns-1; i++)            
                inputFields[i].text = value.data[0, i].ToString();            
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
    void CreateMatrixUI()
    {
        /// <summary>
        /// Creates the matrix UI based on the number of rows and columns.
        /// Used by the setter of the matrix.
        /// </summary>
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);            
        }

        glg.constraintCount = columns;

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)            
                Instantiate(inputField, transform);

        float offset = 40f;

        plusR.transform.localPosition = new Vector3(- (columns + .5f) * (glg.cellSize.x + glg.spacing.x) / 2, offset , plusR.transform.localPosition.z); // lots of hardcoding here
        minusR.transform.localPosition = new Vector3(- (columns + .5f) * (glg.cellSize.x + glg.spacing.x) / 2, -offset, minusR.transform.localPosition.z);
        plusC.transform.localPosition = new Vector3(- offset, (rows + .25f) * (glg.cellSize.y + glg.spacing.y) / 2, plusC.transform.localPosition.z);
        minusC.transform.localPosition = new Vector3(offset, (rows + .25f) * (glg.cellSize.y + glg.spacing.y) / 2, minusC.transform.localPosition.z);
    }
    void CreateVectorUI()
    {
        /// <summary>
        /// Similar to CreateMatrixUI() but for a 4 x 1 matrix. (again just for more clarity)
        /// </summary>
        columns = 4;
        rows = 1;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }

        glg.constraintCount = columns;

        for (int i = 0; i < columns; i++)
        {
            TMP_InputField tempInputField =  Instantiate(inputField, transform).GetComponent<TMP_InputField>();
            if (i == columns - 1)
            {
                tempInputField.text = "1";
                tempInputField.interactable = false;
            }
        }

        plusC.SetActive(false); // not allowing the user to change the vector
        minusC.SetActive(false);
        plusR.SetActive(false);
        minusR.SetActive(false);
    }
    public void AddRow()
    {
        /// <summary>
        /// Subscribes to the + button click event to add a row to the matrix.
        /// </summary>
        if (rows < maxSize)
        {
            rows++;
            CreateMatrixUI();
        }
        else
            ErrorGUI.instance.ShowError("Can't have more than 9 rows!", 2f);
    }
    public void AddColumn()
    {
        /// <summary>
        /// Subscribes to the + button click event to add a column to the matrix.
        /// </summary>
        if (columns < maxSize)
        {
            columns++;
            CreateMatrixUI();
        }
        else
            ErrorGUI.instance.ShowError("Can't have more than 9 columns!", 2f);
    }
    public void RemoveRow()
    {
        /// <summary>
        /// Subscribes to the - button click event to remove a row from the matrix.
        /// </summary>
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
        /// <summary>
        /// Subscribes to the - button click event to remove a column from the matrix.
        /// </summary>
        if (columns > 1)
        {
            columns--;
            CreateMatrixUI();
        }
        else
            ErrorGUI.instance.ShowError("Can't have less than 1 column!", 2f);
    }    
}
