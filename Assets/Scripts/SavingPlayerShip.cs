//GitHub Repository: https://github.com/Orionn08/GADV-Assignment-2
//this script saves the player's ship layout in the Ship Design and Combat scenes.
//it works closely together with PlayerShipBuilder.cs

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavingPlayerShip
{
    public List<RoomData> Rooms = new(); //creates the list to store all rooms, slots dont count, within the roomsParent, which the SceneManager has the value.

    public void SaveShip(Transform roomsParent)
    {
        Rooms.Clear(); //clears whatever records were in there previously, as the layout can change from scene to scene

        foreach(Transform room in roomsParent)
        {   
            Room roomScript = room.GetComponent<Room>();
            if (roomScript == null) continue; //this will be true for slots, which as said dont count.
            RoomData data = new RoomData(); //creates the new data that will be stores as an element in the Rooms list.
            data.prefab = room.GetComponent<Room>().Prefab;
            data.position = room.localPosition;
            data.name = room.name;
            Rooms.Add(data); //gets all the necessary information needed to ensure it can be replicated properly and correctly.
        }
    }
}

[System.Serializable]
public class RoomData
{
    public GameObject prefab;
    public Vector3 position;
    public string name;
} //declares what is necessary to be stored in the Rooms list. 

//made with the help of Chat GPT