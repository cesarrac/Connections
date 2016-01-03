using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Master_Manager : MonoBehaviour {

    GameObject mainMenuPanel;

    void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
    }

	void Awake()
    {
        // 1)  ---- MAIN MENU  -----:
        if (Application.loadedLevel == 0)
        {
            mainMenuPanel = GameObject.FindGameObjectWithTag("Menu Panel");
            DisplayMainMenuOptions();
        }
    }

    void DisplayMainMenuOptions()
    {
        if (!mainMenuPanel.activeSelf)
        {
            mainMenuPanel.SetActive(true);
        }

    }

    public void LoadWorld()
    {
        Application.LoadLevel(1);
    }
}
