using UnityEngine;
using TMPro;
using MatrixLibrary;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Linq;

public class CalculationGUI : MonoBehaviour
{
    [SerializeField] OperationGUI opGUI;
    [SerializeField] GameObject scalarPrefab, matrixPrefab, operationDisplayPrefab, translationMatrixPrefab, scalingMatrixPrefab, rotationMatrixPrefab, wstMatrixPrefab;
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
        ClearDisplay();

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
            case Operation.EQALITY_CHECK:
                EqualityCheckDisplay();
                break;
            case Operation.TRANSLATION:
                CreateTranslationDisplay();
                break;
            case Operation.SCALING:
                CreateScalingDisplay();
                break;
            case Operation.ROTATION:
                CreateRotationDisplay();
                break;
            case Operation.WORLD_SPACE_TRANSFORMATION:
                CreateWorldSpaceTranformationDisplay();
                break;
        }
    }
    public void CalculateAndDisplay()
    {
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
            }
        }
        catch (System.Exception ex)
        {
            ErrorGUI.instance.ShowError(ex.Message, 3f);
        }
    }
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
    void EqualityCheckDisplay()
    {  
        _zeroIdentityHolder.SetActive(true);
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        CreateOperationDisplay("");
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
    }
    void CreateTranslationDisplay()
    {
        glg.cellSize = new Vector2(400, 400);
        _zeroIdentityHolder.SetActive(false);
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        matrices[0].vector = new Matrix(new float[1, 4]);
        CreateOperationDisplay("*");
        matrices.Add(Instantiate(translationMatrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        CreateOperationDisplay("T");
    }
    void CreateScalingDisplay()
    {
        glg.cellSize = new Vector2(400, 400);
        _zeroIdentityHolder.SetActive(false);
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        matrices[0].vector = new Matrix(new float[1, 4]);
        CreateOperationDisplay("*");
        matrices.Add(Instantiate(scalingMatrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
    }
    void CreateRotationDisplay()
    {
        glg.cellSize = new Vector2(400, 400);
        _zeroIdentityHolder.SetActive(false);
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        matrices[0].vector = new Matrix(new float[1, 4]);
        CreateOperationDisplay("*");
        Instantiate(rotationMatrixPrefab, calcGUIParent);
        matrices.Clear();
        matrices = GetComponentsInChildren<MatrixGUI>().ToList();
        /*foreach (MatrixGUI mGUI in matrices)
            print(mGUI.transform.parent.parent.name);*/
    }
    void CreateWorldSpaceTranformationDisplay()
    {
        glg.cellSize = new Vector2(400, 400);
        _zeroIdentityHolder.SetActive(false);
        matrices.Add(Instantiate(matrixPrefab, calcGUIParent).GetComponentInChildren<MatrixGUI>());
        matrices[0].vector = new Matrix(new float[1, 4]);
        CreateOperationDisplay("*");
        Instantiate(wstMatrixPrefab, calcGUIParent);
        matrices.Clear();
        matrices = GetComponentsInChildren<MatrixGUI>().ToList();
    }
    void CreateOperationDisplay(string operation)
    {
        _operationDisplayHolder = Instantiate(operationDisplayPrefab, calcGUIParent);
        TextMeshProUGUI operationDisplayText = _operationDisplayHolder.GetComponentInChildren<TextMeshProUGUI>();
        operationDisplayText.text = operation;
    }    
    void ClearDisplay()
    {
        foreach (Transform child in calcGUIParent)            
            Destroy(child.gameObject);

        matrices.Clear();
        _resultMatrixHolder = null;
    }   
    void CalculateAndDisplayAddition()
    {
        Matrix sum = matrices[0].matrix + matrices[1].matrix;

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
        float scalarValue = float.Parse(_scalarHolder.GetComponentInChildren<TMP_InputField>().text);
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
        Matrix transpose = ~matrices[0].matrix;

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
    void CalculateAndDisplayEquality()
    {
        if (matrices[0].matrix == matrices[1].matrix)
            _operationDisplayHolder.GetComponentInChildren<TextMeshProUGUI>().text = "=";        
        else
            _operationDisplayHolder.GetComponentInChildren<TextMeshProUGUI>().text = "≠";
    }
    void CalculateAndDisplayTranslation()
    {
        Matrix translationMatrix = matrices[0].matrix * ~matrices[1].matrix;

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
        Matrix rotationMatrix = matrices[5].matrix * matrices[4].matrix * matrices[3].matrix; // Rz * Ry * Rx  
        rotationMatrix = matrices[2].matrix * rotationMatrix; // P * Rz * Ry * Rx
        print(matrices[2].matrix + " " + matrices[5].matrix + " " + matrices[4].matrix + " " + matrices[3].matrix);

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
        Matrix wstMatrix = ~matrices[7].matrix * matrices[6].matrix * matrices[5].matrix * matrices[4].matrix * matrices[3].matrix * matrices[8].matrix; // T * Rz * Ry * Rx * S * I
        wstMatrix = matrices[2].matrix * wstMatrix; // P * T * Rz * Ry * Rx * S * I

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
    public void CreateZeroIdentityMatrix()
    {
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
