using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSourcePool : MonoBehaviour
{

  [Header("Amp. Env. in seconds")]
  [SerializeField, Range(0f, 2f)] private double _attackTime;
  [SerializeField, Range(0f, 2f)] private double _sustainTime;
  [SerializeField, Range(0f, 2f)] private double _releaseTime;
  [SerializeField] GameObject audioSourcePrefab;

  private readonly int _audioSourcesInPool = 8;
  private int _nextAudioSourceIndex = 0;
  private bool _voiceStealingEnabled = true;
  private List<AudioSourceVoice> _audioSourceVoices;
  private Dictionary<AudioAssets.AudioType, AudioSource> _audioSourceAllocator = new Dictionary<AudioAssets.AudioType, AudioSource>();


  void Start()
  {
    _audioSourceVoices = new List<AudioSourceVoice>();
    GameObject tmp;

    for (int i = 0; i < _audioSourcesInPool; i++)
    {
      tmp = Instantiate(audioSourcePrefab);
      tmp.transform.parent = transform;
      tmp.transform.localPosition = Vector3.zero;

      _audioSourceVoices.Add(tmp.GetComponent<AudioSourceVoice>());
    }
  }

  public void PlayAudioTypeASRenvelope(AudioAssets.AudioType audioType, AudioClip audioClip, AudioMixerGroup audioMixerGroup)
  {

    _nextAudioSourceIndex = GetNextAvailableAudioSourceIndex();
    _audioSourceVoices[_nextAudioSourceIndex].Play(audioClip, _attackTime, _sustainTime, _releaseTime, audioMixerGroup);
    // allocate audio source
    // play

  }

  private int GetNextAvailableAudioSourceIndex()
  {
    if (_voiceStealingEnabled)
    {
      int nextAudioSourceIndex = (_nextAudioSourceIndex + 1) % _audioSourcesInPool;
      return nextAudioSourceIndex;
    }
    else
    {
      // is the voice playing method. if yes, do something.. 
      return 0;
    }
  }


  private void AllocateAudioSource(AudioAssets.AudioType audioType, AudioSource audioSource)
  {
    // return audioSources[i].GetComponent<AudioSource>();
    if (!_audioSourceAllocator.ContainsKey(audioType))
    {
      _audioSourceAllocator.Add(audioType, audioSource);
    }
  }

  public void StopAudioType(AudioAssets.AudioType audioType)
  {
    if (_audioSourceAllocator.ContainsKey(audioType))
    {
      _audioSourceAllocator[audioType].Stop();
      // disable gameObject which contains the audioPlayer
      _audioSourceAllocator[audioType].transform.gameObject.SetActive(false);
      // Remove the AudioSource from the allocator to free up a voice for other sounds
      _audioSourceAllocator.Remove(audioType);
    }
  }
}
