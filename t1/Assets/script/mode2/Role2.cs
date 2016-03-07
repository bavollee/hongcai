using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AutoRot))]
[RequireComponent(typeof(RunForward))]
public class Role2 : MonoBehaviour
{
    public delegate void ScoreUpdatedCallback(int newestScore);
    public ScoreUpdatedCallback scoreUpdatedCallback;

    private Transform _bombBindPoint;
    private AutoRot _autoRot;
    private RunForward _runForward;

    private int _score = 0;
    public int score
    {
        get { return _score; }
    }


    void Awake()
    {
        _bombBindPoint = transform.Find("bp_bomb");
        _autoRot = gameObject.GetComponent<AutoRot>();
        _runForward = gameObject.GetComponent<RunForward>();
    }

    public void SetIsActivated(bool value)
    {
        _autoRot.enabled = value;
        _runForward.enabled = value;
    }

    public void BindBomb(Bomb bomb)
    {
        Transform tran = bomb.transform;
        tran.parent = _bombBindPoint;
        tran.localPosition = Vector3.zero;
        tran.localRotation = Quaternion.identity;
        tran.localScale = Vector3.one;

        bomb.BindPlayer(this);
    }

    public void AddScore(int add)
    {
        _score += add;
        if (null != scoreUpdatedCallback)
            scoreUpdatedCallback(_score);
    }
}
