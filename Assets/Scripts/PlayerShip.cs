using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerShip
{
    public List<RoomData> rooms = new List<RoomData>();

    public void SaveShip(Transform roomsParent)
    {
        rooms.Clear();

        foreach(Transform room in roomsParent)
        {
            Room roomScript = room.GetComponent<Room>();
            if (roomScript == null)
                continue;
            RoomData data = new RoomData();
            data.prefab = room.GetComponent<Room>().prefab;
            data.position = room.localPosition;
            data.name = room.name;
            rooms.Add(data);
        }
    }
}


[System.Serializable]
public class RoomData
{
    public GameObject prefab;
    public Vector3 position;
    public string name;
}
//made with the help of Chat GPT