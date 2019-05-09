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

    public void Add(GameObject _object, Vector3 _position, Quaternion _rotation, int _idPlayer)
    {
        Player player = new Player();
        player = Instantiate(_object, _position, _rotation).AddComponent<Player>() as Player;
        #region EVENT_LISTENER_ADD_Player
        player.GetComponent<Player>().IdPlayer = _idPlayer;
        player.GetComponent<Player>().EVENT_CHANGELINE += OnChangeLine;
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
    public void ChangeLineObjectFactories(int _indexObjectOnList)
    {
        Get(_indexObjectOnList).ChangeLine();
    }
    public int GetNumberOfObjectFactories()
    {
       return listOfObjetFactories.Count;
    }
    #region EVENT_LISTENER_METHOD
    private void OnChangeLine(object _sender, EventArgs e)
    {
        GameObject sender = (GameObject)_sender;
        var playerMov = sender.GetComponent<PlayerMovement>();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("IdPlayer: " + sender.GetComponent<Player>().IdPlayer);
        }
        if (playerMov.line > 0 && playerMov.line < 5)
        {
            Debug.Log(playerMov.line);
            if (Input.GetKeyDown(KeyCode.D))
            {
                playerMov.ChangeLine(true);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                playerMov.ChangeLine(false);
            }
        }
    }

    private void Remove(object _sender, EventArgs e)
    {
        GameObject sender = (GameObject)_sender;
        listOfObjetFactories.Remove(sender.GetComponent<Player>());
        #region EVENT_LISTENER_REMOVE_Player
        sender.GetComponent<Player>().EVENT_CHANGELINE -= OnChangeLine;
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