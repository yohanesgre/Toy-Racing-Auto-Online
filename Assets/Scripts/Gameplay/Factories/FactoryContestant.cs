using System;
using System.Collections.Generic;
using UnityEngine;
using BlitheFramework;

public class FactoryContestant: BaseClass
{
    #region EVENT
    public event EventHandler EVENT_REMOVE;
    #endregion EVENT
    private List<Contestant> listOfObjetFactories;
    public FactoryContestant()
    {
        listOfObjetFactories = new List<Contestant>();
        Init();
    }
    public override void Init()
    {

    }

    public void Add(GameObject _object, Vector3 _position, Quaternion _rotation, string _idPlayer)
    {
        Contestant contestant = new Contestant();
        contestant = Instantiate(_object, _position, _rotation).AddComponent<Contestant>() as Contestant;
        #region EVENT_LISTENER_ADD_Contestant
        contestant.GetComponent<Contestant>().IdPlayer = _idPlayer;
        contestant.GetComponent<Contestant>().EVENT_REMOVE += Remove;
        #endregion EVENT_LISTENER_ADD_Contestant
        listOfObjetFactories.Add(contestant);
    }
    public Contestant Get(int _indexObjectOnList)
    {
        return (listOfObjetFactories[_indexObjectOnList]) as Contestant;
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
    private void Remove(object _sender, EventArgs e)
    {
        GameObject sender = (GameObject)_sender;
        listOfObjetFactories.Remove(sender.GetComponent<Contestant>());
        #region EVENT_LISTENER_REMOVE_Contestant
        sender.GetComponent<Contestant>().EVENT_REMOVE -= Remove;
        #endregion EVENT_LISTENER_REMOVE_Contestant
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