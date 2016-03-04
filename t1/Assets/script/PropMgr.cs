using UnityEngine;
using System.Collections;

public class PropMgr : MonoBehaviour
{
    public GameObject scope;
    public PropEffect[] props;
    public float timeInterval = 3f;

    private Bounds _scopeBounds;
    private float lastTime = 0f;


    void Awake()
    {
        _scopeBounds = scope.GetComponent<MeshFilter>().mesh.bounds;
    }

    void Update()
    {
        if (lastTime >= timeInterval)
        {
            GenProp();
            lastTime = 0f;
        }
        else
        {
            lastTime += Time.deltaTime;
        }
    }

    private void GenProp()
    {
        if (props.Length <= 0)
        {
            Debug.Log("未指定道具");
            return;
        }

        PropEffect prop = props[Random.Range(0, props.Length)];
        float posx = Random.Range(_scopeBounds.min.x, _scopeBounds.max.x);
        float posz = Random.Range(_scopeBounds.min.y, _scopeBounds.max.y);
        GameObject propGO = Instantiate(prop.gameObject, new Vector3(posx, 0, posz), Quaternion.identity) as GameObject;
        Debug.Log(string.Format("产生道具：{0}", propGO.name));
    }
}
