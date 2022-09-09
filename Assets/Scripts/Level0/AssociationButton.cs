using UnityEngine;

public enum AssociationButtonType
{
    imageButton,
    textButton
}

[System.Serializable]
public class AssociationButton
{
    public Sprite buttonImage;
    public string buttonText;

    public AssociationButton (Sprite _buttonImage, string _buttonText) 
    {
        buttonImage = _buttonImage;
        buttonText = _buttonText;
    }
}
