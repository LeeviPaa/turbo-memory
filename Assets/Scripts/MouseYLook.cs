using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseYLook : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 150;

    private void Update()
    {
        MouseRotate();
    }
    
    private void MouseRotate()
    {
        if(Cursor.visible)
            return;

        transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), 0, 0) * _rotationSpeed * Time.deltaTime);
    }
}
