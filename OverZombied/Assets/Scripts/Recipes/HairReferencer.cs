using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HairReferencer : MonoBehaviour
{
    private enum HairID { NORMAL, CUT, COMBED, CUTCOMBED }
    
    public MeshRenderer[] hairs;

    public bool updateUI = true;

    [SerializeField]
    Canvas canvas;

    [SerializeField]
    private Image[] images;

    public void UpdateUI()
    {
        if (!updateUI)
            return;

        for(int i = 0; i < 4; ++i)
        {
            if (hairs[i].gameObject.activeSelf)
            {
                switch (i)
                {
                    case (int)HairID.NORMAL:
                        images[0].gameObject.SetActive(false); images[1].gameObject.SetActive(false);
                        canvas.gameObject.SetActive(false);
                        break;
                    case (int)HairID.CUT:
                        images[0].gameObject.SetActive(true); images[1].gameObject.SetActive(false);
                        canvas.gameObject.SetActive(true);
                        break;
                    case (int)HairID.COMBED:
                        images[0].gameObject.SetActive(false); images[1].gameObject.SetActive(true);
                        canvas.gameObject.SetActive(true);
                        break;
                    case (int)HairID.CUTCOMBED:
                        images[0].gameObject.SetActive(true); images[1].gameObject.SetActive(true);
                        canvas.gameObject.SetActive(true);
                        break;
                }
                break;
            }
        }
    }
}
