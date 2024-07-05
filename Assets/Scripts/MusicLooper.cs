using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLooper : MonoBehaviour
{

    [SerializeField] List<AudioClip> _tracks;
    AudioSource _audioSource;
    int _currentTrack = 0;
    [SerializeField] float _referenceVolume;
    [SerializeField] float _fadeTime;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(FadeInMusic());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayNextTrack()
    {
        _audioSource.clip = _tracks[_currentTrack];
        _audioSource.Play();
        _currentTrack = (_currentTrack + 1) % _tracks.Count;
        Invoke("PlayNextTrack", _audioSource.clip.length);
    }

    private IEnumerator FadeInMusic()
    {
        _audioSource.Stop();
        _audioSource.volume = 0.0f;
        _audioSource.clip = _tracks[0];
        _audioSource.Play();
        Invoke("PlayNextTrack", _audioSource.clip.length);
        while (_audioSource.volume < _referenceVolume)
        {
            _audioSource.volume += _referenceVolume * Time.deltaTime / _fadeTime;
            yield return null;
        }
    }

    public float GetReferenceVolume()
    {
        return _referenceVolume;
    }
}
