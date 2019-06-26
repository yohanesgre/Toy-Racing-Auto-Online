using System;
using System.Collections.Generic;
using UnityEngine;
using BlitheFramework;
using WayPoint;
using DG.Tweening;

public class FactoryPlayer: BaseClass
{
    #region EVENT
    public event EventHandler EVENT_REMOVE;
    #endregion EVENT
    private List<Player> listOfObjetFactories;
    public FactoryPlayer()
    {
        listOfObjetFactories = new List<Player>();
        Init();
    }
    public override void Init()
    {

    }

    public void Add(GameObject _object, Vector3 _position, Quaternion _rotation, string _idPlayer)
    {
        Player player = new Player();
        player = Instantiate(_object, _position, _rotation).AddComponent<Player>() as Player;
        #region EVENT_LISTENER_ADD_Player
        player.GetComponent<Player>().EVENT_NEUTRAL += OnNeutral;
        player.GetComponent<Player>().EVENT_REVERSE += OnReverse;
        player.GetComponent<Player>().IdPlayer = _idPlayer;
        player.GetComponent<Player>().EVENT_TURNLEFT += OnTurnleft;
        player.GetComponent<Player>().EVENT_TURNRIGHT += OnTurnright;
        player.GetComponent<Player>().EVENT_ONBRAKE += Onbrake;
        player.GetComponent<Player>().EVENT_THROTTLE += OnThrottle;
        player.GetComponent<Player>().EVENT_REMOVE += Remove;
        #endregion EVENT_LISTENER_ADD_Player
        listOfObjetFactories.Add(player);
    }
    public Player Get(int _indexObjectOnList)
    {
        return (listOfObjetFactories[_indexObjectOnList]) as Player;
    }
    public void RemoveObjectFactories(int _indexObjectOnList)
    {
       Get(_indexObjectOnList).Remove();
    }
    public int GetNumberOfObjectFactories()
    {
       return listOfObjetFactories.Count;
    }
    #region EVENT_LISTENER_METHOD
    private void OnNeutral(object _sender, EventArgs e)
    {
        GameObject sender = (GameObject)_sender;
        var _player = sender.GetComponent<Player>();
        var _car = sender.GetComponent<Car>();
        _car.SetterThrottle(0f);
    }

    private void OnReverse(object _sender, EventArgs e)
    {
        GameObject sender = (GameObject)_sender;
        var _player = sender.GetComponent<Player>();
        var _car = sender.GetComponent<Car>();
        _car.SetterThrottle(-1f);
    }

    private void OnThrottle(object _sender, EventArgs e)
    {
        GameObject sender = (GameObject)_sender;
        var _player = sender.GetComponent<Player>();
        var _car = sender.GetComponent<Car>();
        _car.SetterThrottle(1f);
    }
    

    private void Onbrake(object _sender, EventArgs e)
    {
        GameObject sender = (GameObject)_sender;
        //playerMov.speed -= 0.05f;
    }

    private void OnTurnleft(object _sender, EventArgs e)
    {
        GameObject sender = (GameObject)_sender;
        var player = sender.GetComponent<Player>();
        sender.transform.GetChild(0).position = sender.transform.GetChild(0).transform.position + new Vector3(0, 0, 0.1f);
    }

    private void OnTurnright(object _sender, EventArgs e)
    {
        GameObject sender = (GameObject)_sender;
        var player = sender.GetComponent<Player>();
        sender.transform.GetChild(0).position = sender.transform.GetChild(0).transform.position + new Vector3(0, 0, -0.1f);
    }

    private void Remove(object _sender, EventArgs e)
    {
        GameObject sender = (GameObject)_sender;
        listOfObjetFactories.Remove(sender.GetComponent<Player>());
        #region EVENT_LISTENER_REMOVE_Player
        sender.GetComponent<Player>().EVENT_NEUTRAL -= OnNeutral;
        sender.GetComponent<Player>().EVENT_REVERSE -= OnReverse;
        sender.GetComponent<Player>().EVENT_THROTTLE -= OnThrottle;
        sender.GetComponent<Player>().EVENT_ONBRAKE -= Onbrake;
        sender.GetComponent<Player>().EVENT_TURNLEFT -= OnTurnleft;
        sender.GetComponent<Player>().EVENT_TURNRIGHT -= OnTurnright;
        sender.GetComponent<Player>().EVENT_REMOVE -= Remove;
        #endregion EVENT_LISTENER_REMOVE_Player
        Destroy(sender);
    }
    #endregion EVENT_LISTENER_METHOD
    private void RemoveAllObjectFactories()
    {
        while (GetNumberOfObjectFactories() != 0)
        {
            RemoveObjectFactories(0);
        }
    }
    #region public method

    public void Remove()
    {
       RemoveAllObjectFactories();
       dispatchEvent(EVENT_REMOVE, this.gameObject, EventArgs.Empty);
    }
    #endregion
}