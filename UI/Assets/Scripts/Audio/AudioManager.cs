using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
  static AudioMixerGroupsManager _audioMixerGroupsManager;
  static AudioSourcePool _audioSourcePool;
  static AudioAssets _audioAssets;

  private void Awake()
  {
    _audioMixerGroupsManager = GetComponent<AudioMixerGroupsManager>();
    _audioSourcePool = GetComponent<AudioSourcePool>();
    _audioAssets = GetComponent<AudioAssets>();
  }

  public void PlayAudioType(AudioAssets.AudioType audioType, int pitchMultiplier)
  {
    AudioMixerGroup audioMixerGroup = _audioMixerGroupsManager.GetAudioMixerGroup(audioType);
    AudioClip audioClip = _audioAssets.GetClipInList(audioType);

    if (audioClip != null && audioMixerGroup != null)
    {
      _audioSourcePool.PlayAudioTypeASRenvelope(audioType, audioClip, pitchMultiplier, audioMixerGroup);
    }
    else
    {
      Debug.LogError("AudioClip and/or AudioMixerGroup returned null");
    }
  }

  public void PlayOneShot(AudioAssets.AudioType audioType)
  {

  }

  public void StopAudioType(AudioAssets.AudioType audioType)
  {
    _audioSourcePool.StopAudioType(audioType);
  }
}
