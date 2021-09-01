using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundPlayers : MonoBehaviour
{
  
  static int AudioPlayersInPool = 8;
  private static List<GameObject> soundPlayers;
  private static Dictionary<SoundAssets.SoundType, AudioSource> AudioPlayerAllocator = new Dictionary<SoundAssets.SoundType, AudioSource>();

  [SerializeField] GameObject soundPlayerPrefab;

  void Start()
  {
    soundPlayers = new List<GameObject>();
    GameObject tmp;

    for (int i = 0; i < AudioPlayersInPool; i++)
    {
      tmp = Instantiate(soundPlayerPrefab);
      tmp.transform.parent = this.transform;
      tmp.SetActive(false);

      soundPlayers.Add(tmp);
    }
  }

  public static AudioSource GetAvailableAudioSource()
  {
    for (int i = 0; i < AudioPlayersInPool; i++)
    {
      if (!soundPlayers[i].activeInHierarchy)
      {
        soundPlayers[i].SetActive(true);

        return soundPlayers[i].GetComponent<AudioSource>();
      }
    }
    // if no available sound player, then return null
    return null;
  }

  public static void AllocateAudioPlayer(SoundAssets.SoundType soundType, AudioSource audioSource)
  {
    if (!AudioPlayerAllocator.ContainsKey(soundType))
    {
      AudioPlayerAllocator.Add(soundType, audioSource);
    }
  }

  public static void PlayAudioSource(AudioSource audioSource)
  {
    audioSource.Play();
  }

  public static void StopSoundType(SoundAssets.SoundType soundType)
  {
    if (AudioPlayerAllocator.ContainsKey(soundType))
    {
      AudioPlayerAllocator[soundType].Stop();
      // disable gameObject which contains the audioPlayer
      AudioPlayerAllocator[soundType].transform.gameObject.SetActive(false);
      // Remove the AudioSource from the allocator to free up a voice for other sounds
      AudioPlayerAllocator.Remove(soundType);
    }
  }
}
