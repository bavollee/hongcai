using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    public GameObject titleGO;
    public Transform head;
    public float countDown = 10f;

    public delegate void BombCallback(Bomb bomb, Role2 target);
    public BombCallback bombCallback;

    public delegate void PassBombCallback(Bomb bomb, Role2 from, Role2 to);
    public PassBombCallback passBombCallback;

    private Transform uiCamera;
    private UILabel _bombTitle;
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
            _bombTitle.text = string.Format("{0:F}", _time);
        }
    }

    private bool _bRun = false;

    private Role2 _player = null;
    public Role2 player
    {
        get { return _player; }
    }

    private static readonly float _TouchCD = 1f;
    private float _lastTouchTime = 0f;


    void Start()
    {
        uiCamera = GameObject.Find("UI Root/2D Camera").transform;

        GameObject bombTitleGO = Instantiate(titleGO.gameObject) as GameObject;
        bombTitleGO.transform.parent = uiCamera.transform;
        bombTitleGO.transform.localPosition = Vector3.zero;
        bombTitleGO.transform.localRotation = Quaternion.identity;
        bombTitleGO.transform.localScale = Vector3.one;

        _bombTitle = bombTitleGO.GetComponent<UILabel>();

        _titleTran = _bombTitle.transform;
        _time = countDown;

        // 一开始即可传递
        if (Time.realtimeSinceStartup - _lastTouchTime < _TouchCD)
            _lastTouchTime = Time.realtimeSinceStartup - _TouchCD;
    }

    void Update()
    {
        _titleTran.position = WorldToUI(_player.transform.position);
        //_titleTran.position = WorldToUI(head.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Time.realtimeSinceStartup - _lastTouchTime < _TouchCD)
                return;
            _lastTouchTime = Time.realtimeSinceStartup;

            Role2 player = other.gameObject.GetComponent<Role2>();
            if (null != passBombCallback)
                passBombCallback(this, _player, player);
            player.BindBomb(this);

            Vector3 point = (other.gameObject.transform.position + _player.transform.position) / 2;
            if (GameMgr.instance)
            {
                GameMgr.instance.addEffect(point);
                GameMgr.instance.playAudio();
            }
        }
    }

    public void BindPlayer(Role2 player)
    {
        _player = player;
    }

    public void Run()
    {
        StartCoroutine(OnRun());
    }

    private IEnumerator OnRun()
    {
        _bRun = true;

        _time = countDown > 0 ? countDown : 0;

        while (_time > 0)
        {
            time = _time >= 0 ? _time : 0;
            yield return null;
            _time -= Time.deltaTime;
        }
        time = 0;
        OnBomb();

        yield return true;
    }

    private void OnBomb()
    {
        _bRun = false;

        if (null != bombCallback)
            bombCallback(this, _player);

        Debug.LogWarning("booooooooomp!!");
        Destroy(_bombTitle.gameObject);
        Destroy(gameObject);
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
