using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using System.IO;
using UnityEngine.SceneManagement;
using MessagePack;
using MessagePack.Resolvers;

public class ConnectionManager : Singleton<ConnectionManager>, RealTimeMultiplayerListener
{
    public PlayGamesClientConfiguration config;
    public Participant player;
    public int playerIndex;
    public List<Participant> participants = new List<Participant>();
    public object[] dataReceived;

    protected ConnectionManager() {
        
    }

    public void Init() {
        config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
    }

    public void CreateQuickGame()
    {
        const int MIN_OPPONENTS = 1, MAX_OPPONENTS = 1;
        const int GameType = 0;

        PlayGamesPlatform.Instance.RealTime.CreateQuickGame(MIN_OPPONENTS, MAX_OPPONENTS, GameType, this);
    }

    public void CreateWithInvitationScreen()
    {
        const int MIN_OPPONENTS = 1, MAX_OPPONENTS = 4;
        const int GameType = 0;

        PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(MIN_OPPONENTS, MAX_OPPONENTS, GameType, this);
    }

    public void OnRoomSetupProgress(float progress)
    {
        //PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
        MainMenuManager.Instance.txtStatus.text = "Waiting for room...";
    }

    public void OnRoomConnected(bool success)
    {
        if (success)
        {
            MainMenuManager.Instance.txtStatus.text = "Room connected success";
            player = PlayGamesPlatform.Instance.RealTime.GetSelf();
            MainMenuManager.Instance.txtStatus.text = player.ParticipantId;
            participants = PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
            for (int i = 0; i < participants.Count; i++)
            {
                if (participants[i].ParticipantId == player.ParticipantId)
                {
                    playerIndex = i;
                }
            }
            MainMenuManager.Instance.txtStatus.text = "Participants count: " + participants.Count;
            MainMenuManager.Instance.txtStatus.text = "Loading game scene...";
            MainMenuManager.Instance.LoadSceneGameplay();
        }
        else
        {
            MainMenuManager.Instance.txtStatus.text = "Room connected failed";
        }
        
    }

    public void OnLeftRoom()
    {
        throw new NotImplementedException();
    }

    public void OnParticipantLeft(Participant participant)
    {
        participants.Remove(participant);
    }

    public void OnPeersConnected(string[] participantIds)
    {/*
        List<Participant> _participants = PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
        foreach (Participant _p in participants)
        {
            foreach(Participant _p2 in _participants)
            {
                if (_p.ParticipantId != player.ParticipantId && _p.ParticipantId != _p2.ParticipantId && isConnected)
                {
                    participants.Add(_p);
                    GameManager.Instance.UpdateParticipant(_p);
                }
            }
        }*/
    }

    public void OnPeersDisconnected(string[] participantIds)
    {
        throw new NotImplementedException();
    }

    public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
    {
        dataReceived = MessagePackSerializer.Deserialize<object[]>(data, ContractlessStandardResolver.Instance);
        /*if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("SampleScene") && dataReceived != null)
        {
            GameManager.Instance.txtDebugger.text =
            "SelfID: " + player.ParticipantId +
            "\nType: " + dataReceived[0] +
            "\nPlayer ID: " + dataReceived[1] +
            "\nPosition X: " + (float)dataReceived[2] +
            "\nPosition Y: " + (float)dataReceived[3] +
            "\nPosition Z: " + (float)dataReceived[4] +
            "\nRotation X: " + (float)dataReceived[5] +
            "\nRotation Y: " + (float)dataReceived[6] +
            "\nRotation Z: " + (float)dataReceived[7] +
            "\nLap: " + (int)dataReceived[8] + 
            "\nBest Lap Time: " + (int)dataReceived[9];
        }*/
    }
}