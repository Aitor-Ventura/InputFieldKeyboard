using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyInputField : InputField
{
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        MoveTextEnd(false);
    }
    
    public override void OnDeselect(BaseEventData eventData)
    {
    }


    public void AddCharacter(string character)
    {
        int insertPos = caretPosition;

        // Check if any text is selected
        if (caretPosition != caretSelectPositionInternal)
        {
            // Delete the selected text first
            int startPos = Mathf.Min(caretPosition, caretSelectPositionInternal);
            int endPos = Mathf.Max(caretPosition, caretSelectPositionInternal);
            string beforeText = text.Substring(0, startPos);
            string afterText = text.Substring(endPos);
            text = beforeText + character + afterText;
            insertPos = startPos;
        }
        else
        {
            string beforeText = text.Substring(0, caretPosition);
            string afterText = text.Substring(caretPosition);
            text = beforeText + character + afterText;
        }

        ForceCaretPosition(insertPos + character.Length);
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

    public void MaintainFocus()
    {
        if (!isFocused)
        {
            ActivateInputField();
            MoveTextEnd(false);
        }
    }

    private void Update()
    {
        MaintainFocus();
    }

    private void ForceCaretPosition(int newPosition)
    {
        caretPosition = newPosition;
        caretSelectPositionInternal = caretPosition;
        
        // Try forcing the update
        UpdateLabel();
    }
}