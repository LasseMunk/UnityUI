using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerGroupsManager : MonoBehaviour
{
  public AudioMixer MainAudioMixer;

  public AudioMixerGroup GetAudioMixerGroup(AudioAssets.AudioType audioType)
  {
    switch (audioType)
    {
      case AudioAssets.AudioType.Ambience:
        return MainAudioMixer.FindMatchingGroups("AmbiencesAudioMixerGroup")[0];
      case AudioAssets.AudioType.Click:
        return MainAudioMixer.FindMatchingGroups("ClicksAudioMixerGroup")[0];
      case AudioAssets.AudioType.Swipe:
        return MainAudioMixer.FindMatchingGroups("SwipesAudioMixerGroup")[0];
      default:
        Debug.LogError("No AudioMixerGroup assigned to audioType");
        return null;
    }
  }
}
