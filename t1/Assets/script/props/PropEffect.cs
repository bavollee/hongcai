using UnityEngine;
using System.Collections;

public class PropEffect : MonoBehaviour
{
    public float lifeTime = 0;

    private float _bornTime = 0f;

    void Start()
    {
        //...
    }

    protected virtual void Update()
    {
        if (0 != lifeTime)
        {
            if (_bornTime < lifeTime)
            {
                _bornTime += Time.deltaTime;
            }
            else
            {
                Debug.Log("效果道具消失");
                Destroy(gameObject);
            }
        }
    }



    void OnTriggerEnter(Collider other)
    {
        OnTriggerPropEffect(other.gameObject);
    }

    protected virtual void OnTriggerPropEffect(GameObject target)
    {
        Debug.Log(string.Format("【{0}】触发道具效果", target.name));
        Destroy(gameObject);
    }
}
