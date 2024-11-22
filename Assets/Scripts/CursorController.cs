using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] Texture2D cursor;
    [SerializeField] Texture2D cursorClicked;

    private void Awake()
    {
        ChangeCursor(cursor);
    }

    void ChangeCursor(Texture2D cursorType)
    {
        Vector2 hotspot = new Vector2(cursorType.width / 2, cursorType.height / 2);

        Cursor.SetCursor(cursorType, hotspot, CursorMode.Auto);
    }
}
