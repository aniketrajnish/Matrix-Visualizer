using UnityEngine;
using MatrixLibrary;

public class WorldSpaceGUI : MonoBehaviour
{
    public static WorldSpaceGUI instance;
    private void Awake() => instance = this;
    private void Start()
    {
        MatrixGUI identityGUI = transform.GetChild(transform.childCount - 1).GetComponentInChildren<MatrixGUI>();
        identityGUI.matrix = Matrix.identity(4);        
    }
    public void WorldSpaceLayout()
    {
        transform.GetChild(3).SetParent(transform.parent, false);
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
    }    
}
