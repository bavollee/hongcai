using UnityEngine;
using System.Collections;

public class TurnBack : PropEffect
{
    protected override void OnTriggerPropEffect(GameObject target)
    {
        Debug.Log(string.Format("【{0}】触发道具效果：反方向走", target.name));

        Move moveCom = target.GetComponent<Move>();
        if (null != moveCom)
        {
            moveCom.isForward = !moveCom.isForward;
        }
    }
}
