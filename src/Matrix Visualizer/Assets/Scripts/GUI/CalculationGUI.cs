﻿using UnityEngine;
using TMPro;
using MatrixLibrary;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Linq;
using System.Security.Cryptography;

public class CalculationGUI : MonoBehaviour
{
    /// <summary>
    /// The heart of the calculator GUI.
    /// It creates the GUI based on the operation selected.
    /// It then also calculates the result and displays it.
    /// </summary>
    [SerializeField] OperationGUI opGUI;
    [SerializeField] GameObject scalarPrefab, matrixPrefab, operationDisplayPrefab, 
        translationMatrixPrefab, scalingMatrixPrefab, rotationMatrixPrefab, wstMatrixPrefab,
        ostMatrixPrefab, cvsMatrixPrefab;
    [SerializeField] GameObject _zeroMatrixHolder, _identityMatrixHolder, _zeroIdentityHolder;
    GameObject _scalarHolder, _resultMatrixHolder, _operationDisplayHolder; 
    Transform calcGUIParent;
    GridLayoutGroup glg;
    List<MatrixGUI> matrices = new List<MatrixGUI>();    
    private void Awake()
    {
        calcGUIParent = transform.GetChild(0);
        glg = calcGUIParent.GetComponent<GridLayoutGroup>();
        UpdateDisplay();
    }
    public void UpdateDisplay()
    {
        /// <summary>
        /// Creates the GUI based on the operation selected.
        /// </summary>
        ClearDisplay(); // clearing the current GUI and its references

        switch (opGUI.currentOp)
        {
            case Operation.ADDITION:
                AdditionDisplay();
                break;
            case Operation.SUBTRACTION:
                SubtractionDisplay();
                break;
            case Operation.SCALAR_MULTIPLICATION:
                ScalarMultiplicationDisplay();
                break;
            case Operation.MATRIX_MULTIPLICATION:
                MatrixMultiplicationDisplay();
                break;
            case Operation.TRANSPOSE:
                TransposeDisplay();
                break;
            case Operation.INVERSE:
                InverseDisplay();
                break;
            case Operation.EQALITY_CHECK:
                EqualityCheckDisplay();
                break;
            case Operation.TRANSLATION:
                TranslationDisplay();
                break;
            case Operation.SCALING:
                ScalingDisplay();
                break;
            case Operation.ROTATION:
                RotationDisplay();
                break;
            case Operation.WORLD_SPACE_TRANSFORMATION:
                WorldSpaceTranformationDisplay();
                break;
            case Operation.OBJECT_SPACE_TRANSFORMATION:
                ObjectSpaceTransformationDisplay();
                break;
            case Operation.CAMERA_VIEW_SPACE:
                CameraViewSpaceMatrixDisplay();
                break;
        }
    }
    public void CalculateAndDisplay()
    {
        /// <summary>
        /// Listens to the on click event of the calculate button.
        /// Displays the result of the operation.
        /// </summary>
        try
        {
            switch (opGUI.currentOp)
            {
                case Operation.ADDITION:
                    CalculateAndDisplayAddition();
                    break;
                case Operation.SUBTRACTION:
                    CalculateAndDisplaySubtraction();
                    break;
                case Operation.SCALAR_MULTIPLICATION:
                    CalculateAndScalarMatrixMultiplication();
                    break;
                case Operation.MATRIX_MULTIPLICATION:
                    CalculateAndDisplayMatrixMultiplication();
                    break;
                case Operation.TRANSPOSE:
                    CalculateAndDisplayTranspose();
                    break;
                case Operation.INVERSE:
                    CalculateAndDisplayInverse();
                    break;
                case Operation.EQALITY_CHECK:
                    CalculateAndDisplayEquality();
                    break; 
                case Operation.TRANSLATION:
                    CalculateAndDisplayTranslation();
                    break;
                case Operation.SCALING:
                    CalculateAndDisplayScaling();
                    break;
                case Operation.ROTATION:
                    CalculateAndDisplayRotation();
                    break;
                case Operation.WORLD_SPACE_TRANSFORMATION:
                    CalculateAndDisplayWorldSpaceTransformation();
                    break;
                case Operation.OBJECT_SPACE_TRANSFORMATION:
                    CalculateAndDisplayObjectSpaceTransformation();
                    break;
                case Operation.CAMERA_VIEW_SPACE:
                    CalculateAndDisplayCameraViewSpaceMatrix();
                    break;
            }
        }
        catch (System.Exception ex)
        {
            ErrorGUI.instance.ShowError(ex.Message, 3f);
        }
    }
    /// <summary>
    /// The functions below are used to create the GUI for the respective operations.
    /// </summary>
    void AdditionDisplay()
    {
        _zeroIdentityHolder.SetActive(true);
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        CreateOperationDisplay("+");
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
    }
    void SubtractionDisplay()
    {
        _zeroIdentityHolder.SetActive(true);
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        CreateOperationDisplay("-");
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
    }
    void ScalarMultiplicationDisplay()
    {
        _zeroIdentityHolder.SetActive(true);
        _scalarHolder = Instantiate(scalarPrefab, calcGUIParent);
        CreateOperationDisplay("*");
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());        
    }
    void MatrixMultiplicationDisplay()
    {
        _zeroIdentityHolder.SetActive(true);
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        CreateOperationDisplay("*");
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
    }
    void TransposeDisplay()
    {   
        _zeroIdentityHolder.SetActive(true);
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        CreateOperationDisplay("T");
    }
    void InverseDisplay()
    {
        _zeroIdentityHolder.SetActive(true);
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        CreateOperationDisplay("I");
    }
    void EqualityCheckDisplay()
    {  
        _zeroIdentityHolder.SetActive(true);
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        CreateOperationDisplay("");
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
    }
    void TranslationDisplay()
    {
        glg.cellSize = new Vector2(400, 400); // only 4x1 and 4x4 matrices so we can reduce the cell size
        _zeroIdentityHolder.SetActive(false); // no need for the zero and identity matrices        
        matrices.Add(Instantiate(translationMatrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        CreateOperationDisplay("*"); 
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        matrices[1].vector = new Matrix(new float[4, 1]);
    }
    void ScalingDisplay()
    {
        glg.cellSize = new Vector2(400, 400);
        _zeroIdentityHolder.SetActive(false);
        matrices.Add(Instantiate(scalingMatrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        CreateOperationDisplay("*");
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        matrices[1].vector = new Matrix(new float[4, 1]);        
    }
    void RotationDisplay()
    {
        glg.cellSize = new Vector2(400, 400);
        _zeroIdentityHolder.SetActive(false);
        
        Instantiate(rotationMatrixPrefab, calcGUIParent);
        CreateOperationDisplay("*");
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        matrices[0].vector = new Matrix(new float[4, 1]);
        matrices.Clear();
        matrices = GetComponentsInChildren<MatrixGUI>().ToList(); // since more than one matrix in the prefab
        /*foreach (MatrixGUI mGUI in matrices)
            print(mGUI.transform.parent.parent.name);*/
    }
    void WorldSpaceTranformationDisplay()
    {
        glg.cellSize = new Vector2(400, 400);
        _zeroIdentityHolder.SetActive(false);  
        Instantiate(wstMatrixPrefab, calcGUIParent);
        CreateOperationDisplay("*");
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        matrices[0].vector = new Matrix(new float[4, 1]);
        matrices.Clear();
        matrices = GetComponentsInChildren<MatrixGUI>().ToList();
        /*foreach (MatrixGUI mGUI in matrices)
            print(mGUI.transform.parent.parent.name);*/
    }
    void ObjectSpaceTransformationDisplay()
    {
        glg.cellSize = new Vector2(400, 400);
        _zeroIdentityHolder.SetActive(false);
        Instantiate(ostMatrixPrefab, calcGUIParent);
        CreateOperationDisplay("*");
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        matrices[0].vector = new Matrix(new float[4, 1]);
        matrices.Clear();
        matrices = GetComponentsInChildren<MatrixGUI>().ToList();
    }
    void CameraViewSpaceMatrixDisplay()
    {
        glg.cellSize = new Vector2(400, 400);
        _zeroIdentityHolder.SetActive(false);
        Instantiate(cvsMatrixPrefab, calcGUIParent);
        CreateOperationDisplay("*");
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        matrices[0].vector = new Matrix(new float[4, 1]);
        matrices.Clear();
        matrices = GetComponentsInChildren<MatrixGUI>().ToList();
    }
    void CreateOperationDisplay(string operation)
    {
        /// <summary>
        /// Creates a text component to show the user what operation is being performed.
        /// </summary>
        _operationDisplayHolder = Instantiate(operationDisplayPrefab, calcGUIParent);
        TextMeshProUGUI operationDisplayText = _operationDisplayHolder.GetComponentInChildren<TextMeshProUGUI>();
        operationDisplayText.text = operation;
    }    
    void ClearDisplay()
    {
        /// <summary>
        /// Clears the current GUI and its references.
        /// </summary>
        foreach (Transform child in calcGUIParent)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }

        matrices.Clear();
        _resultMatrixHolder = null;
        _operationDisplayHolder = null;

        if (RotationGUI.instance != null)
        {
            RotationGUI.instance.gameObject.SetActive(false);
            Destroy(RotationGUI.instance.gameObject);
        }
        if (WorldSpaceGUI.instance != null)
        {
            WorldSpaceGUI.instance.gameObject.SetActive(false);
            Destroy(WorldSpaceGUI.instance.gameObject);
        }
    }   
    /// <summary>
    /// The functions below are used to calculate the result of the respective operations.
    /// </summary>
    void CalculateAndDisplayAddition()
    {
        Matrix sum = matrices[0].matrix + matrices[1].matrix;

        if (_resultMatrixHolder == null) // spawn the operation display and matrix if no calculation has been done yet
        {
            CreateOperationDisplay("=");
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent); 
        }
        else // if a calculation has been done, destry old result and spawn new one
        {
            Destroy(_resultMatrixHolder);
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        _resultMatrixHolder.GetComponentInChildren<MatrixGUI>().matrix = sum;
    }
    void CalculateAndDisplaySubtraction()
    {
        Matrix difference = matrices[0].matrix - matrices[1].matrix;

        if (_resultMatrixHolder == null)
        {
            CreateOperationDisplay("=");
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent); 
        }
        else
        {
            Destroy(_resultMatrixHolder);
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        _resultMatrixHolder.GetComponentInChildren<MatrixGUI>().matrix = difference;
    }
    void CalculateAndScalarMatrixMultiplication()
    {
        string sInput = _scalarHolder.GetComponentInChildren<TMP_InputField>().text;
        float scalarValue = string.IsNullOrEmpty(sInput) ? 0f : float.Parse(sInput);

        Matrix product = scalarValue * matrices[0].matrix;

        if (_resultMatrixHolder == null)
        {
            CreateOperationDisplay("=");
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent); 
        }
        else
        {
            Destroy(_resultMatrixHolder);
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        _resultMatrixHolder.GetComponentInChildren<MatrixGUI>().matrix = product;
    }
    void CalculateAndDisplayMatrixMultiplication()
    {
        Matrix product = matrices[0].matrix * matrices[1].matrix;

        if (_resultMatrixHolder == null)
        {
            CreateOperationDisplay("=");
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        else
        {
            Destroy(_resultMatrixHolder);
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        _resultMatrixHolder.GetComponentInChildren<MatrixGUI>().matrix = product;
    }
    void CalculateAndDisplayTranspose()
    {
        Matrix transpose = !matrices[0].matrix;

        if (_resultMatrixHolder == null)
        {
            CreateOperationDisplay("=");
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent); 
        }
        else
        {
            Destroy(_resultMatrixHolder);
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        _resultMatrixHolder.GetComponentInChildren<MatrixGUI>().matrix = transpose;
    }
    void CalculateAndDisplayInverse()
    {
        Matrix inverse = ~matrices[0].matrix;

        if (_resultMatrixHolder == null)
        {
            CreateOperationDisplay("=");
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        else
        {
            Destroy(_resultMatrixHolder);
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        _resultMatrixHolder.GetComponentInChildren<MatrixGUI>().matrix = inverse;
    }
    void CalculateAndDisplayEquality()
    {
        if (matrices[0].matrix == matrices[1].matrix)
            _operationDisplayHolder.GetComponentInChildren<TextMeshProUGUI>().text = "=";        
        else
            _operationDisplayHolder.GetComponentInChildren<TextMeshProUGUI>().text = "≠";
    }
    void CalculateAndDisplayTranslation()
    {
        Matrix translationMatrix = matrices[0].matrix * matrices[1].matrix;

        if (_resultMatrixHolder == null)
        {
            CreateOperationDisplay("=");
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        else
        {
            Destroy(_resultMatrixHolder);
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        _resultMatrixHolder.GetComponentInChildren<MatrixGUI>().matrix = translationMatrix;
    }
    void CalculateAndDisplayScaling()
    {
        Matrix scalingMatrix = matrices[0].matrix * matrices[1].matrix;

        if (_resultMatrixHolder == null)
        {
            CreateOperationDisplay("=");
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        else
        {
            Destroy(_resultMatrixHolder);
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        _resultMatrixHolder.GetComponentInChildren<MatrixGUI>().matrix = scalingMatrix;
    }
    void CalculateAndDisplayRotation()
    {
        Matrix rotationMatrix = matrices[0].matrix * matrices[1].matrix * matrices[2].matrix; // Rx * Ry * Rz  
        rotationMatrix = rotationMatrix * matrices[3].matrix; // Rx * Ry * Rz  * P

        if (_resultMatrixHolder == null)
        {
            CreateOperationDisplay("=");
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        else
        {
            Destroy(_resultMatrixHolder);
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        _resultMatrixHolder.GetComponentInChildren<MatrixGUI>().matrix = rotationMatrix;
    }
    void CalculateAndDisplayWorldSpaceTransformation()
    {        
        Matrix wstMatrix = matrices[0].matrix * matrices[1].matrix; // S * Rx * Ry * Rz * T * I * P

        if (_resultMatrixHolder == null)
        {
            CreateOperationDisplay("=");
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        else
        {
            Destroy(_resultMatrixHolder);
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        _resultMatrixHolder.GetComponentInChildren<MatrixGUI>().matrix = wstMatrix;
    }
    void CalculateAndDisplayObjectSpaceTransformation()
    {
        Matrix ostMatrix = matrices[0].matrix * matrices[1].matrix; // T * Rz * Ry * Rx * S * I * P

        if (_resultMatrixHolder == null)
        {
            CreateOperationDisplay("=");
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        else
        {
            Destroy(_resultMatrixHolder);
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        _resultMatrixHolder.GetComponentInChildren<MatrixGUI>().matrix = ostMatrix;
    }
    void CalculateAndDisplayCameraViewSpaceMatrix()
    {
        Matrix cvsMatrix = matrices[0].matrix * matrices[1].matrix; 

        if (_resultMatrixHolder == null)
        {
            CreateOperationDisplay("=");
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        else
        {
            Destroy(_resultMatrixHolder);
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        _resultMatrixHolder.GetComponentInChildren<MatrixGUI>().matrix = cvsMatrix;
    }
    public void CreateZeroIdentityMatrix()
    {
        /// <summary>
        /// Creates a zero matrix, an identity matrix or both based on the user input.
        /// </summary>
        try
        {
            TMP_InputField[] inputFieldsZero = _zeroMatrixHolder.GetComponentsInChildren<TMPro.TMP_InputField>();
            TMP_InputField inputFieldIdentity = _identityMatrixHolder.GetComponentInChildren<TMPro.TMP_InputField>();

            int.TryParse(inputFieldsZero[0].text, out int zeroRows);
            int.TryParse(inputFieldsZero[1].text, out int zeroColumns);
            int.TryParse(inputFieldIdentity.text, out int identitySize);

            if (zeroRows < 0 || zeroColumns < 0 || identitySize < 0)
                ErrorGUI.instance.ShowError("Matrix dimensions should be positive integers!", 2f);
            else if ((zeroRows == 0 || zeroColumns == 0) && identitySize == 0)
                ErrorGUI.instance.ShowError("Enter values for dimensions!", 2f);
            if (zeroRows > 0 && zeroColumns > 0 && identitySize == 0)
            {
                Matrix zeroMatrix = Matrix.zero(zeroRows, zeroColumns);
                matrices[0].matrix = zeroMatrix;
            }
            else if (zeroRows == 0 && zeroColumns == 0 && identitySize > 0)
            {
                Matrix identityMatrix = Matrix.identity(identitySize);
                matrices[0].matrix = identityMatrix;
            }
            else if (zeroRows > 0 && zeroColumns > 0 && identitySize > 0)
            {
                if (matrices.Count >= 2)
                {
                    Matrix zeroMatrix = Matrix.zero(zeroRows, zeroColumns);
                    Matrix identityMatrix = Matrix.identity(identitySize);
                    matrices[0].matrix = zeroMatrix;
                    matrices[1].matrix = identityMatrix;
                }
                else
                    ErrorGUI.instance.ShowError("Only one matrix available!", 2f);
            }
        }
        catch (System.Exception ex)
        {
            ErrorGUI.instance.ShowError(ex.Message, 2f);
        }
    }

}
