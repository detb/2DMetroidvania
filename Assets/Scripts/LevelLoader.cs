using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private int levelIndex;
    
    private static readonly int Start = Animator.StringToHash("start");
    
    [Header("On new scene event")]
        [SerializeField] protected UnityEvent onNewSceneEvent;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        pc = col.GetComponent<PlayerController>();
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(levelIndex));
    }

    IEnumerator LoadLevel(int index)
    {
        pc.Freeze();
        transition.SetTrigger(Start);
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(index);
        pc.Unfreeze();
    }
    
    
}
