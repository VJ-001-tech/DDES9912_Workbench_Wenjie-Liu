using UnityEngine;
using UnityEngine.InputSystem;

public class RayManager : MonoBehaviour
{
    public Camera rayCamera;
    private Ray ray;
    private RaycastHit hit;

    private ClickBase lastClickGo;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            ray = rayCamera.ScreenPointToRay(Mouse.current.position.value);
            if(Physics.Raycast(ray, out hit, 100))
            {
                lastClickGo = hit.collider.GetComponent<ClickBase>();
                if (lastClickGo != null)
                {
                    lastClickGo.OnMouseDownClick();
                }
            }
        }

        if(Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (lastClickGo != null)
            {
                lastClickGo.OnMouseUpClick();
                lastClickGo = null;
            }
        }
    }

}
