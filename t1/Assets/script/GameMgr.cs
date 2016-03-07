using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMgr : MonoBehaviour
{
    public static GameMgr instance = null;
    public List<Effect> effect = new List<Effect>();
    AudioSource _audio;
    void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }
    public void addEffect(Vector3 v3,string url = "effect",float delTime = 1f)
    {
        effect.Add(new Effect(v3, url,delTime));
    }
    public void playAudio()
    {
        _audio.Play();
    }
    void Update()
    {
        foreach (var item in effect)
        {
            item.update();
        }
    }
    
}
