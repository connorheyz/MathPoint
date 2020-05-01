using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanvas
{
    void Clear();
    void Destroy();
    void DrawText(string text, Rect position, float fontsize = 12f);
}
