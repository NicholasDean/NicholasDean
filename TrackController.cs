using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour
{
    public static TrackController Instance { set; get; }

    public GameObject[] trackPrefabs = new GameObject[3]; //Array of game objects to hold the, currently 3, track prefabs
    private List<GameObject> currTrack = new List<GameObject>();

    private float zPos = 0; //Spawns the tracks a 0z value

    private int trackLength = 84; //The length of each piece of track is the same

    private int shownTracks = 3; //No of shown tracks before the prev is deleted

    public Transform player;

    public Transform resetBox;

    private void Start()
    {

        for (int i = 0; i < shownTracks; i++)
        {
            PositionTrack(Random.Range(0, trackPrefabs.Length)); //This will randomly spawn the number of tracks, 3, to make it randomly generated
        } 
        
    }

    private void Update()
    {
        if(player.position.z - 84 > zPos - (shownTracks * trackLength)) //If the player has hit the tile infront of them resawn a new one, will only spawn once the first track has been completed
        {
            PositionTrack(Random.Range(0, trackPrefabs.Length)); //Randomly spawns a new tile
            DestroyTrack();
        }
    }
    public void PositionTrack(int index)
    {
        GameObject instantiate = Instantiate(trackPrefabs[index], transform.forward * zPos, transform.rotation); //This allows each track to be cloned into the level automatically each time a new track is needed
        currTrack.Add(instantiate); //adds the current tile being used to this list
        zPos += trackLength; //This allows each track to be placed on the end of the prev track
    }

    private void DestroyTrack()
    {
        Destroy(currTrack[0]); //This deleted the first track in the list once the player hits the point where a new track is spawned
        currTrack.RemoveAt(0); //This removes the track from the list making the 2nd oldest index 0.
    }

}
