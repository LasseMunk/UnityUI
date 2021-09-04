using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioAssets : MonoBehaviour
{

  public class AudioClips
  {
    public AudioClip audioClip;
  }

  public enum AudioType
  {
    Ambience,
    Click,
    Swipe
  }

  private static Dictionary<AudioAssets.AudioType, List<AudioClips>> AudioListsDict = new Dictionary<AudioAssets.AudioType, List<AudioClips>>();

  void Awake()
  {
    LoadAudioClipsListsIntoDict();
    // LogListsCount();
  }

  void LoadAudioClipsListsIntoDict()
  {
    foreach (AudioType AudioType in Enum.GetValues(typeof(AudioType)))
    {
      if (!AudioListsDict.ContainsKey(AudioType))
      {
        List<AudioClips> AudioClipsList = new List<AudioClips>();
        switch (AudioType)
        {
          case AudioType.Ambience:
            AudioClipsList = LoadAudioClipsFromFolderToList("Ambiences");
            break;
          case AudioType.Click:
            AudioClipsList = LoadAudioClipsFromFolderToList("Clicks");
            break;
          case AudioType.Swipe:
            AudioClipsList = LoadAudioClipsFromFolderToList("Swipes");
            break;
          default:
            break;
        }
        AudioListsDict.Add(AudioType, AudioClipsList);
      }
    }
  }

  void LogListsCount()
  {
    foreach (KeyValuePair<AudioAssets.AudioType, List<AudioClips>> entry in AudioListsDict)
    {
      string AudioListName = entry.Key.ToString();
      string NumberOfClipsInList = entry.Value.Count.ToString();

      print($"{AudioListName}: <color=red>{NumberOfClipsInList}</color>");
    }
  }

  public AudioClip GetClipInList(AudioType AudioType)
  {
    if (AudioListsDict.ContainsKey(AudioType))
    {
      List<AudioClips> audioClipList = AudioListsDict[AudioType];
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
