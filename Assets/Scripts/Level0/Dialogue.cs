using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    public string characterName;
    public Sprite characterImage;

    public string[] sentences;

    public Dialogue(string _characterName, Sprite _characterImage, string[] _sentences)
    {
        characterName = _characterName;
        characterImage = _characterImage;
        sentences = _sentences;
    }
}
