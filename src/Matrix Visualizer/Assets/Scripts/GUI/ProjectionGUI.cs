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
    public static ProjectionGUI instance;
    [SerializeField] GameObject perspectiveInput, orthographicInput;
    [SerializeField] TMP_InputField[] perspectiveInputs, orthographicInputs;
    private void Awake() => instance = this;

    ToggleGroup tg;
    //[SerializeField] TMP_InputField[] rInputs, tInputs;
    private void Start()
    {
        /*MatrixGUI identityGUI = transform.GetChild(transform.childCount - 1).GetComponentInChildren<MatrixGUI>();
        identityGUI.matrix = Matrix.identity(4);*/
        /*foreach (var input in rInputs.Concat(tInputs))
            input.onValueChanged.AddListener((string value) => UpdateCameraViewSpaceMatrix());*/
        perspectiveInputs = perspectiveInput.GetComponentsInChildren<TMP_InputField>();
        orthographicInputs = orthographicInput.GetComponentsInChildren<TMP_InputField>();

        foreach (var input in perspectiveInputs)
            input.onValueChanged.AddListener((string value) => UpdatePerspectiveMatrix());

        foreach (var input in orthographicInputs)
            input.onValueChanged.AddListener((string value) => UpdateOrthographicMatrix());

        tg = GetComponentInChildren<ToggleGroup>();

        foreach (Toggle toggle in tg.GetComponentsInChildren<Toggle>())
            toggle.onValueChanged.AddListener((bool value) => UpdateProjectionInputs());

        UpdateProjectionInputs();        

        //UpdateCameraViewSpaceMatrix();
    }
    public void UpdateProjectionInputs()
    {
        perspectiveInput.SetActive(false);
        orthographicInput.SetActive(false);

        if (tg.ActiveToggles().First().name == "Perpective Toogle")
        {
            perspectiveInput.SetActive(true);
            UpdatePerspectiveMatrix();
        }
        else
        {
            orthographicInput.SetActive(true);
            UpdateOrthographicMatrix();
        }
    }
    public void UpdatePerspectiveMatrix()
    {
        float fov = string.IsNullOrEmpty(perspectiveInputs[0].text) ? 0f : float.Parse(perspectiveInputs[0].text);
        float aspect = string.IsNullOrEmpty(perspectiveInputs[1].text) ? 0f : float.Parse(perspectiveInputs[1].text);
        float near = string.IsNullOrEmpty(perspectiveInputs[2].text) ? 0f : float.Parse(perspectiveInputs[2].text);
        float far = string.IsNullOrEmpty(perspectiveInputs[3].text) ? 0f : float.Parse(perspectiveInputs[3].text);

        Matrix perspectiveMatrix = MatrixHelpers.PerspectiveProjectionMatrix(fov, aspect, near, far);
        GetComponentInChildren<MatrixGUI>().matrix = perspectiveMatrix;
    }
    public void UpdateOrthographicMatrix()
    {
        float left = string.IsNullOrEmpty(orthographicInputs[0].text) ? 0f : float.Parse(orthographicInputs[0].text);
        float top = string.IsNullOrEmpty(orthographicInputs[1].text) ? 0f : float.Parse(orthographicInputs[1].text);
        float near = string.IsNullOrEmpty(orthographicInputs[2].text) ? 0f : float.Parse(orthographicInputs[2].text);
        float far = string.IsNullOrEmpty(orthographicInputs[3].text) ? 0f : float.Parse(orthographicInputs[3].text);
        float bottom = string.IsNullOrEmpty(orthographicInputs[4].text) ? 0f : float.Parse(orthographicInputs[4].text);
        float right = string.IsNullOrEmpty(orthographicInputs[5].text) ? 0f : float.Parse(orthographicInputs[5].text);

        Matrix orthographicMatrix = MatrixHelpers.OrthographicProjectionMatrix(left, right, bottom, top, near, far);
        GetComponentInChildren<MatrixGUI>().matrix = orthographicMatrix;
    }
    /*public void UpdateCameraViewSpaceMatrix()
    {
        float[] rs = rInputs.Select(input => string.IsNullOrEmpty(input.text) ? 0f : float.Parse(input.text)).ToArray();
        float[] ts = tInputs.Select(input => string.IsNullOrEmpty(input.text) ? 0f : float.Parse(input.text)).ToArray();

        Matrix camWSTMatrix = MatrixHelpers.WorldSpaceTransformationMatrix(ts,new float[] { 1, 1, 1 },rs);
        Matrix cvsMatrix = MatrixHelpers.CameraViewSpaceMatrix(camWSTMatrix);
        GetComponentInChildren<MatrixGUI>().matrix = cvsMatrix;
    }*/
    /*public void WorldSpaceLayout()
    {
        transform.GetChild(3).SetParent(transform.parent, false); // lots of magic numbers here, but it works and gets the GUI in the right order
        transform.GetChild(2).SetParent(transform.parent, false);

        transform.GetChild(4).SetParent(transform.parent, false);
        transform.GetChild(4).SetParent(transform.parent, false);
        transform.GetChild(4).SetParent(transform.parent, false);
        transform.GetChild(4).SetParent(transform.parent, false);
        transform.GetChild(4).SetParent(transform.parent, false);

        transform.GetChild(1).SetParent(transform.parent, false);
        transform.GetChild(0).SetParent(transform.parent, false);
        transform.GetChild(0).SetParent(transform.parent, false);
        transform.GetChild(0).SetParent(transform.parent, false);

        transform.SetParent(transform.parent.parent, false);
    }*/
}
