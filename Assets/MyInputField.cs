using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class MyInputField : InputField
{
    protected override void Start()
    {
        base.Start();
        onEndEdit.AddListener(DeactivateEdit);
    }

    public void AddCharacter(string character)
    {
        
        string beforeText = text.Substring(0, caretPosition);
        string afterText = text.Substring(caretPosition);
        text = beforeText + character + afterText;

        ForceCaretPosition(caretPosition + character.Length);
        
        Debug.Log("After adding a letter index: " + caretPosition);
    }


    public void DeleteCharacter()
    {
        Debug.Log("Before removing a letter index: " + caretPosition);
        
        if (caretPosition <= 0) return;

        string beforeText = text.Substring(0, caretPosition - 1);
        string afterText = text.Substring(caretPosition);
        
        ForceCaretPosition(caretPosition - 1);
        
        text = beforeText + afterText;

        Debug.Log("After removing a letter index: " + caretPosition);
    }

    public void MoveCaretLeft()
    {
        if (caretPosition <= 0) return;
        
        ForceCaretPosition(caretPosition - 1);
        Debug.Log("After moving caret left index: " + caretPosition);
    }
    
    public void MoveCaretRight()
    {
        if (caretPosition >= text.Length) return;

        ForceCaretPosition(caretPosition + 1);
        Debug.Log("After moving caret right index: " + caretPosition);
    }

    private void ForceCaretPosition(int newPosition)
    {
        caretPosition = newPosition;
        caretSelectPositionInternal = caretPosition;
        
        // Try forcing the update
        UpdateLabel();
    }

    public void DeactivateEdit(string text)
    {
        DeactivateInputField();
    }
}