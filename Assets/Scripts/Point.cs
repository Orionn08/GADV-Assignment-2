using UnityEngine;
using System.Collections.Generic;

public enum PointTypes
{
    Empty, 
    RoomHealth, 
    ShipHealth,
    Shield
}

public class Point : MonoBehaviour
{
    private SpriteRenderer _pointRenderer;

    private Dictionary<PointTypes, Color> _pointColors = new Dictionary<PointTypes, Color>()
    {
        { PointTypes.Empty, new Color32(149, 149, 149, 255) },
        { PointTypes.RoomHealth, new Color32(0, 150, 0, 255) },
        { PointTypes.ShipHealth, new Color32(255, 0, 0, 255) },
        { PointTypes.Shield, new Color32(0, 0, 255, 255)}
    };

    private void Awake()
    {
        _pointRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetPoint(PointTypes type)
    {
        _pointRenderer.color = _pointColors[type];
    }
}
