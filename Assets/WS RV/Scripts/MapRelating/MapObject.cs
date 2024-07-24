using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MapObject
{
    public string ObjectID;
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
    public Vector3 FinalPosition; // Optionnel, utilisé uniquement pour certains objets
    public float Speed;

    public MapObject(string id, Vector3 position, Quaternion rotation, Vector3 scale, Vector3 finalPosition, float speed)
    {
        ObjectID = id;
        Position = position;
        Rotation = rotation;
        Scale = scale;
        FinalPosition = finalPosition;
        Speed = speed;
    }
}