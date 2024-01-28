using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSwitchGUI : MonoBehaviour
{
    /// <summary>
    /// For the next week :]
    /// </summary>
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(SceneSwitchTempMessgae);
    }
    void SceneSwitchTempMessgae()
    {
        ErrorGUI.instance.ShowError("Hey Chris! I'll work on the 3D view next week :)", 3f);
    }
}
