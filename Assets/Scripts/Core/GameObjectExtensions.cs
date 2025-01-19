#nullable enable

using System;
using UnityEngine;

public static class GameObjectExtensions {
    public static T GetComponent_ThrowIfNull<T>(this GameObject pGameObject) {
        T? component = pGameObject.GetComponent<T>();
        if (component == null) {
            throw new Exception($"Could not GetComponent on GameObject [{pGameObject.name}]. Component was null!");
        }

        return component;
    }
    
    public static T GetComponentInChildren_ThrowIfNull<T>(this GameObject pGameObject, bool pIncludeInactive = false) {
        T? component = pGameObject.GetComponentInChildren<T>(pIncludeInactive);
        if (component == null) {
            throw new Exception($"Could not GetComponent on GameObject [{pGameObject.name}]. Component was null!");
        }

        return component;
    }
    
    public static void DestroyChildren(this GameObject pGameObject) {
        foreach (Transform child in pGameObject.transform) {
            UnityEngine.Object.Destroy(child.gameObject);
        }
    }

    public static void DestroyChildrenImmediate(this GameObject pGameObject) {
        foreach (Transform child in pGameObject.transform) {
            UnityEngine.Object.DestroyImmediate(child.gameObject);
        }
    }

    public static GameObject? FindChildByName(this GameObject pParent, string pName) {
        // Check if the current GameObject has the target name
        if (pParent.name == pName) {
            return pParent;
        }

        // Iterate through each child and perform a recursive search
        foreach (Transform child in pParent.transform) {
            GameObject result = child.gameObject.FindChildByName(pName);
            if (result != null) {
                return result;
            }
        }

        // If not found in this branch, return null
        return null;
    }
}
