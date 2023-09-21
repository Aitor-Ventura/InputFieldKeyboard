using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyInputField : InputField
{
    #region Variables
    private bool m_IsDragging = false;
    private int m_DragStartCaretCaretPosition;
    #endregion
    
    #region Properties
    public bool IsDragging
    {
        get => m_IsDragging;
        set => m_IsDragging = value;
    }
    
    public int DragStartCaretPosition
    {
        get => m_DragStartCaretCaretPosition;
        set => m_DragStartCaretCaretPosition = value;
    }
    #endregion
    
    #region Unity Methods
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        IsDragging = true;
        DragStartCaretPosition = caretPosition;
    }
    
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        IsDragging = false;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        DragStartCaretPosition = caretPosition;
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
        
        if (caretPosition != DragStartCaretPosition)
        {
            int startPos = Mathf.Min(caretPosition, DragStartCaretPosition);
            int endPos = Mathf.Max(caretPosition, DragStartCaretPosition);
            
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
    }
    
    public void DeleteCharacter()
    {
        if (caretPosition != m_DragStartCaretCaretPosition)
        {
            int startPos = Mathf.Min(caretPosition, DragStartCaretPosition);
            int endPos = Mathf.Max(caretPosition, DragStartCaretPosition);
            
            string beforeText = text.Substring(0, startPos);
            string afterText = text.Substring(endPos);
            
            text = beforeText + afterText;
            ForceCaretPosition(startPos);
        }
        else
        {
            if (caretPosition <= 0) return;

            string beforeText = text.Substring(0, caretPosition - 1);
            string afterText = text.Substring(caretPosition);
        
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
        DragStartCaretPosition = caretPosition;
        
        UpdateLabel();
    }
    #endregion
}