using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Animator transition;
    [SerializeField]
    private float transitionTime = 1f;

    [SerializeField] private int levelIndex;
    
    private static readonly int Start = Animator.StringToHash("start");

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(levelIndex));
    }

    IEnumerator LoadLevel(int index)
    {
        transition.SetTrigger(Start);
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(index);
    }
}
