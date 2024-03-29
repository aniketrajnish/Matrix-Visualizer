﻿using UnityEngine;
using TMPro;
using MatrixLibrary;
using System.Collections.Generic;
using UnityEngine.UI;

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
        ostMatrixPrefab, cvsMatrixPrefab, projectionMatrixPrefab;
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
        ResetDisplay(); // clearing the current GUI and its references

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
                WorldSpaceTransformationDisplay();
                break;
            case Operation.OBJECT_SPACE_TRANSFORMATION:
                ObjectSpaceTransformationDisplay();
                break;
            case Operation.CAMERA_VIEW_SPACE:
                CameraViewSpaceDisplay();
                break;
            case Operation.PROJECTION:
                ProjectionDisplay();
                break;
            case Operation.MODEL_VIEW_PROJECTION:
                ModelViewProjectionDisplay();
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
                case Operation.PROJECTION:
                    CalculateAndDisplayProjectionMatrix();
                    break;
                case Operation.MODEL_VIEW_PROJECTION:
                    CalculateAndDisplayModelViewProjectionMatrix();
                    break;
            }
        }
        catch (System.Exception ex)
        {
            ErrorGUI.instance.ShowError(ex.Message, 3f);
        }
    }
   
    void SimpleOperationsDisplay(string operation)
    {
        /// <summary>
        /// To display the GUI for addition and subtraction.
        /// </summary>
        _zeroIdentityHolder.SetActive(true);
        glg.transform.localPosition = new Vector3(-740, 0, 0); 
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        CreateOperationDisplay(operation);
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
    }
    void SingleOperationDisplay(string operation)
    {
        /// <summary>
        /// To display the GUI for transpose and inverse.
        /// </summary>
        _zeroIdentityHolder.SetActive(true);
        glg.transform.localPosition = new Vector3(-440, 0, 0);
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        CreateOperationDisplay(operation);
    }
    void TransformationDisplay(GameObject transromationHolder)
    {
        /// <summary>
        /// To display the GUI for all the transformation operations.
        /// </summary>
        glg.cellSize = new Vector2(400, 400); // only 4x1 and 4x4 matrices so we can reduce the cell size
        glg.transform.localPosition = new Vector3(-540, 0, 0);
        _zeroIdentityHolder.SetActive(false); // no need for the zero and identity matrices
        matrices.Add(Instantiate(transromationHolder, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        CreateOperationDisplay("*");
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        matrices[1].vector = new Matrix(new float[4, 1]);
    }
    /// <summary>
    /// The functions below are used to create the GUI for the respective operations.
    /// </summary>
    void AdditionDisplay() => SimpleOperationsDisplay("+");
    void SubtractionDisplay() => SimpleOperationsDisplay("-");
    void ScalarMultiplicationDisplay()
    {
        _zeroIdentityHolder.SetActive(true);
        glg.transform.localPosition = new Vector3(-740, 0, 0);
        _scalarHolder = Instantiate(scalarPrefab, calcGUIParent);
        CreateOperationDisplay("*");
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
    }
    void MatrixMultiplicationDisplay() => SimpleOperationsDisplay("*");
    void EqualityCheckDisplay() => SimpleOperationsDisplay("");    
    void TransposeDisplay() => SingleOperationDisplay("T");
    void InverseDisplay() => SingleOperationDisplay("I");          
    void TranslationDisplay() => TransformationDisplay(translationMatrixPrefab);
    void ScalingDisplay() => TransformationDisplay(scalingMatrixPrefab);     
    void RotationDisplay() => TransformationDisplay(rotationMatrixPrefab);
    void WorldSpaceTransformationDisplay() => TransformationDisplay(wstMatrixPrefab);
    void ObjectSpaceTransformationDisplay() => TransformationDisplay(ostMatrixPrefab);
    void CameraViewSpaceDisplay() => TransformationDisplay(cvsMatrixPrefab);
    void ProjectionDisplay() => TransformationDisplay(projectionMatrixPrefab);
    void ModelViewProjectionDisplay()
    {
        glg.cellSize = new Vector2(500, 500);
        glg.transform.localPosition = new Vector3(-760, 0, 0);
        _zeroIdentityHolder.SetActive(false);
        matrices.Add(Instantiate(projectionMatrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        CreateOperationDisplay("*");
        matrices.Add(Instantiate(cvsMatrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        CreateOperationDisplay("*");
        matrices.Add(Instantiate(wstMatrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        CreateOperationDisplay("*");
        matrices.Add(Instantiate(ostMatrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        CreateOperationDisplay("*");
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        matrices[matrices.Count - 1].vector = new Matrix(new float[4, 1]);
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
    void ResetDisplay()
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

        if (RotationGUI.instance != null) // assign a new instace to the current rotation, world space and object space GUIs
        {
            RotationGUI.instance.gameObject.SetActive(false);
            Destroy(RotationGUI.instance.gameObject);
        }
        if (WorldSpaceGUI.instance != null)
        {
            WorldSpaceGUI.instance.gameObject.SetActive(false);
            Destroy(WorldSpaceGUI.instance.gameObject);
        }
        if (ObjectSpaceGUI.instance != null)
        {
            ObjectSpaceGUI.instance.gameObject.SetActive(false);
            Destroy(ObjectSpaceGUI.instance.gameObject);
        }
    }      
    void CalculateAndDisplayOperation(Matrix result)
    {
        /// <summary>
        /// The parent function to calculate the result of the operation and display it.
        /// </summary>
        if (_resultMatrixHolder == null) // if the result matrix holder is not created yet, create it and an equals sign
        {
            CreateOperationDisplay("=");
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        else // else destroy the previous result matrix and create a new one
        {
            Destroy(_resultMatrixHolder);
            _resultMatrixHolder = Instantiate(matrixPrefab, calcGUIParent);
        }
        _resultMatrixHolder.GetComponentInChildren<MatrixGUI>().matrix = result;
    }
    /// <summary>
    /// The functions below are used to calculate the result of the respective operations.
    /// </summary>
    void CalculateAndDisplayAddition() => CalculateAndDisplayOperation(matrices[0].matrix + matrices[1].matrix);    
    void CalculateAndDisplaySubtraction() => CalculateAndDisplayOperation(matrices[0].matrix - matrices[1].matrix);    
    void CalculateAndScalarMatrixMultiplication()
    {
        string sInput = _scalarHolder.GetComponentInChildren<TMP_InputField>().text;
        float scalarValue = string.IsNullOrEmpty(sInput) ? 0f : float.Parse(sInput);

        Matrix product = scalarValue * matrices[0].matrix;

        CalculateAndDisplayOperation(product);
    }
    void CalculateAndDisplayMatrixMultiplication() => CalculateAndDisplayOperation(matrices[0].matrix * matrices[1].matrix);    
    void CalculateAndDisplayTranspose() => CalculateAndDisplayOperation(!matrices[0].matrix);    
    void CalculateAndDisplayInverse() => CalculateAndDisplayOperation(~matrices[0].matrix);    
    void CalculateAndDisplayEquality()
    {
        if (matrices[0].matrix == matrices[1].matrix)
            _operationDisplayHolder.GetComponentInChildren<TextMeshProUGUI>().text = "=";        
        else
            _operationDisplayHolder.GetComponentInChildren<TextMeshProUGUI>().text = "≠";
    }
    void CalculateAndDisplayTranslation() => CalculateAndDisplayOperation(matrices[0].matrix * matrices[1].matrix);    
    void CalculateAndDisplayScaling() => CalculateAndDisplayOperation(matrices[0].matrix * matrices[1].matrix);   
    void CalculateAndDisplayRotation() => CalculateAndDisplayOperation(matrices[0].matrix * matrices[1].matrix);    
    void CalculateAndDisplayWorldSpaceTransformation() => CalculateAndDisplayOperation(matrices[0].matrix * matrices[1].matrix);    
    void CalculateAndDisplayObjectSpaceTransformation() => CalculateAndDisplayOperation(matrices[0].matrix * matrices[1].matrix);    
    void CalculateAndDisplayCameraViewSpaceMatrix() => CalculateAndDisplayOperation(matrices[0].matrix * matrices[1].matrix);
    void CalculateAndDisplayProjectionMatrix() => CalculateAndDisplayOperation(matrices[0].matrix * matrices[1].matrix);    
    void CalculateAndDisplayModelViewProjectionMatrix() => CalculateAndDisplayOperation(matrices[0].matrix * matrices[1].matrix * matrices[2].matrix * matrices[3].matrix * matrices[4].matrix);    
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
