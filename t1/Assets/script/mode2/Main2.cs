using UnityEngine;
using System.Collections;

public class Main2 : MonoBehaviour
{
    public GameObject startBtn;
    public InputMgr[] inputs;
    public Role2[] players;
    public Bomb bombPrefab;
    public UILabel tips;
    public GameObject resetBtn;

    private Bomb _curBomb = null;
    private Role2 _lastPassedBombPlayer = null;

    public int passAddScore = 0; // 目前传递不给分
    public int bombAddScore = 5;
    public int bombSubScore = -3;

    public float gameTime = 60f;
    private float _gameTimeRemain = 0f;

    private bool _bStartGame = false;


    void Awake()
    {
        SetIsInputEnabled(false);
        SetIsPlayersActivated(false);

        tips.text = "请按Start按钮开始游戏";

        UIEventListener.Get(startBtn).onClick = OnStartBtnClick;
        UIEventListener.Get(resetBtn).onClick = OnResetBtnClick;

        resetBtn.SetActive(false);

        Time.timeScale = 0;
    }

    private void SetIsInputEnabled(bool value)
    {
        for (int i = 0; i < inputs.Length; ++i)
        {
            inputs[i].enabled = value;
        }
    }

    private void SetIsPlayersActivated(bool value)
    {
        for (int i = 0; i < players.Length; ++i)
        {
            players[i].SetIsActivated(value);
        }
    }

    private void OnStartBtnClick(GameObject go)
    {
        startBtn.SetActive(false);
        StartCoroutine(OnStartGame());
    }

    private void OnResetBtnClick(GameObject go)
    {
        Application.LoadLevel("game2");
    }

    private IEnumerator OnStartGame()
    {
        Time.timeScale = 1;

        _curBomb = CreateBomb();

        yield return StartCoroutine(OnSelectPlayer());

        if (!_bStartGame)
        {
            yield return new WaitForSeconds(1f);

            SetIsInputEnabled(true);
            SetIsPlayersActivated(true);

            StartCoroutine(OnCountDown());

            _bStartGame = true;
        }

        _curBomb.Run();

        yield return true;
    }

    private IEnumerator OnCountDown()
    {
        _gameTimeRemain = gameTime;
        while (_gameTimeRemain > 0)
        {
            _gameTimeRemain -= Time.deltaTime;
            tips.text = string.Format("{0:N0}", _gameTimeRemain);
            yield return null;
        }
        _gameTimeRemain = 0;
        tips.text = string.Format("{0:N0}", _gameTimeRemain);

        OnGameOver();

        yield return true;
    }

    private void OnGameOver()
    {
        Time.timeScale = 0;

        int topScore = int.MinValue;
        int topScorePlayerNum = 0;
        InputMgr topScorePlayerInput = null;
        for (int i = 0; i < inputs.Length; ++i)
        {
            InputMgr input = inputs[i];
            Role2 player = input.playerGO.GetComponent<Role2>();
            int score = player.score;
            if (score > topScore)
            {
                topScore = score;
                topScorePlayerNum = 1;
                topScorePlayerInput = input;
            }
            else if (score == topScore)
            {
                ++topScorePlayerNum;
            }
        }

        if (1 == topScorePlayerNum)
        {
            tips.text = string.Format("胜者：{0}", topScorePlayerInput.playerName);
        }
        else
        {
            tips.text = "~不分胜负~";
        }
        _bStartGame = false;

        resetBtn.SetActive(true);
    }

    private Bomb CreateBomb()
    {
        GameObject bombGO = Instantiate(bombPrefab.gameObject) as GameObject;
        Bomb bomb = bombGO.GetComponent<Bomb>();
        bomb.passBombCallback += OnPassBombCallback;
        bomb.bombCallback += OnBomb;
        return bomb;
    }

    private void OnPassBombCallback(Bomb bomb, Role2 from, Role2 to)
    {
        _lastPassedBombPlayer = from;
        from.AddScore(passAddScore);
    }

    private void OnBomb(Bomb bomb, Role2 target)
    {
        if (null != _lastPassedBombPlayer)
        {
            _lastPassedBombPlayer.AddScore(bombAddScore);
            _lastPassedBombPlayer = null;

            target.AddScore(bombSubScore);
        }

        StartCoroutine(OnNextRound());
    }

    private IEnumerator OnNextRound()
    {
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(OnStartGame());
    }

    IEnumerator OnSelectPlayer()
    {
        const float countDown = 5f;

        float stayTime = Random.Range(0.2f, 0.6f);
        float time = countDown;
        int idx = 0;
        while (time > 0)
        {
            Role2 player = players[idx];
            player.BindBomb(_curBomb);

            yield return new WaitForSeconds(stayTime);
            time -= stayTime;

            idx = (idx + 1) % players.Length;
        }

        yield return true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
