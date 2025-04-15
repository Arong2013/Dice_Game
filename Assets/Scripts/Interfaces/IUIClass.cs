
using UnityEngine;

public interface IHoverable
{
    void OnHoverEnter();
    void OnHoverExit();
}

public interface IUIRenderable
{
    void BindToUI(GameObject uiRoot);
}
