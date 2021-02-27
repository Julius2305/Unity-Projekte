using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Transform menuTransform;
    private bool isPaused;

    private void Start()
    {
        isPaused = false;
        Pause();

        menuTransform = this.gameObject.transform.Find("Menu");
    }

    private void Update()
    {
        if (UnityInput.Instance.GetKeyDown(KeyCode.P))
            Pause();
    }

    public void StartButton()
    {
        Pause();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    /// <summary>
    /// Toggles the pause mode, when pause mode is not active yet.
    /// Otherwise detoggle it.
    /// </summary>
    private void Pause()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
        }

        menuTransform.gameObject.SetActive(isPaused);
    }
}
