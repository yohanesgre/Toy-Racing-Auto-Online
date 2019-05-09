using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlitheFramework;
using WayPoint;

public class GameManager : BaseClass
{
    #region Initialize
    #region EVENT
    public event EventHandler EVENT_REMOVE;
    #endregion EVENT

    #region Public_field
    #endregion Public_field

    #region Pivate_field
    #endregion Pivate_field
    #endregion Initialize

    public override void Init()
    {
        CreateFactoryPlayer();
        factoryPlayer.Add(prefabPlayer, new Vector3(0, 0, 0), Quaternion.identity, 1);
        factoryPlayer.Add(prefabPlayer, new Vector3(1, 1, 0), Quaternion.identity, 2);
        InitLineManagerOnPlayer();
    }

    void Start()
    {
        Init();
    }
    #region factory
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
    void InitLineManagerOnPlayer()
    {
        for (int i = 0; i < factoryPlayer.GetNumberOfObjectFactories(); i++)
        {
            factoryPlayer.Get(i).GetComponent<PlayerMovement>().manager = GameObject.FindGameObjectWithTag("LineManager").GetComponent<WaypointManager>();
            factoryPlayer.Get(i).GetComponent<PlayerMovement>().line = i+1;
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

    private void FixedUpdate()
    {
    }
    public void UpdateMethod()
    {
        for (int i = 0; i < factoryPlayer.GetNumberOfObjectFactories(); i++)
        {
            factoryPlayer.ChangeLineObjectFactories(i);
        }
        //GameObject.Find("Main Camera").GetComponent<Camera>().transform.position = new Vector3 (factoryPlayer.Get(0).gameObject.transform.position.x, factoryPlayer.Get(0).gameObject.transform.position.y, -10);
    }
    #endregion
}