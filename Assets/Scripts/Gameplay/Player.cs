using BlitheFramework;
using System;
using UnityEngine;
using WayPoint;

public class Player : BaseClass
{
    #region Initialize
    #region EVENT
    public event EventHandler EVENT_CHANGELINE; //dispatchEvent(EVENT_ATTACH, this.gameObject, EventArgs.Empty);
    public event EventHandler EVENT_REMOVE;
    #endregion EVENT

    #region Public_field
    public int IdPlayer
    {
        get { return idPlayer; } set { idPlayer = value; }
    }
    #endregion Public_field

    #region Pivate_field
    int idPlayer;
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
    public void ChangeLine()
    {
        dispatchEvent(EVENT_CHANGELINE, this.gameObject, EventArgs.Empty);
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
        gameObject.GetComponent<PlayerMovement>().UpdatePosition();
    }
    #endregion
}