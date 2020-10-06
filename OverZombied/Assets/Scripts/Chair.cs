using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public MeshRenderer normalHair;
    public MeshRenderer cutHair;
    public MeshRenderer combedHair;
    public MeshRenderer cutcombedHair;








    public void SelectMeshes()
    {
        normalHair.material.color *= PlaceableSurface.selectedColorMultiplier;
        cutHair.material.color *= PlaceableSurface.selectedColorMultiplier;
        combedHair.material.color *= PlaceableSurface.selectedColorMultiplier;
        cutcombedHair.material.color *= PlaceableSurface.selectedColorMultiplier;   
    }

    public void UnSelectMeshes()
    {
        normalHair.material.color /= PlaceableSurface.selectedColorMultiplier;
        cutHair.material.color /= PlaceableSurface.selectedColorMultiplier;
        combedHair.material.color /= PlaceableSurface.selectedColorMultiplier;
        cutcombedHair.material.color /= PlaceableSurface.selectedColorMultiplier;
    }
}
