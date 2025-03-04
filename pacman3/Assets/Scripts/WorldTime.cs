using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldTime : MonoBehaviour
{
    float time;
    bool run = true;
    TextMeshProUGUI timeLabel;

    private void Awake() {
        timeLabel = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable() {
        Player.OnPlayerDeath += TimeStop;
        Player.OnPlayerReset += TimeStart;
    }

    private void OnDisable() {
        Player.OnPlayerDeath -= TimeStop;
        Player.OnPlayerReset -= TimeStart;
    }

    void Update()
    {
        if (run == false) {
            return;
        }

        time += Time.deltaTime;
        timeLabel.text = time.ToString("00:00");
    }

    private void TimeStart() {
        run = true;
    }

    private void TimeStop() {
        run = false;
    }
}
