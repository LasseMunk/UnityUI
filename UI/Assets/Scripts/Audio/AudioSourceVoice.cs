using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSourceVoice : MonoBehaviour
{

  public bool isPlaying = false;

  private readonly ASREnvelope _envelope = new ASREnvelope();
  private AudioSource _audioSource;

  void Awake()
  {
    _audioSource = GetComponent<AudioSource>();
  }

  public void Play(AudioClip audioClip, double attackTime, double sustainTime, double releaseTime, AudioMixerGroup audioMixerGroup)
  {
    // isPlaying = true;
    _audioSource.clip = audioClip;
    _audioSource.outputAudioMixerGroup = audioMixerGroup;
    double nonNegativeSustainTime = (sustainTime > attackTime) ? (sustainTime - attackTime) : 0.0;
    sustainTime = nonNegativeSustainTime;
    _envelope.Reset(attackTime, sustainTime, releaseTime, AudioSettings.outputSampleRate);
  }

  public void Stop()
  {
    // isPlaying = false;
  }

  private void OnAudioFilterRead(float[] buffer, int numChannels)
  {
    for (int sampleIndex = 0; sampleIndex < buffer.Length; sampleIndex += numChannels)
    {
      double volume = _envelope.GetLevel();

      for (int channelIndex = 0; channelIndex < numChannels; channelIndex++)
      {
        buffer[sampleIndex + channelIndex] *= (float)volume;
      }
    }
  }
}
