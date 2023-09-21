using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualKeyboard : MonoBehaviour {

    public MyInputField InputField;

    private enum KeyType
    {
        None,
        Character,
        Left,
        Right,
        Backspace
    }

    private KeyType heldKey = KeyType.None;
    private string heldCharacter = "";
    private float holdTimer = 0.0f;
    private float keyRepeatDelay = 0.5f;
    private float keyRepeatRate = 0.1f;

    private void Update()
    {
        if (heldKey == KeyType.None) return;
        
        holdTimer -= Time.deltaTime;
        if (holdTimer > 0.0f) return;

        switch (heldKey)
        {
            case KeyType.Character:
                InputField.AddCharacter(heldCharacter);
                break;
            case KeyType.Left:
                InputField.MoveCaretLeft();
                break;
            case KeyType.Right:
                InputField.MoveCaretRight();
                break;
            case KeyType.Backspace:
                InputField.DeleteCharacter();
                break;
        }
        
        holdTimer = keyRepeatRate;
    }

    public void KeyPress(string c)
    {
        InputField.AddCharacter(c);
        
        heldKey = KeyType.Character;
        heldCharacter = c;
        holdTimer = keyRepeatDelay;
    }

    public void KeyRelease()
    {
        heldKey = KeyType.None;
    }

    public void KeyLeft()
    {
        InputField.MoveCaretLeft();
        
        heldKey = KeyType.Left;
        holdTimer = keyRepeatDelay;
    }

    public void KeyRight()
    {
        InputField.MoveCaretRight();
        
        heldKey = KeyType.Right;
        holdTimer = keyRepeatDelay;
    }

    public void KeyDelete()
    {
        InputField.DeleteCharacter();
        
        heldKey = KeyType.Backspace;
        holdTimer = keyRepeatDelay;
    }
}
