using UnityEngine;
public abstract class FocusHandler : MonoBehaviour
{
    public virtual void HandleFocus()
    {
        // Always Override
    }

    public virtual string Hint()
    {
        // Always Override
        return "";
    }
}