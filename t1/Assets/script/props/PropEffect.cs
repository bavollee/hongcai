using UnityEngine;
using System.Collections;

public class PropEffect : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        OnTriggerPropEffect(other.gameObject);
        Destroy(gameObject);
    }

    protected virtual void OnTriggerPropEffect(GameObject target)
    {
        Debug.Log(string.Format("【{0}】触发道具效果", target.name));
    }
}
