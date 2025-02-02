using System;
using UnityEngine;

[Serializable]
public class TutorialTower 
{
    public string tname;
    public int tcost;
    public GameObject perfabs;

    public TutorialTower(string _tname, int _tcost, GameObject _prefabs)
    {
        tname = _tname;
        tcost = _tcost;
        perfabs = _prefabs;
    }
}