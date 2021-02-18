using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Tethering.Net;

public class ProfanityCheckBehaviour : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _playerNameTextField;

    [SerializeField]
    private UnityEvent OnValidationFailed;
    [SerializeField]
    private UnityEvent OnValidationSucceeded;

    private ProfanityFilter _filter;

    private void Awake()
    {
        _filter = new ProfanityFilter();
        _filter.LoadString(Resources.Load<TextAsset>("ProfanityList").text);
    }

    public void Validate()
    {
        var profane = _filter.ContainsProfanity(_playerNameTextField.text, out string word);
        if (profane)
            OnValidationFailed.Invoke();
        else
            OnValidationSucceeded.Invoke();
    }
}
