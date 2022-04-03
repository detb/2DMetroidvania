using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Animator transition;
    [SerializeField]
    private float transitionTime = 1f;
    private PlayerController pc;
    private int prevLevel;
    [SerializeField] private int levelIndex;
    
    private static readonly int StartAnim = Animator.StringToHash("start");
    
    [Header("On new scene event")]
        [SerializeField] protected UnityEvent onNewSceneEvent;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        LoadNextLevel();
    }

    void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    public void LoadLevelAndRespawn(int index)
    {
        StartCoroutine(LoadLevel(index));
    }

    private void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(levelIndex));
    }

    private void PreviousLevel(int index) {
        prevLevel = index;
    }

    IEnumerator LoadLevel(int index)
    {
        pc.Freeze();
        transition.SetTrigger(StartAnim);
        yield return new WaitForSeconds(transitionTime);

        var am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        am.StopLevelMusic(prevLevel);
        PreviousLevel(index);
        

        SceneManager.LoadScene(index);
        am.PlayLevelMusic(index);
        pc.Unfreeze();

    }
    
    
}
