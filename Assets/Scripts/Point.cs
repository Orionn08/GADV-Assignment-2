//GitHub Repository: https://github.com/Orionn08/GADV-Assignment-2
//this script holds the dictionary of the different colors that the various point types have,
//allowing other scripts to call for a point to be a specific type, via changing color, using a method in this script

using UnityEngine;
using System.Collections.Generic;

public enum PointType
{
    Empty, 
    RoomHealth, 
    ShipHealth,
    Shield,
    ExtraRoomHealth
} //creates the references of the different types of points for the dictionary below and also for other scripts to more easily call a specific point type.

public class Point : MonoBehaviour
{
    private SpriteRenderer _pointRenderer; //needs to be referenced or else the color of the object, which is what determines the point type, can't be changed.

    private Dictionary<PointType, Color> _pointColors = new Dictionary<PointType, Color>()
    {
        {PointType.Empty, new Color32(149, 149, 149, 255)},
        {PointType.RoomHealth, new Color32(0, 150, 0, 255)},
        {PointType.ShipHealth, new Color32(255, 0, 0, 255)},
        {PointType.Shield, new Color32(0, 0, 255, 255)},
        {PointType.ExtraRoomHealth, new Color32(149, 255, 149, 255)}
    };

    private void Awake()
    {
        _pointRenderer = GetComponent<SpriteRenderer>();

        if (_pointRenderer == null)
        {
            Debug.LogError($"{name}: Point renderer cannot be found");
            return;
        }
    }

    public void SetPoint(PointType type)
    {
        _pointRenderer.color = _pointColors[type];
    } //sets the type of the point, by changing its color, based on what type the other script wants.
}
//code taken from chat gpt and modified
