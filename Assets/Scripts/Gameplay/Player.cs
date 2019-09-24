using BlitheFramework;
using GooglePlayGames;
using MessagePack;
using MessagePack.Resolvers;
using System;
using UnityEngine;
using WayPoint;

public class Player : BaseClass
{
    #region Initialize
    #region EVENT
    public event EventHandler EVENT_NEUTRAL; //dispatchEvent(EVENT_NEUTRAL, this.gameObject, EventArgs.Empty);
    public event EventHandler EVENT_REVERSE; //dispatchEvent(EVENT_REVERSE, this.gameObject, EventArgs.Empty);
    public event EventHandler EVENT_THROTTLE; //dispatchEvent(EVENT_GAS, this.gameObject, EventArgs.Empty);
    public event EventHandler EVENT_ONBRAKEUP; //dispatchEvent(EVENT_ONBRAKEUP, this.gameObject, EventArgs.Empty);
    public event EventHandler EVENT_ONBRAKE; //dispatchEvent(EVENT_ONBRAKE, this.gameObject, EventArgs.Empty);
    public event EventHandler EVENT_TURNLEFT; //dispatchEvent(EVENT_TURNLEFT, this.gameObject, EventArgs.Empty);
    public event EventHandler EVENT_TURNRIGHT; //dispatchEvent(EVENT_TURNRIGHT, this.gameObject, EventArgs.Empty);
    public event EventHandler EVENT_REMOVE;
    #endregion EVENT

    #region Public_field
    public string IdPlayer
    {
        get { return idPlayer; } set { idPlayer = value; }
    }

    public bool IsThrottle { get => isThrottle; set => isThrottle = value; }
    public bool IsReverse { get => isReverse; set => isReverse = value; }
    #endregion Public_field

    #region Pivate_field
    string idPlayer;
    bool isThrottle;
    bool isReverse;
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
    public void OnBrake()
    {
        dispatchEvent(EVENT_ONBRAKE, this.gameObject, EventArgs.Empty);
    }
    
    public void TurnLeft()
    {
        dispatchEvent(EVENT_TURNLEFT, this.gameObject, EventArgs.Empty);
    }

    public void TurnRight()
    {
        dispatchEvent(EVENT_TURNRIGHT, this.gameObject, EventArgs.Empty);
    }

    public void OnThrottle()
    {
        dispatchEvent(EVENT_THROTTLE, this.gameObject, EventArgs.Empty);
    }

    public void OnReverse()
    {
        dispatchEvent(EVENT_REVERSE, this.gameObject, EventArgs.Empty);
    }

    public void OnNeutral()
    {
        dispatchEvent(EVENT_NEUTRAL, this.gameObject, EventArgs.Empty);
    }

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

        /**Update gerakan player
         * 
         * struktur bisa diganti2
         * yg ini index 0 participant id dr googleplay
         * 1 - 3 posisi xyz
         * 4 - 6 rotasi xyz
         * 7 lapnya skrng
         * 8 bestlaptime
         * 
         * trus array object diserialize jd byte utk dikirim ke semua
        */
        var _obj = new object[] { "Player Movement",
            ConnectionManager.Instance.player.ParticipantId,
            transform.GetChild(0).position.x,
            transform.GetChild(0).position.y,
            transform.GetChild(0).position.z,
            transform.GetChild(0).eulerAngles.x,
            transform.GetChild(0).eulerAngles.y,
            transform.GetChild(0).eulerAngles.z,
            LapManager.Instance.CurrentLap,
            LapManager.Instance.BestLapTime
        };
        var _byte = MessagePackSerializer.Serialize(_obj, ContractlessStandardResolver.Instance);
        PlayGamesPlatform.Instance.RealTime.SendMessageToAll(false, _byte);
    }
    #endregion
}