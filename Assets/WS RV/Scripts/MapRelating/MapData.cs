using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapData
{
    public List<MapObject> MapObjects;

    public MapData()
    {
        MapObjects = new List<MapObject>();
    }

    public void AddObject(MapObject mapObject)
    {
        MapObjects.Add(mapObject);
    }
}
