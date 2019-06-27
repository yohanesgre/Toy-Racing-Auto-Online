using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlitheFramework;
using WayPoint;
using GooglePlayGames.BasicApi.Multiplayer;
using DG.Tweening;
using GooglePlayGames;
using MessagePack;
using MessagePack.Resolvers;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class GameManager : BaseClass
{
    #region Initialize
    #region EVENT
    public event EventHandler EVENT_REMOVE;
    #endregion EVENT

    #region Public_field
    public static GameManager Instance;
    public FactoryPlayer FactoryPlayer { get => factoryPlayer; set => factoryPlayer = value; }
    public int TotalLaps { get => totalLaps; set => totalLaps = value; }
    public bool isGameStart;
    public bool isGameOver;
    
    #endregion Public_field

    #region Pivate_field
    [SerializeField]
    private GameObject[] spawnner;
    private int totalLaps;
    #endregion Pivate_field
    #endregion Initialize

    public override void Init()
    {
        Instance = this;
        CreateFactoryPlayer();
        CreateFactoryContestant();
        InitPlayer();
        GameObject.Find("Main Camera").GetComponent<CameraFollow>().target = factoryPlayer.Get(0).transform;
        //GameObject.Find("Mini Map Camera").GetComponent<CameraFollow>().target = factoryPlayer.Get(0).transform;
        InitContestant();
        InitStartGame();
    }

    void Start()
    {
        Init();
    }
    #region factory
    [SerializeField] private GameObject prefabContestant;
    FactoryContestant factoryContestant;
    private void CreateFactoryContestant()
    {
        var go = new GameObject();
        go.name = "FactoryContestant";
        factoryContestant = new FactoryContestant();
        factoryContestant = go.AddComponent<FactoryContestant>() as FactoryContestant;
        #region EVENT_LISTENER_ADD_FactoryContestant
        factoryContestant.EVENT_REMOVE += OnRemoveFactoryContestant;
        #endregion EVENT_LISTENER_ADD_FactoryContestant    
}

    [SerializeField] private GameObject prefabPlayer;
    FactoryPlayer factoryPlayer;
    private void CreateFactoryPlayer()
    {
        var go = new GameObject();
        go.name = "FactoryPlayer";
        factoryPlayer = new FactoryPlayer();
        factoryPlayer = go.AddComponent<FactoryPlayer>() as FactoryPlayer;
        #region EVENT_LISTENER_ADD_FactoryPlayer
        factoryPlayer.EVENT_REMOVE += OnRemoveFactoryPlayer;
        #endregion EVENT_LISTENER_ADD_FactoryPlayer    
}

    #region EVENT_LISTENER_ADD
    #endregion EVENT_LISTENER_ADD
    #region EVENT_LISTENER_METHOD
    private void OnRemoveFactoryContestant(object _sender, EventArgs e)
    {
        GameObject sender = (GameObject)_sender;
        #region EVENT_LISTENER_REMOVE_FactoryContestant
        sender.GetComponent<FactoryContestant>().EVENT_REMOVE -= OnRemoveFactoryContestant;
        #endregion EVENT_LISTENER_REMOVE_FactoryContestant
        Destroy(sender);
    }

    
    private void OnRemoveFactoryPlayer(object _sender, EventArgs e)
    {
        GameObject sender = (GameObject)_sender;
        #region EVENT_LISTENER_REMOVE_FactoryPlayer
        sender.GetComponent<FactoryPlayer>().EVENT_REMOVE -= OnRemoveFactoryPlayer;
        #endregion EVENT_LISTENER_REMOVE_FactoryPlayer
        Destroy(sender);
    }

    #endregion EVENT_LISTENER_METHOD
    #endregion factory
    #region private method
    void InitStartGame()
    {
        totalLaps = 3;
        StartCoroutine(CountdownStartGame());
    }

    IEnumerator CountdownStartGame()
    {
        GameManagerUI.Instance.TxtCountdownStartGame.text = "Race will start in ...";
        yield return new WaitForSeconds(1f);
        for (int i = 3; i < 0; i--)
        {
            GameManagerUI.Instance.TxtCountdownStartGame.text = (i).ToString();
            yield return new WaitForSeconds(1f);
        }
        GameManagerUI.Instance.TxtCountdownStartGame.text = "GO!!";
        yield return new WaitForSeconds(0.3f);
        GameManagerUI.Instance.TxtCountdownStartGame.gameObject.SetActive(false);
        GameManagerUI.Instance.PanelCountdownStartGame.SetActive(false);
        isGameStart = true;
        yield return null;
    }

    IEnumerator BackToMainMenu()
    {
        yield return new WaitForSeconds(2f);
        GameManagerUI.Instance.TxtCountdownStartGame.text = "Leaving race...";
        yield return new WaitForSeconds(0.5f);
        GameManagerUI.Instance.TxtCountdownStartGame.text = "Return to Main Menu...";
        PlayGamesPlatform.Instance.RealTime.LeaveRoom();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(0);
        yield return null;
    }

    void InitPlayer()
    {
        switch (ConnectionManager.Instance.playerIndex)
        {
            case 0:
                factoryPlayer.Add(prefabPlayer, spawnner[0].transform.position, Quaternion.Euler(0, 0, -90), ConnectionManager.Instance.player.ParticipantId);
                break;
            case 1:
                factoryPlayer.Add(prefabPlayer, spawnner[1].transform.position, Quaternion.Euler(0, 0, -90), ConnectionManager.Instance.player.ParticipantId);
                break;
        }
        //factoryPlayer.Add(prefabPlayer, spawnner[0].transform.position, Quaternion.Euler(0, 0, -90), "1");
    }

    private void InitContestant()
    {
        for (int i = 0; i < ConnectionManager.Instance.participants.Count; i++)
        {
            if (ConnectionManager.Instance.participants[i].ParticipantId != ConnectionManager.Instance.player.ParticipantId)
                factoryContestant.Add(prefabContestant, spawnner[i].transform.position, Quaternion.Euler(0, 0, -90), ConnectionManager.Instance.participants[i].ParticipantId);
        }
    }
    #endregion
    #region public method
    public void Remove()
    {
       dispatchEvent(EVENT_REMOVE, this.gameObject, EventArgs.Empty);
    }
    #endregion
    #region update
    private void Update()
    {
        UpdateMethod();
    }
    
    public void UpdateMethod()
    {
        if (!isGameStart)
            return;
        if (!isGameOver)
        {
            UpdateCarMovement();
            UpdateLaps();
        }
    }

    private void UpdateLaps()
    {
        if ((int)ConnectionManager.Instance.dataReceived[8] >= totalLaps + 1)
        {
            GameManagerUI.Instance.PanelCountdownStartGame.SetActive(true);
            GameManagerUI.Instance.TxtGameOver.gameObject.SetActive(true);
            string _winnerName = "";
            for(int i = 0; i < ConnectionManager.Instance.participants.Count; i++)
            {
                if(ConnectionManager.Instance.participants[i].ParticipantId != ConnectionManager.Instance.player.DisplayName)
                {
                    _winnerName = ConnectionManager.Instance.participants[i].DisplayName;
                }
            }
            GameManagerUI.Instance.TxtGameOver.text = _winnerName +
                " win!\n" +
                "Total Lap Time: " +
                LapManagerUI.Instance.SecondsToTime(LapManager.Instance.TotalLapTime);
            isGameOver = true;
            StartCoroutine(BackToMainMenu());
        }
        if (LapManager.Instance.CurrentLap >= (totalLaps + 1))
        {
            Debug.Log("Lap Finish");
            GameManagerUI.Instance.PanelCountdownStartGame.SetActive(true);
            GameManagerUI.Instance.TxtGameOver.gameObject.SetActive(true);
            GameManagerUI.Instance.TxtGameOver.text = "You win!\n" +
                "Total Lap Time: " +
                LapManagerUI.Instance.SecondsToTime(LapManager.Instance.TotalLapTime);
            isGameOver = true;
            Social.ReportScore((int)LapManager.Instance.TotalLapTime, "CgkI_tK68fYQEAIQAg", (bool success) => {
            });
            StartCoroutine(BackToMainMenu());
        }
    }

    private void UpdateCarMovement()
    {
        factoryPlayer.Get(0).OnNeutral();
        if (factoryPlayer.Get(0).IsThrottle && !factoryPlayer.Get(0).IsReverse)
            factoryPlayer.Get(0).OnThrottle();
        if (!factoryPlayer.Get(0).IsThrottle && factoryPlayer.Get(0).IsReverse)
            factoryPlayer.Get(0).OnReverse();
    }
    #endregion
}