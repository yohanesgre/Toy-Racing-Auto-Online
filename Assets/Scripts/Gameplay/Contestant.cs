using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlitheFramework;
public class Contestant : BaseClass
{
    #region Initialize
    #region EVENT
    public event EventHandler EVENT_REMOVE;
    #endregion EVENT

    #region Public_field
    public string IdPlayer
    {
        get { return idPlayer; }
        set { idPlayer = value; }
    }
    public float BestLapTime { get => bestLapTime; set => bestLapTime = value; }
    public int CurrentLap { get => currentLap; set => currentLap = value; }
    #endregion Public_field

    #region Pivate_field
    string idPlayer;
    float bestLapTime;
    int currentLap;
    #endregion Pivate_field
    #endregion Initialize

    public override void Init()
    {

    }

    void Start()
    {

    }
    #region factory
    #region EVENT_LISTENER_ADD
    #endregion EVENT_LISTENER_ADD
    #region EVENT_LISTENER_METHOD
    #endregion EVENT_LISTENER_METHOD
    #endregion factory
    #region private method
    #endregion
    #region public method
    public void Remove()
    {
       dispatchEvent(EVENT_REMOVE, this.gameObject, EventArgs.Empty);
    }
    #endregion
    #region update
    public void FixedUpdate()
    {
        UpdateMethod();
    }

    public void UpdateMethod()
    {
        UpdatePartcipantMovement();
    }

    private void UpdatePartcipantMovement()
    {
        // mengupdate object musuh/contestant pada client pemain sesuai index yg diterima dr connectionmanager
        GameManagerUI.Instance.TxtDebugger2.text = "UpdateParticipantMovement";
        if ((string)ConnectionManager.Instance.dataReceived[0] == "Player Movement")
        {
            GameManagerUI.Instance.TxtDebugger2.text = "Type: Movement Player";
            if ((string)ConnectionManager.Instance.dataReceived[1] == idPlayer)
            {
                GameManagerUI.Instance.TxtDebugger2.text = "Contestant ID: " + idPlayer;
                transform.position = new Vector3((float)ConnectionManager.Instance.dataReceived[2], (float)ConnectionManager.Instance.dataReceived[3], (float)ConnectionManager.Instance.dataReceived[4]);
                transform.eulerAngles = new Vector3((float)ConnectionManager.Instance.dataReceived[5], (float)ConnectionManager.Instance.dataReceived[6], (float)ConnectionManager.Instance.dataReceived[7]);
                currentLap = (int)ConnectionManager.Instance.dataReceived[8];
                bestLapTime = (int)ConnectionManager.Instance.dataReceived[9];
            }
        }
    }
    #endregion
}