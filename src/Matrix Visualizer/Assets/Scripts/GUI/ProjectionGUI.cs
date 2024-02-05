using UnityEngine;
using MatrixLibrary;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class ProjectionGUI : MonoBehaviour
{
    /// <summary>    
    /// Creates Perspective/Orthographic Projection Matrix GUI based on user choice.
    /// </summary>
    [SerializeField] GameObject perspectiveInput, orthographicInput;
    [SerializeField] TMP_InputField[] perspectiveInputs, orthographicInputs;
    ToggleGroup tg;
    private void Start()
    {       
        perspectiveInputs = perspectiveInput.GetComponentsInChildren<TMP_InputField>();
        orthographicInputs = orthographicInput.GetComponentsInChildren<TMP_InputField>();

        foreach (var input in perspectiveInputs)
            input.onValueChanged.AddListener((string value) => UpdatePerspectiveMatrix());

        foreach (var input in orthographicInputs)
            input.onValueChanged.AddListener((string value) => UpdateOrthographicMatrix());

        tg = GetComponentInChildren<ToggleGroup>();

        foreach (Toggle toggle in tg.GetComponentsInChildren<Toggle>())
            toggle.onValueChanged.AddListener((bool value) => UpdateProjectionInputs());

        UpdateProjectionInputs(); // init perspective matrix
    }
    public void UpdateProjectionInputs()
    {
        /// <summary>
        /// Change the matrix and GUI to perspective or orthographic based on user choice.
        /// </summary>
        perspectiveInput.SetActive(false);
        orthographicInput.SetActive(false);

        if (tg.ActiveToggles().First().name == "Perpective Toogle")
        {
            perspectiveInput.SetActive(true);
            UpdatePerspectiveMatrix(); // re-calculate perspective matrix
        }
        else
        {
            orthographicInput.SetActive(true);
            UpdateOrthographicMatrix(); // re-calculate orthographic matrix
        }
    }
    public void UpdatePerspectiveMatrix()
    {
        /// <summary>
        /// Creates a perspective matrix based on the user input.
        /// </summary>
        float fov = string.IsNullOrEmpty(perspectiveInputs[0].text) ? 0f : float.Parse(perspectiveInputs[0].text);
        float aspect = string.IsNullOrEmpty(perspectiveInputs[1].text) ? 0f : float.Parse(perspectiveInputs[1].text);
        float near = string.IsNullOrEmpty(perspectiveInputs[2].text) ? 0f : float.Parse(perspectiveInputs[2].text);
        float far = string.IsNullOrEmpty(perspectiveInputs[3].text) ? 0f : float.Parse(perspectiveInputs[3].text);

        Matrix perspectiveMatrix = MatrixHelpers.PerspectiveProjectionMatrix(fov, aspect, near, far);
        GetComponentInChildren<MatrixGUI>().matrix = perspectiveMatrix;
    }
    public void UpdateOrthographicMatrix()
    {
        /// <summary>
        /// Creates an orthographic matrix based on the user input.
        /// </summary>
        float left = string.IsNullOrEmpty(orthographicInputs[0].text) ? 0f : float.Parse(orthographicInputs[0].text);
        float top = string.IsNullOrEmpty(orthographicInputs[1].text) ? 0f : float.Parse(orthographicInputs[1].text);
        float near = string.IsNullOrEmpty(orthographicInputs[2].text) ? 0f : float.Parse(orthographicInputs[2].text);
        float far = string.IsNullOrEmpty(orthographicInputs[3].text) ? 0f : float.Parse(orthographicInputs[3].text);
        float bottom = string.IsNullOrEmpty(orthographicInputs[4].text) ? 0f : float.Parse(orthographicInputs[4].text);
        float right = string.IsNullOrEmpty(orthographicInputs[5].text) ? 0f : float.Parse(orthographicInputs[5].text);

        Matrix orthographicMatrix = MatrixHelpers.OrthographicProjectionMatrix(left, right, bottom, top, near, far);
        GetComponentInChildren<MatrixGUI>().matrix = orthographicMatrix;
    }
}
