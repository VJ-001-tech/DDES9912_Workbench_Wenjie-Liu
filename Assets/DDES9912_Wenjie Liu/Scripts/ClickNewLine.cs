using UnityEngine;

public class ClickNewLine : ClickBase
{
    public override void OnMouseDownClick()
    {
        WriteLine.instance.RunNewLine();
    }

}
