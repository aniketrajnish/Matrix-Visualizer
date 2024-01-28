using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using TMPro;

public class OperationGUI : MonoBehaviour
{
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
    public static string EnumString(string enumName) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(enumName.ToLower().Replace("_", " "));
    public static string ReverseEnumString(string enumName) => enumName.ToUpper().Replace(" ", "_");
    public void ChangeOperationMode(string operationName) => currentOp = (Operation)System.Enum.Parse(typeof(Operation), ReverseEnumString(operationName));
}
