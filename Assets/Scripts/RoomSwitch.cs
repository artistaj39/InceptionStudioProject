using UnityEngine;

/// <summary>
/// Generates four interconnected rooms arranged as a 2x2 sequence.
/// When the player moves to a new room, the corresponding room 
/// diagonal to the player updates to a new one.
/// </summary>
public class RoomSwitch : MonoBehaviour
{
    /// <summary>
    /// Parameters
    /// ----------
    /// player          :    GameObject, default: None
    ///     The player (controller) game object (i.e., capsule).
    /// roomPrefabs     :    array of GameObject, default: None
    ///     Container that stores different types of rooms as prefabs.
    /// roomSize        :    float, default: 20f
    ///     Size of the rooms available, assuming equal length and width.
    /// </summary>

    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject[] roomPrefabs;

    [SerializeField, Range(1f, 50f)]
    float roomSize = 20f;


    /// <summary>
    /// Internal Variables
    /// ------------------
    /// rooms           : array of GameObject
    ///     Array that stores the 4 rooms to be generated and manipulated.
    /// roomId          : int, default: 0
    ///     Index of the room to be manipulated.
    /// roomPrefabId    : int, default: 0
    ///     Index of the room prefab to be used to update the room.
    /// oldPlayerState  : int, default: 0
    ///     Tracking variable for player state update. 
    ///     Triggers the room to change if different from newPlayerState.
    /// newPlayerState  : int, default: 0
    ///     Tracking variable for player state update.
    ///     Updates when player enters a new room based on its position.
    /// </summary>

    GameObject[] rooms;

    int roomId = 0;
    int roomPrefabId = 0;
    int oldPlayerState = 0;
    int newPlayerState = 0;


    void Awake()
    {
        // Initialize the room
        InitializeRooms();

        // Initialize the player state
        newPlayerState = SetPlayerState();
    }


    /// <summary>
    /// On game start, create 4 identical instances of the default room
    /// and orient them accordingly.
    /// </summary>
    void InitializeRooms()
    {
        rooms = new GameObject[4];
        for (int i = 0; i < 4; i++) InstantiateRoom(i, 0);
    }


    /// <summary>
    /// Create an instance of a room based upon where it is 
    /// and which prefab to use. 
    /// </summary>
    /// <param name="instanceId">The index for the room to be made.</param>
    /// <param name="prefabId">The index for the prefab to use.</param>
    void InstantiateRoom(int instanceId, int prefabId)
    {
        // Determine an angular value from origin to place the room.
        float angle = Mathf.PI * 0.25f * (2 * instanceId + 1);

        // Create the room, and assign its position based on the angular value
        // and its size.
        rooms[instanceId] = Instantiate(roomPrefabs[prefabId]);
        rooms[instanceId].transform.position = (
            Vector3.right * Mathf.Cos(angle) +
            Vector3.forward * Mathf.Sin(angle)
        ) * roomSize * Mathf.Sqrt(2f);

        // Orient the room so that it connects with its neighbors.
        // NOTE: this can change depending on the room geometry!
        rooms[instanceId].transform.localRotation =
            Quaternion.Euler(Vector3.up * 45f * (1 - 2 * instanceId));
        
        // Make the room a child object of this script's gameObject.
        rooms[instanceId].transform.parent = transform;
    }


    /// <summary>
    /// Determines the state of the player 
    /// (i.e., the room index where the player is).
    /// </summary>
    /// <returns>An integer representing the player state.</returns>
    int SetPlayerState()
    {
        // Get player position in the world coordinates.
        Vector3 p = player.transform.position;

        // Set the state based upon the x- and z- coordinates.
        int state;
        if (p.x > 0 && p.z > 0) state = 0;
        else if (p.x < 0 && p.z > 0) state = 1;
        else if (p.x < 0 && p.z < 0) state = 2;
        else if (p.x > 0 && p.z < 0) state = 3;
        else state = -1;

        // Return the state
        return state;
    }


    private void Update()
    {
        // With every frame update, get the player's state (room index).
        newPlayerState = SetPlayerState();

        // If the state changes:
        if (newPlayerState != oldPlayerState)
        {
            // Find the index of the diagonal room
            roomId = (newPlayerState + 2) % 4;

            // Find the next prefab in the sequence to use
            roomPrefabId = (roomPrefabId + 1) % roomPrefabs.Length;

            // Destroy the old diagonal room
            Destroy(rooms[roomId]);

            // Restore a new diagonal room
            InstantiateRoom(roomId, roomPrefabId);
            
            // Update player state
            oldPlayerState = newPlayerState;
        }

    }
}
