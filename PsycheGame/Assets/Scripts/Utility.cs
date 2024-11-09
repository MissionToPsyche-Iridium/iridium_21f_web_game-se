using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utility
{
    public static T FindComponentInParents<T>(GameObject gameObject) where T : Component
    {
        if (gameObject == null)
        {
            return null;
        }

        T component = gameObject.GetComponent<T>();
        Transform parent = gameObject.transform.parent;

        while (parent != null && component == null)
        {
            component = parent.gameObject.GetComponent<T>();
            parent = parent.parent;
        }

        return component;
    }

    public static T FindComponentFromRoot<T>(GameObject root) where T : Component
    {
        T component = root.GetComponent<T>();
        if (component != null)
        {
            return component;
        }

        foreach (Transform child in root.transform)
        {
            component = FindComponentFromRoot<T>(child.gameObject);
            if (component != null)
            {
                return component;
            }
        }

        return null;
    }

    public static T FindComponentInScene<T>(Scene scene) where T : Component
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            T component = FindComponentFromRoot<T>(root);
            if (component != null)
            {
                return component;
            }
        }
        return null;
    }

    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        T copy = destination.AddComponent<T>();
        foreach (FieldInfo field in original.GetType().GetFields())
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }
}
