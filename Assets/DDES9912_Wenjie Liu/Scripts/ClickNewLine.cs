using UnityEngine;

public class ClickNewLine : ClickBase
{
    public TypeWriter typeWrite;

    public override void OnMouseDownClick()
    {
        typeWrite.RunNewLine();
    }

}
