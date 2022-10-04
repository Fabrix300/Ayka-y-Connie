using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveDataObj
{
    public GameLevel[] gameLevelList;

    public SaveDataObj(GameLevel[] _gameLevelList)
    {
        gameLevelList = _gameLevelList;
    }
}
