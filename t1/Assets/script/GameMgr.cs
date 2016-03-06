using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMgr : MonoBehaviour
{
    public static GameMgr instance = null;
    public List<Effect> effect = new List<Effect>();
    AudioSource audio;
    void Awake()
    {
        audio = GetComponent<AudioSource>();

    }
    public void addEffect(Vector3 v3)
    {
        effect.Add(new Effect(v3));
    }
    public void playAudio()
    {
        audio.Play();
    }
    void Update()
    {
        foreach (var item in effect)
        {
            item.update();
        }
    }
    
}
