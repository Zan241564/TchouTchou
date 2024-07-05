using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource _music;
    [SerializeField] AudioSource _trainRolling;
    [SerializeField] float _fadeTime;
    [SerializeField] float _referenceVolumeRolling;
    [SerializeField] AudioSource _trainSounds;
    [SerializeField] List<AudioClip> _trainStartStop;
    [SerializeField] AudioSource _endGame;
    [SerializeField] List<AudioClip> _endGameTracks;
    [SerializeField] float _referenceVolumeEndGame;
    [SerializeField] float _quickFadeMinVolume;
    [SerializeField] float _quickFadeSpeedFactor;
    [SerializeField] AudioSource _uiSounds;
    [SerializeField] List<AudioClip> _uiSFX;
    [SerializeField] float _referenceVolumeUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FadeInTrainRolling()
    {
        _trainRolling.volume = 0.0f;
        _trainRolling.Play();
        while(_trainRolling.volume < _referenceVolumeRolling)
        {
            _trainRolling.volume += _referenceVolumeRolling * Time.deltaTime / _fadeTime;
            yield return null;
        }
    }

    public IEnumerator FadeOutTrainRolling()
    {
        float startVolume = _trainRolling.volume;
        while (_trainRolling.volume > 0.0f)
        {
            _trainRolling.volume -= startVolume * Time.deltaTime / _fadeTime;
            yield return null;
        }
        _trainRolling.Stop();
    }

    public void TrainStartStop(int id)
    {
        _trainSounds.Stop();
        _trainSounds.clip = _trainStartStop[id];
        _trainSounds.Play();
    }

    public void EndGame(int id)
    {
        _endGame.Stop();
        _endGame.clip = _endGameTracks[id];
        _endGame.volume = _referenceVolumeEndGame;
        _endGame.Play();
        StartCoroutine(QuickFadeMusic(_endGame.clip.length));
    }

    private IEnumerator QuickFadeMusic(float duration)
    {
        float startVolume = _music.volume;
        while(_music.volume > _quickFadeMinVolume)
        {
            _music.volume -= startVolume * Time.deltaTime / _fadeTime * _quickFadeSpeedFactor;
            yield return null;
        }
        yield return new WaitForSeconds(duration);
        float refVolume = _music.GetComponent<MusicLooper>().GetReferenceVolume();
        while (_music.volume < refVolume)
        {
            _music.volume += refVolume * Time.deltaTime / _fadeTime;
            yield return null;
        }
    }

    public void UI(int id)
    {
        _uiSounds.Stop();
        _uiSounds.clip = _uiSFX[id];
        _uiSounds.volume = _referenceVolumeUI;
        _uiSounds.Play();
    }
    
}
