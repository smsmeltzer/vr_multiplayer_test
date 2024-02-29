using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Runtime.CompilerServices;

public class PlayerInfo
{
    public int actor;
    public short lives;
    public bool team;

    public PlayerInfo(int actor, short lives, bool team)
    {
        this.actor = actor;
        this.lives = lives;
        this.team = team;
    }
}
public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        ConnectedtoServer();
    }

    private void ConnectedtoServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Attempting a connection to server...");

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Successfully Connected!");
        base.OnConnectedToMaster();

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = true;   
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined the room!");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New player joined the room");
        base.OnPlayerEnteredRoom(newPlayer);
    }

}
