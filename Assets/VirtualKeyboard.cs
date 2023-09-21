using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class VirtualKeyboard : MonoBehaviour 
{
    public enum KeyType
    {
        None,
        Character,
        Left,
        Right,
        Backspace
    }
    
    #region Variables
    public MyInputField InputField;
    
    [Tooltip("Text object to display the current index of the caret.")]
    [SerializeField] private Text caretIndexCaretIndexText;

    [Tooltip("How long to wait before repeating a key press.")]
    [SerializeField] private float m_KeyRepeatDelay = 0.5f;
    
    private KeyType m_HeldKey = KeyType.None;
    private string m_HeldCharacter = "";
    private float m_HoldTimer = 0.0f;
    #endregion
    
    #region Properties
    public Text CaretIndexText
    {
        get => caretIndexCaretIndexText;
        set => caretIndexCaretIndexText = value;
    }
    
    public KeyType HeldKey
    {
        get => m_HeldKey;
        set => m_HeldKey = value;
    } 
    
    public string HeldCharacter
    {
        get => m_HeldCharacter;
        set => m_HeldCharacter = value;
    }
    
    public float HoldTimer
    {
        get => m_HoldTimer;
        set => m_HoldTimer = value;
    }
    
    public float KeyRepeatDelay
    {
        get => m_KeyRepeatDelay;
        set => m_KeyRepeatDelay = value;
    }
    #endregion
    
    #region Unity Methods
    private void Update()
    {
        CheckKeyPress();
        DisplayCaretIndex();
    }
    
    #if UNITY_EDITOR
    public void OnValidate()
    {
        if (KeyRepeatDelay < 0.0f)
        {
            Debug.LogWarning("Key repeat delay cannot be less than 0.0f. Set to 0.0f.", this);
            KeyRepeatDelay = 0.0f;
        }
    }
    #endif
    #endregion

    #region Public Methods
    public void KeyPress(string c)
    {
        InputField.AddCharacter(c);
        
        HeldKey = KeyType.Character;
        HeldCharacter = c;
        HoldTimer = KeyRepeatDelay;
    }

    public void KeyRelease()
    {
        HeldKey = KeyType.None;
    }

    public void KeyLeft()
    {
        InputField.MoveCaretLeft();
        
        HeldKey = KeyType.Left;
        HoldTimer = KeyRepeatDelay;
    }

    public void KeyRight()
    {
        InputField.MoveCaretRight();
        
        HeldKey = KeyType.Right;
        HoldTimer = KeyRepeatDelay;
    }

    public void KeyDelete()
    {
        InputField.DeleteCharacter();
        
        HeldKey = KeyType.Backspace;
        HoldTimer = KeyRepeatDelay;
    }
    #endregion
    
    #region Private Methods

    private void CheckKeyPress()
    {
        if (HeldKey == KeyType.None) return;
        
        HoldTimer -= Time.deltaTime;
        if (HoldTimer > 0.0f) return;

        switch (HeldKey)
        {
            case KeyType.Character:
                InputField.AddCharacter(HeldCharacter);
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
        
        HoldTimer = KeyRepeatDelay;
    }
    
    private void DisplayCaretIndex()
    {
        if (CaretIndexText == null) return;

        CaretIndexText.text = $"Current Caret Index: {InputField.caretPosition} \n" +
                              $"Caret Selection Index: {InputField.DragCaretSelectionPosition}";
    }
    #endregion
}
