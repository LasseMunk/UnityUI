using UnityEngine;
using UnityEngine.Audio;

public static class AudioManager
{
  static AudioMixerGroupsManager _audioMixerGroupsManager = GameObject.FindGameObjectWithTag("AudioMaster").GetComponent<AudioMixerGroupsManager>();

  public static void PlayAudioType(AudioAssets.AudioType audioType)
  {
    AudioMixerGroup audioMixerGroup = _audioMixerGroupsManager.GetAudioMixerGroup(audioType);
    AudioClip audioClip = AudioAssets.GetClipInList(audioType);

    if (audioClip != null && audioMixerGroup != null)
    {
      AudioSourcePool.PlayAudioTypeASRenvelope(audioType, audioClip, audioMixerGroup);
    }
    else
    {
      Debug.LogError("AudioClip and/or AudioMixerGroup returned null");
    }
  }

  public static void PlayOneShot(AudioAssets.AudioType audioType)
  {

  }

  public static void StopAudioType(AudioAssets.AudioType audioType)
  {
    AudioSourcePool.StopAudioType(audioType);
  }
}
