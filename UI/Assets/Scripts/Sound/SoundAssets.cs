using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class SoundAssets : MonoBehaviour
{

  public class AudioClips
  {
    public AudioClip audioClip;
    // Expand with pr. sound parameters here
  }

  public enum SoundType
  {
    Ambience,
    Click,
    Swipe
  }

  private static Dictionary<SoundAssets.SoundType, List<AudioClips>> AudioListsDict = new Dictionary<SoundAssets.SoundType, List<AudioClips>>();

  void Awake()
  {
    LoadAudioClipsListsIntoDict();
    // LogListsCount();
  }

  void LoadAudioClipsListsIntoDict()
  {
    foreach (SoundType soundType in Enum.GetValues(typeof(SoundType)))
    {
      if (!AudioListsDict.ContainsKey(soundType))
      {
        List<AudioClips> AudioClipsList = new List<AudioClips>();
        switch (soundType)
        {
          case SoundType.Ambience:
            AudioClipsList = LoadAudioClipsFromFolderToList("Ambiences");
            break;
          case SoundType.Click:
            AudioClipsList = LoadAudioClipsFromFolderToList("Clicks");
            break;
          case SoundType.Swipe:
            AudioClipsList = LoadAudioClipsFromFolderToList("Swipes");
            break;
          default:
            break;
        }
        AudioListsDict.Add(soundType, AudioClipsList);
      }
    }
  }

  void LogListsCount()
  {
    foreach (KeyValuePair<SoundAssets.SoundType, List<AudioClips>> entry in AudioListsDict)
    {
      print($"{entry.Key}: <color=red>{entry.Value.Count}</color>");
    }
  }

  public static AudioClip GetClipInList(SoundType soundType)
  {
    if (AudioListsDict.ContainsKey(soundType)) // currently always returns the 0 indexed clip
    {
      List<AudioClips> audioClipList = AudioListsDict[soundType];
      AudioClip clip = audioClipList[0].audioClip;
      return clip;
    }
    else { return null; }
  }

  public AudioClip GetRandomAudioClipFromList(List<AudioClips> _whichList)
  {
    int _sizeOfList = _whichList.Count;
    if (_sizeOfList > 0)
    {
      int randomSampleIndex = Random.Range(0, _sizeOfList - 1);
      var randomAudioClip = _whichList[randomSampleIndex].audioClip;
      return randomAudioClip;
    }
    else
    {
      return null;
    }
  }

  // List<AudioClips> LoadAudioClipsFromFolderToList(string _FolderPath)
  // {
  //   // FolderPath relative to /Assets/Audio
  //   string[] _FilesInFolder = GetWavFilesInFolder(_FolderPath);

  //   var tempList = new List<AudioClips>();

  //   for (int i = 0; i < _FilesInFolder.Length; i++)
  //   {
  //     var tempClip = new AudioClips();
  //     StartCoroutine(GetAudioClip(_FilesInFolder[i], tempClip.audioClip));
  //     tempList.Add(tempClip);
  //   }
  //   return tempList;
  // }

  // IEnumerator GetAudioClip(string PathToFile, AudioClip loadAudioClip)
  // {
  //   using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(PathToFile, AudioType.WAV))
  //   {
  //     yield return www.SendWebRequest();

  //     if (www.result == UnityWebRequest.Result.ConnectionError)
  //     {
  //       Debug.Log(www.error);
  //     }
  //     else
  //     {
  //       loadAudioClip = DownloadHandlerAudioClip.GetContent(www);
  //     }
  //   }
  // }
  List<AudioClips> LoadAudioClipsFromFolderToList(string _FolderPath)
  {
    // FolderPath relative to /Assets/Audio
    string[] _FilesInFolder = GetOggFilesInFolder(_FolderPath);

    var tempList = new List<AudioClips>();

    for (int i = 0; i < _FilesInFolder.Length; i++)
    {
      var tempClip = new AudioClips();
      tempClip.audioClip = Resources.Load<AudioClip>(_FilesInFolder[i]);
      tempList.Add(tempClip);
    }
    return tempList;
  }

  // private string[] GetWavFilesInFolder(string _FolderName)
  // {
  //   string _pathToFolder = Application.dataPath + "/Resources/Audio/" + _FolderName;
  //   // Application.dataPath returns: <path to project folder>/Assets
  //   if (Directory.Exists(_pathToFolder))
  //   {
  //     string[] _FilesInFolder = Directory.GetFiles(_pathToFolder, "*.wav");
  //     return _FilesInFolder;
  //   }
  //   else
  //   {
  //     Debug.LogError("Could not find folder with given name in path: /Resources/Audio");
  //     return null;
  //   }
  // }
  private string[] GetOggFilesInFolder(string _FolderName)
  {
    string _pathToFolder = Application.dataPath + "/Resources/Audio/" + _FolderName;
    // Application.dataPath returns: <path to project folder>/Assets
    if (Directory.Exists(_pathToFolder))
    {
      string[] _FilesInFolder = Directory.GetFiles(_pathToFolder, "*.ogg");

      for (int i = 0; i < _FilesInFolder.Length; i++)
      {
        string filePathToRemove = Application.dataPath + "/Resources/";
        // removes system specific filepath
        string RelativeFilePath = _FilesInFolder[i].Remove(0, filePathToRemove.Length);
        // removes filetype/extension (e.g. .wav, .mp3, .ogg)
        string[] removedFileExtension = RelativeFilePath.Split('.');
        _FilesInFolder[i] = removedFileExtension[0];
      }
      return _FilesInFolder;
    }
    else
    {
      Debug.Log(_FolderName);
      Debug.LogError("Could not find folder with given name in path: /Resources/Audio");
      return null;
    }
  }
}
