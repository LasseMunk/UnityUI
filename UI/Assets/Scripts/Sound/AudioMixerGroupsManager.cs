using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerGroupsManager : MonoBehaviour
{
  public AudioMixer MainAudioMixer;

  private void Start()
  {

  }

  public AudioMixerGroup GetAudioMixerGroup(SoundAssets.SoundType soundType)
  {
    switch (soundType)
    {
      case SoundAssets.SoundType.Ambience:
        return MainAudioMixer.FindMatchingGroups("AmbiencesAudioMixerGroup")[0];
      case SoundAssets.SoundType.Click:
        return MainAudioMixer.FindMatchingGroups("ClicksAudioMixerGroup")[0];
      case SoundAssets.SoundType.Swipe:
        return MainAudioMixer.FindMatchingGroups("SwipesAudioMixerGroup")[0];
      default:
        Debug.LogError("No AudioMixerGroup assigned to soundType");
        return null;
    }
  }
}
