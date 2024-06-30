using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This static class allows to activate, deactivate, or toggle any UI element in the program.
/// </summary>
public static class StaticUIManager
{
    /// <summary>
    /// Activate a selected GameObject (UI element).
    /// </summary>
    /// <param name="uiElement">The UI element to activate.</param>
    public static void ActivateUIElement(GameObject uiElement)
    {
        if (uiElement != null)
        {
            uiElement.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"{nameof(StaticUIManager)}: {nameof(ActivateUIElement)} received a null reference.");
        }
    }

    /// <summary>
    /// Deactivate a selected GameObject (UI element).
    /// </summary>
    /// <param name="uiElement">The UI element to deactivate.</param>
    public static void DeactivateUIElement(GameObject uiElement)
    {
        if (uiElement != null)
        {
            uiElement.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"{nameof(StaticUIManager)}: {nameof(DeactivateUIElement)} received a null reference.");
        }
    }

    /// <summary>
    /// Toggle a selected GameObject (UI element).
    /// </summary>
    /// <param name="uiElement">The UI element to toggle.</param>
    public static void ToggleUIElement(GameObject uiElement)
    {
        if (uiElement != null)
        {
            uiElement.SetActive(!uiElement.activeSelf);
        }
        else
        {
            Debug.LogWarning($"{nameof(StaticUIManager)}: {nameof(ToggleUIElement)} received a null reference.");
        }
    }
}