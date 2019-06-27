using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames.BasicApi.Multiplayer;

public class MainMenuManager : MonoBehaviour
{
    public Button btnQuickMatch;
    public Text txtStatus;
    public static MainMenuManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        ConnectionManager.Instance.Init();
        btnQuickMatch.gameObject.SetActive(false);
        Social.localUser.Authenticate((bool success) => {
            if (success)
                btnQuickMatch.gameObject.SetActive(true);
        });
    }
    
    public void BtnQuickMatchPressed()
    {
        txtStatus.text = "Finding match...";
        btnQuickMatch.gameObject.SetActive(false);
        ConnectionManager.Instance.CreateQuickGame();
    }

    public void BtnLeaderboardPressed()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkI_tK68fYQEAIQAg");
    }

    public void BtnLeaderboardPlusPressed()
    {
        Social.ReportScore(12345, "CgkI_tK68fYQEAIQAg", (bool success) => {
            // handle success or failure
        });
    }

    public void LoadSceneGameplay()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
    /*
    public void CreateQuickGame()
    {
        const int MIN_OPPONENTS = 1, MAX_OPPONENTS = 1;
        const int GameType = 0;

        PlayGamesPlatform.Instance.RealTime.CreateQuickGame(MIN_OPPONENTS, MAX_OPPONENTS, GameType, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRoomSetupProgress(float percent)
    {
        throw new System.NotImplementedException();
    }

    public void OnRoomConnected(bool success)
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnLeftRoom()
    {
        throw new System.NotImplementedException();
    }

    public void OnParticipantLeft(Participant participant)
    {
        throw new System.NotImplementedException();
    }

    public void OnPeersConnected(string[] participantIds)
    {
        throw new System.NotImplementedException();
    }

    public void OnPeersDisconnected(string[] participantIds)
    {
        throw new System.NotImplementedException();
    }

    public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
    {
        throw new System.NotImplementedException();
    }*/
}
