using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using TMPro;

public class Notification : MonoBehaviour
{
    [SerializeField]
    float timerStart;
    [SerializeField]
    float timerDuration = 5f;
    [SerializeField]
    bool active;

    private void LateUpdate()
    {
        if (active)
        {
            if (Time.time - timerStart >= timerDuration)
            {
                active = false;
                CloseNotification();
            }
        }
    }
    public void Initiate(string textDetails, string textNotification = null)
    {
        gameObject.SetActive(true);

        transform.Find(TicketNotification.NotificationDetail.ToString()).GetComponent<TextMeshProUGUI>().SetText(textDetails);
        if (textNotification != null) gameObject.transform.Find(TicketNotification.NotificationText.ToString()).GetComponent<TextMeshProUGUI>().SetText(textNotification);
        StartTimer();
    }
    public void StartTimer(float duration = 5f)
    {
        timerStart = Time.time;
        timerDuration = duration;
        active = true;
    }
    public void CloseNotification()
    {
        gameObject.SetActive(false);
    }
}
