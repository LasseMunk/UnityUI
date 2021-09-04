using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceVoice : MonoBehaviour
{
  // private bool isPlaying = false;
  private readonly ASREnvelope _envelope = new ASREnvelope();
  private AudioSource _audioSource;

  void Awake()
  {
    _audioSource = GetComponent<AudioSource>();
  }

  public void Play(AudioClip audioClip, double attackTime, double sustainTime, double releaseTime, AudioMixerGroup audioMixerGroup)
  {
    // isPlaying = true;
    _envelope.Reset(attackTime, sustainTime, releaseTime, AudioSettings.outputSampleRate);
    _audioSource.clip = audioClip;
    _audioSource.outputAudioMixerGroup = audioMixerGroup;
    double nonNegativeSustainTime = (sustainTime > attackTime) ? (sustainTime - attackTime) : 0.0;
    sustainTime = nonNegativeSustainTime;
    _audioSource.Play();
  }

  // public void Stop()
  // {
  //   // isPlaying = false;
  // }

  // public bool GetIsPlaying()
  // {
  //   return isPlaying;
  // }

  // public void SetIsPlaying(bool setIsPlaying)
  // {
  //   isPlaying = setIsPlaying;
  // }

  private void OnAudioFilterRead(float[] buffer, int numChannels)
  {
    for (int sIdx = 0; sIdx < buffer.Length; sIdx += numChannels)
    {
      double volume = _envelope.GetLevel();

      for (int cIdx = 0; cIdx < numChannels; ++cIdx)
      {
        buffer[sIdx + cIdx] *= (float)volume;
      }
    }
  }
}
