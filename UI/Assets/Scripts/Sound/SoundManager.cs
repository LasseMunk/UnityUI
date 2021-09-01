using UnityEngine;
using UnityEngine.Audio;

public static class SoundManager
{
  static AudioMixerGroupsManager _audioMixerGroupsManager = GameObject.FindGameObjectWithTag("SoundMaster").GetComponent<AudioMixerGroupsManager>();

  public static void PlaySoundType(SoundAssets.SoundType soundType)
  {
    var audioSource = SoundPlayers.GetAvailableAudioSource();
    audioSource.outputAudioMixerGroup = _audioMixerGroupsManager.GetAudioMixerGroup(soundType);
    AudioClip tempClip = SoundAssets.GetClipInList(soundType);

    if (tempClip != null)
    {
      audioSource.clip = tempClip;
      SoundPlayers.AllocateAudioPlayer(soundType, audioSource);
      SoundPlayers.PlayAudioSource(audioSource);
    }
    else
    {
      Debug.LogError("AudioClip returned null from requested AudioClipsList");
    }
  }

  public static void PlayOneShot(SoundAssets.SoundType soundType)
  {
    var audioSource = SoundPlayers.GetAvailableAudioSource();
    audioSource.PlayOneShot(SoundAssets.GetClipInList(soundType));
  }

  public static void StopSoundType(SoundAssets.SoundType soundType)
  {
    SoundPlayers.StopSoundType(soundType);
  }
}
