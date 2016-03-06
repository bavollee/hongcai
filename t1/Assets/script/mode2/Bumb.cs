using UnityEngine;
using System.Collections;

public class Bumb : MonoBehaviour
{
    public Transform uiCamera;
    public GameObject titleGO;
    public Transform head;
    public float countDown = 10f;

    private UILabel _bumbTitle;
    private Transform _titleTran;
    private float _time = 0;
    private float time
    {
        get
        {
            return _time;
        }

        set
        {
            _time = value;
            _bumbTitle.text = string.Format("{0:F}", _time);
        }
    }


    void Start()
    {
        GameObject bumbTitleGO = Instantiate(titleGO.gameObject) as GameObject;
        bumbTitleGO.transform.parent = uiCamera.transform;
        bumbTitleGO.transform.localPosition = Vector3.zero;
        bumbTitleGO.transform.localRotation = Quaternion.identity;
        bumbTitleGO.transform.localScale = Vector3.one;

        _bumbTitle = bumbTitleGO.GetComponent<UILabel>();

        _titleTran = _bumbTitle.transform;
        _time = countDown;
    }

    void Update()
    {
        _titleTran.position = WorldToUI(head.position);

        if (0 != _time)
        {
            if (_time > 0)
            {
                _time -= Time.deltaTime;
                time = _time >= 0 ? _time : 0;
            }

            if (0 == time)
            {
                OnBumb();
            }
        }
    }

    private void OnBumb()
    {
        Debug.LogWarning("buuuuuuuuump!!");
    }

    private static Vector3 WorldToUI(Vector3 point)
    {
        if (null == UICamera.currentCamera)
            return Vector3.zero;

        Vector3 pt = Camera.main.WorldToScreenPoint(point);
        Vector3 ff = UICamera.currentCamera.ScreenToWorldPoint(pt);
        ff.z = 0;
        return ff;
    }
}
