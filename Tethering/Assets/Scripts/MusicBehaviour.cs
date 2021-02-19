using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicBehaviour : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _musicClips;

    [SerializeField]
    private float _interclipDelay = 2f;

    [SerializeField]
    private int _currentIndex = -1;
    private float _currentStartTime = 0;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        PlayRandomClip();
    }

    private void OnEnable()
    {
        PlayRandomClip();
    }

    private void Update()
    {
        if (_musicClips.Length == 0)
            return;
        if (Time.time > _audioSource.clip.length + _currentStartTime + _interclipDelay)
            PlayRandomClip();
    }

    private void PlayRandomClip()
    {
        if (_musicClips.Length == 0)
            return;
        int newIndex = 0;
        do
        {
            newIndex = Random.Range(0, _musicClips.Length);
        } while (newIndex == _currentIndex);
        _currentIndex = newIndex;
        _currentStartTime = Time.time;
        _audioSource.clip = _musicClips[_currentIndex];
        _audioSource.Play();
    }
}
