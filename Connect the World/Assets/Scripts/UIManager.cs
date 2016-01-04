using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance { get; protected set; }
    public GameObject eventPanel, gameAnnouncementPanel;
    public Text eventTitle, eventDescription;
    public Text announceTitle, annouceDesc;
    public Text yearText, monthsText;

    void Awake()
    {
        instance = this;
    }

    public void DisplayEventPanel(WorldEventType eventType, string description)
    {
        eventTitle.text = eventType.ToString();
        eventDescription.text = description;

        if (!eventPanel.activeSelf)
        {
            eventPanel.SetActive(true);
        }
    }

    public void CloseEventPanel()
    {
        if (eventPanel.activeSelf)
        {
            eventPanel.SetActive(false);
        }
    }

    public void DisplayAnnouncementPanel(string title, string description)
    {
        announceTitle.text = title;
        annouceDesc.text = description;

        if (!gameAnnouncementPanel.activeSelf)
        {
            gameAnnouncementPanel.SetActive(true);
        }
    }

    public void CloseAnnouncementPanel()
    {
        if (gameAnnouncementPanel.activeSelf)
        {
            gameAnnouncementPanel.SetActive(false);
        }
    }

    public void DisplayTimePanel(int months, int years = 0)
    {
        yearText.text = years.ToString();
        monthsText.text = months.ToString();
    }


}


