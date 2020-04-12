using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float Speed;
    public float ZoomSpeed;
    void Update()
    {
        CameraMove();
    }

    void CameraMove()
    {
    
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 cameraMovement = new Vector3(hor, ver, 0f) * Speed;
        transform.Translate(cameraMovement, Space.Self);

        float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
        
        if (Mathf.Abs(scrollAmount) > .01f)
        {
            Vector3 hitpos = mouseRay.origin - (mouseRay.direction);
            Vector3 dir = Camera.main.transform.position - hitpos;       
            Camera.main.transform.Translate(dir * scrollAmount * ZoomSpeed, Space.World);
        }
        

    }
    


}
