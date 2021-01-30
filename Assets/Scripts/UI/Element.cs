using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public IElementData _data;
    public virtual void SetData(IElementData data)
    {
        _data = data;
        UpdateVisuals();
    }
    
    public virtual void ClearData()
    {
        _data = null;
    }

    public virtual void UpdateVisuals()
    {
        if (_data == null) return;
    }
}
