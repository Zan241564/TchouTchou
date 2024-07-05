using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceSwapper : MonoBehaviour
{

    [SerializeField] List<AudioClip> _ambiances;
    AudioSource _audioSource;
    [SerializeField] float _fadeTime;
    [SerializeField] float _referenceVolume;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _ambiances[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator PlayAmbiance(int id)
    {
        float startVolume = _audioSource.volume;
        while (_audioSource.volume > 0)
        {
            _audioSource.volume -= startVolume * Time.deltaTime / _fadeTime;
            yield return null;
        }
        _audioSource.Stop();
        _audioSource.clip = _ambiances[id];
        _audioSource.Play();
        while (_audioSource.volume < _referenceVolume)
        {
            _audioSource.volume += _referenceVolume * Time.deltaTime / _fadeTime;
            yield return null;
        }
    }

    public IEnumerator FadeInAmbiance(int id)
    {
        _audioSource.Stop();
        _audioSource.volume = 0.0f;
        _audioSource.clip = _ambiances[id];
        _audioSource.Play();
        while (_audioSource.volume < _referenceVolume)
        {
            _audioSource.volume += _referenceVolume * Time.deltaTime / _fadeTime;
            yield return null;
        }
    }
}
