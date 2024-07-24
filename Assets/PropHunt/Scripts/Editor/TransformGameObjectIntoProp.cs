using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public static class TransformGameObjectIntoProp
{

    public static string PrefabPath = "Assets/PropHunt/Prefabs/Props/";
    /// <summary>
    /// Transform a GameObject containing Rigidbody, Meshrenderer, MeshFilter and Collider into a GameObject following the prop architecture.
    /// </summary>
    [MenuItem("Edit/TransformIntoProp")]
    public static void TransformIntoProp()
    {
        var selectedGameobjects = Selection.gameObjects;

        foreach(GameObject sourceGo in selectedGameobjects)
        {
            GameObject clone;
            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(sourceGo);
            if (prefab != null)
                clone = PrefabUtility.InstantiatePrefab(prefab, sourceGo.transform.parent) as GameObject;
            else
                clone = UnityEngine.Object.Instantiate(sourceGo, sourceGo.transform.parent);
            clone.name = sourceGo.name;

            NetworkGameObject(clone);
            var propChild = CreatePropChild(clone);
            AddPropComponent(clone, propChild);
            PrefabUtility.SaveAsPrefabAssetAndConnect(clone, PrefabPath + clone.name + ".prefab", InteractionMode.AutomatedAction);
            clone.transform.SetPositionAndRotation(sourceGo.transform.position, sourceGo.transform.rotation);
            GameObject.DestroyImmediate(sourceGo);
        }
    }

    /// <summary>
    /// Move the meshRenderer, meshFilter and colliders to a child gameObject. All children of the original Gameobject will become children of the body.
    /// </summary>
    /// <param name="go">The object to edit</param>
    /// <returns>The base object with a whild containing the meshRenderer, meshFilter and colliders components.</returns>
    private static GameObject CreatePropChild(GameObject go)
    {
        try
        {
            PrefabUtility.UnpackPrefabInstance(go, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }
        catch (Exception e)
        {
            Debug.LogWarningFormat("Transformed object {0} is not a prefab. It may not be a problem.", go.name);
            Debug.LogWarning(e);
        }
        var bodyObject = new GameObject(go.name + "Body");

        //GetRenderer of current Object
        var meshRenderer = go.GetComponent<MeshRenderer>();
        if(meshRenderer != null)
        {
            var meshFilter = go.GetComponent<MeshFilter>();
            CutPasteComponentOnGameObject(meshRenderer, bodyObject);
            CutPasteComponentOnGameObject(meshFilter, bodyObject);
        }
        int numberOfChildren = go.transform.childCount;
        for (int i = 0; i < numberOfChildren; i++)
        {
            var transformChild = go.transform.GetChild(0);
            var localPos = transformChild.localPosition;
            var localRot = transformChild.localRotation;
            transformChild.SetParent(bodyObject.transform);
            transformChild.SetLocalPositionAndRotation(localPos, localRot);
        }

        var colliders = go.GetComponents<Collider>();
        foreach (var collider in colliders)
        {
            CutPasteComponentOnGameObject(collider, bodyObject);
        }
        bodyObject.transform.parent = go.transform;
        bodyObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        return bodyObject;
    }

    /// <summary>
    /// Copy a component and paste it on the target GameObject, then destroy the component.
    /// </summary>
    /// <param name="component">Component to copy on the gameobject..</param>
    /// <param name="target">Target to copy onto.</param>
    private static void CutPasteComponentOnGameObject(Component component, GameObject target)
    {
        ComponentUtility.CopyComponent(component);
        ComponentUtility.PasteComponentAsNew(target);
        UnityEngine.Object.DestroyImmediate(component);
    }

    /// <summary>
    /// Add the prop component and fill it. Add a rigidbody if there is none.
    /// </summary>
    /// <param name="sourceObject">Object on which we create the prop.</param>
    /// <param name="propChild">Body of the gameObject.</param>
    private static void AddPropComponent(GameObject sourceObject, GameObject propChild)
    {
        Prop prop = sourceObject.AddComponent<Prop>();
        if(prop == null)
        {
            prop = sourceObject.GetComponent<Prop>();
        }
        var rigidbody = sourceObject.GetComponent<Rigidbody>();
        if(rigidbody == null)
        {
            rigidbody = sourceObject.AddComponent<Rigidbody>();
        }
        prop.Rigidbody = rigidbody;
        prop.BodyGameObject = propChild;
        prop.PropName = sourceObject.name;
    }

    /// <summary>
    /// Add Network components to the gameObject if they don't exist.
    /// </summary>
    /// <param name="go">The newly networked gameObject.</param>
    private static void NetworkGameObject(GameObject go)
    {
        if (go.GetComponent<NetworkObject>() == null)
        {
            go.AddComponent<NetworkObject>();
        }

        if (go.GetComponent<NetworkRigidbody>())
        {
            //NetworkRigidbody requires NetworkTransform, so if it does exist, then we don't need to check for NetworkTransform
            return;
        }
        go.AddComponent<NetworkRigidbody>();

        if (!go.GetComponent<NetworkTransform>()){
            go.AddComponent<NetworkTransform>();
        }
    }
}
