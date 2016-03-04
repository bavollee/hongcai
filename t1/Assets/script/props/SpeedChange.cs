using UnityEngine;
using System.Collections;


public class SpeedChange : PropEffect
{
    public float duration = 8f; // 效果持续时间
    public float coef = 1f; // 变速系数

    void Start()
    {
    }

    protected override void OnTriggerPropEffect(GameObject target)
    {
        SpeedChangeBuff buff = target.GetComponent<SpeedChangeBuff>();
        if (null == buff)
        {
            buff = target.AddComponent<SpeedChangeBuff>();
        }
        else
        {
            buff.time = 0; // 重置生效时间
        }
        buff.duration = duration;
        buff.ChangeSpeed(coef);

        Destroy(gameObject);
    }
}



public class SpeedChangeBuff : MonoBehaviour
{
    public float duration = 8f;
    public float time = 0f;
    public float coef = 1f;

    private Move _moveCom = null;


    protected virtual void Awake()
    {
        _moveCom = gameObject.GetComponent<Move>();
        time = 0;
    }

    public void ChangeSpeed(float c)
    {
        coef = c;
        if (null != _moveCom)
        {
            _moveCom.force = Move.initForce * coef;
            Debug.Log(string.Format("{0}的速度变为{1}", _moveCom.gameObject.name, _moveCom.force));
        }
    }

    protected virtual void Update()
    {
        if (null == _moveCom)
            return;

        if (time < duration)
        {
            time += Time.deltaTime;
        }
        else
        {
            if (null != _moveCom)
            {
                _moveCom.force = Move.initForce;
                _moveCom = null;
            }

            Debug.Log(string.Format("变速（coef: {0}）效果已失效", coef));
            Destroy(this);
        }
    }
}