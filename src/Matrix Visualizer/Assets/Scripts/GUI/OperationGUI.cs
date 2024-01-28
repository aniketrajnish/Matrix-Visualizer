using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using TMPro;

public class OperationGUI : MonoBehaviour
{
    /// <summary>
    /// Class that creates the operation buttons and adds listeners to them.
    /// </summary>
    [SerializeField] GameObject operationBtn, operationDisplayObject;
    [SerializeField] CalculationGUI cGUI;
    List<string> operationNames;
    [HideInInspector] public Operation currentOp = Operation.ADDITION;
    void Start()
    {
        operationNames = new List<string>();

        foreach (string operationName in System.Enum.GetNames(typeof(Operation)))        
            operationNames.Add(EnumString(operationName));

        CreateBtns();
    }
    void CreateBtns()
    {
        /// <summary>
        /// This method creates the operation buttons.
        /// It names the buttons according to the enum names.
        /// These buttons also have a listner that updates the GUI based on the operation selected.
        /// </summary>
        foreach (Transform child in transform)        
            Destroy(child.gameObject);

        for (int i = 0; i < operationNames.Count; i++)
        {
            GameObject btn = Instantiate(operationBtn, transform);
            TextMeshProUGUI textComponent = btn.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = operationNames[i];

            string operationName = operationNames[i];

            btn.GetComponent<Button>().onClick.AddListener(() => ChangeOperationMode(operationName));
            btn.GetComponent<Button>().onClick.AddListener(() => cGUI.UpdateDisplay());
        }
    }
    public static string EnumString(string enumName) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(enumName.ToLower().Replace("_", " ")); // converts enum to title case
    public static string ReverseEnumString(string enumName) => enumName.ToUpper().Replace(" ", "_"); // back from title case to enum
    public void ChangeOperationMode(string operationName) => currentOp = (Operation)System.Enum.Parse(typeof(Operation), ReverseEnumString(operationName)); // tracks the current operation
}
