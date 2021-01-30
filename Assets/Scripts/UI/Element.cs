using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour
{
    public IElementData _data;
    public virtual void SetData(IElementData data)
    {
        if (_data != null && _data.IsEqual(data)) return;
        _data = data;
        BindData();
        UpdateVisuals();
    }

    /// <summary>
    /// Handles any binds to the data provided if possible, to listen possible events
    /// </summary>
    protected abstract void BindData();

    /// <summary>
    /// Handles unbinding anything regarding the data.
    /// </summary>
    protected abstract void UnbindData();

    public virtual void ClearData()
    {
        UnbindData();
        _data = null;
    }

    public virtual void UpdateVisuals()
    {
        if (_data == null) return;
    }
}
