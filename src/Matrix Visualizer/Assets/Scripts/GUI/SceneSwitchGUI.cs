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
        ErrorGUI.instance.ShowError("Hey Chris! I think I need some time to work on my foundation to develop the 3D viewer! \n " +
            "I plan on doing it before the Industry Review.", 5f);
    }
}
