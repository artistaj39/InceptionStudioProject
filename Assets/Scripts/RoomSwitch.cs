using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitch : MonoBehaviour
{
    [SerializeField] GameObject controller;
    [SerializeField]
    GameObject[] rooms;
    GameObject[] usedRooms;
    int currentRoom = 0;


    int roomId = 1;
    // Start is called before the first frame update
    void Start()
    {
        usedRooms = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            usedRooms[i] = rooms[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 controllerPosition = controller.transform.position;
        if (controllerPosition.x > 0 && controllerPosition.z > 0)
        {
            // Change Room 2
            usedRooms[2] = rooms[roomId];
            roomId = (roomId + 1) % rooms.Length;
            currentRoom = roomId;
        }
        else if (controllerPosition.x < 0 && controllerPosition.z > 0)
        {
            // Change Room 3
            usedRooms[3] = rooms[roomId];
            roomId = (roomId + 1) % rooms.Length;
            currentRoom = roomId;
        }
        else if (controllerPosition.x < 0 && controllerPosition.z < 0)
        {
            // Change Room 0
            usedRooms[0] = rooms[roomId];
            roomId = (roomId + 1) % rooms.Length;
            currentRoom = roomId;
        }
        else if (controllerPosition.x > 0 && controllerPosition.z < 0)
        {
            // Change Room 1
            usedRooms[1] = rooms[roomId];
            roomId = (roomId + 1) % rooms.Length;
            currentRoom = roomId;
        }



    }
}
