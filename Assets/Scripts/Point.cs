using UnityEngine;
using System.Collections.Generic;

public enum PointType
{
    Empty, 
    RoomHealth, 
    ShipHealth,
    Shield
}

public class Point : MonoBehaviour
{
    private SpriteRenderer _pointRenderer;

    private Dictionary<PointType, Color> _pointColors = new Dictionary<PointType, Color>()
    {
        {PointType.Empty, new Color32(149, 149, 149, 255)},
        {PointType.RoomHealth, new Color32(0, 150, 0, 255)},
        {PointType.ShipHealth, new Color32(255, 0, 0, 255)},
        {PointType.Shield, new Color32(0, 0, 255, 255)}
    };

    private void Awake()
    {
        _pointRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetPoint(PointType type)
    {
        _pointRenderer.color = _pointColors[type];
    }
}
//code taken from chat gpt and modified
