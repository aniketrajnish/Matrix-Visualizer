using UnityEngine;
[ExecuteInEditMode]
public class Debug : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*print("W2L" + transform.worldToLocalMatrix); 
        print("L2W" + transform.localToWorldMatrix);*/ 
        print("L2W\n" + Matrix4x4.Inverse(Camera.main.transform.localToWorldMatrix));
        print("VM\n" + Camera.main.worldToCameraMatrix);
    }
}
