using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyInputField : InputField
{
    #region Variables
    private bool m_IsDragging = false;
    private int m_DragCaretSelectionSelectionPosition;
    #endregion
    
    #region Properties
    public bool IsDragging
    {
        get => m_IsDragging;
        set => m_IsDragging = value;
    }
    
    public int DragCaretSelectionPosition
    {
        get => m_DragCaretSelectionSelectionPosition;
        set => m_DragCaretSelectionSelectionPosition = value;
    }
    #endregion
    
    #region Unity Methods
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        IsDragging = true;
        DragCaretSelectionPosition = caretPosition;
    }
    
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        IsDragging = false;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        DragCaretSelectionPosition = caretPosition;
    }

    public override void OnDeselect(BaseEventData eventData)
    {
    }
    
    private void Update()
    {
        MaintainFocus();
    }
    #endregion

    
    #region Public Methods
    public void AddCharacter(string character)
    {
        int insertPos = caretPosition;
        string beforeText, afterText;
        
        if (caretPosition != DragCaretSelectionPosition)
        {
            int startPosition = Mathf.Min(caretPosition, DragCaretSelectionPosition);
            int endPosition = Mathf.Max(caretPosition, DragCaretSelectionPosition);
            
            beforeText = text.Substring(0, startPosition);
            afterText = text.Substring(endPosition);
            
            text = beforeText + character + afterText;
            insertPos = startPosition;
        }
        else
        {
            beforeText = text.Substring(0, caretPosition);
            afterText = text.Substring(caretPosition);
            
            text = beforeText + character + afterText;
        }

        ForceCaretPosition(insertPos + character.Length);
    }
    
    public void DeleteCharacter()
    {
        string beforeText, afterText;
        
        if (caretPosition != m_DragCaretSelectionSelectionPosition)
        {
            int startPosition = Mathf.Min(caretPosition, DragCaretSelectionPosition);
            int endPosition = Mathf.Max(caretPosition, DragCaretSelectionPosition);
            
            beforeText = text.Substring(0, startPosition);
            afterText = text.Substring(endPosition);
            
            text = beforeText + afterText;
            ForceCaretPosition(startPosition);
        }
        else
        {
            if (caretPosition <= 0) return;

            beforeText = text.Substring(0, caretPosition - 1);
            afterText = text.Substring(caretPosition);
        
            ForceCaretPosition(caretPosition - 1);
        
            text = beforeText + afterText;
        }
    }


    public void MoveCaretLeft()
    {
        if (caretPosition <= 0) return;
        
        ForceCaretPosition(caretPosition - 1);
    }
    
    public void MoveCaretRight()
    {
        if (caretPosition >= text.Length) return;

        ForceCaretPosition(caretPosition + 1);
    }

    public void MaintainFocus()
    {
        if (!isFocused)
        {
            ActivateInputField();
        }
    }
    #endregion

    #region Private Methods
    private void ForceCaretPosition(int newPosition)
    {
        caretPosition = newPosition;
        DragCaretSelectionPosition = caretPosition;
        
        UpdateLabel();
    }
    #endregion
}