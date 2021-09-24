using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class SwitchData
{
    public string name;
    public bool on;
    public bool off;


    public SwitchData(string _name, bool _on, bool _off)
    {
        name = _name;
        on = _on;
        off = _off;
    }

}
