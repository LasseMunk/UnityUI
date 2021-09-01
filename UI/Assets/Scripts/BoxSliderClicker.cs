using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSliderClicker : MonoBehaviour
{
  Transform _boxSliderTransform;
  Camera _mainCamera;
  RaycastHit RaycastedBox;
  Ray ray;

  List<MeshRenderer> BoxSliderMeshRendererList = new List<MeshRenderer>();
  int PreviousSelectedBoxIndex;
  [SerializeField] int BoxesInSlider;
  [SerializeField] GameObject BoxInSliderPrefab;
  [SerializeField] Material BoxSliderOnMaterial;
  [SerializeField] Material BoxSliderOffMaterial;


  void Awake()
  {
    _mainCamera = Camera.main;
    _boxSliderTransform = gameObject.transform;
  }

  void Start()
  {
    CreateBoxesInSlider(BoxesInSlider);
    PreviousSelectedBoxIndex = 0;
    BoxSliderMeshRendererList[0].material = BoxSliderOnMaterial;
  }

  void Update()
  {

    if (Input.GetMouseButton(0))
    {
      ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

      // out hit: store in hit without returning
      if (Physics.Raycast(ray, out RaycastedBox, 50f))
      {
        int SelectedBoxIndex = GetSelectedBoxIndex(RaycastedBox);
        if (SelectedBoxIndex != PreviousSelectedBoxIndex)
        {
          ChangeMaterials(SelectedBoxIndex);
          SoundManager.StopSoundType(SoundAssets.SoundType.Click);
          SoundManager.PlaySoundType(SoundAssets.SoundType.Click);
        }
      }
    }
    if (Input.GetMouseButtonUp(0))
    {
      SoundManager.StopSoundType(SoundAssets.SoundType.Click);
    }
  }

  int GetSelectedBoxIndex(RaycastHit RaycastedBox)
  {
    return int.Parse(RaycastedBox.transform.gameObject.name);
  }

  void ChangeMaterials(int SelectedBoxIndex)
  {
    if (SelectedBoxIndex > PreviousSelectedBoxIndex)
    {
      ChangeMaterialsToON(SelectedBoxIndex);
    }
    else
    {
      ChangeMaterialsToOFF(SelectedBoxIndex);
    }
    PreviousSelectedBoxIndex = SelectedBoxIndex;
  }

  void ChangeMaterialsToON(int selectedBoxIndex)
  {
    for (int i = PreviousSelectedBoxIndex; i <= selectedBoxIndex; i++)
    {
      BoxSliderMeshRendererList[i].material = BoxSliderOnMaterial;
    }
  }

  void ChangeMaterialsToOFF(int selectedBoxIndex)
  {
    for (int i = PreviousSelectedBoxIndex; i > selectedBoxIndex; i--)
    {
      BoxSliderMeshRendererList[i].material = BoxSliderOffMaterial;
    }
  }

  void CreateBoxesInSlider(int NumberOfBoxes)
  {
    Vector3 position = _boxSliderTransform.transform.position;
    Quaternion rotation = new Quaternion(0, 0, 0, 1);

    for (int i = 0; i < NumberOfBoxes; i++)
    {
      position.y = position.y + 0.6f;
      GameObject tempBox = Instantiate(BoxInSliderPrefab, position, rotation, _boxSliderTransform);
      tempBox.name = i.ToString();
      BoxSliderMeshRendererList.Add(tempBox.GetComponent<MeshRenderer>());
    }
  }
}
