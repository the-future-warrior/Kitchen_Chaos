using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerSelectUI : MonoBehaviour {
    [SerializeField] private TMP_Dropdown gameDurationSelectDropdown;

    private List<string> gameDurationSelectDropdownOptions = new List<string> { 
        "5 mins", 
        "10 mins",
        "15 mins",
        "20 mins",
        "25 mins",
        "30 mins"
    };

    private void Start() {
        if(KitchenGameLobby.Instance.IsLobbyHost()) {
            gameDurationSelectDropdown.AddOptions(gameDurationSelectDropdownOptions);
            SetGameDuration();
            Show();
        } else {
            Hide();
        }
    }

    public void SetGameDuration() {
        CharacterSelectReady.Instance.SetGameDurationInSeconds((gameDurationSelectDropdown.value + 1) * 5 * 60);
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
