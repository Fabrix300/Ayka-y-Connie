using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AssociationImageButton : AssociationButton
{
    public Sprite buttonImage;

    public AssociationImageButton(int _associationButtonPair, Sprite _buttonImage) : base(_associationButtonPair)
    {
        buttonImage = _buttonImage;
    }
}
