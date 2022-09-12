using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AssociationTextButton : AssociationButton
{
    public string buttonText;

    public AssociationTextButton(int _associationButtonPair, string _buttonText) : base(_associationButtonPair)
    {
        buttonText = _buttonText;
    }
}
