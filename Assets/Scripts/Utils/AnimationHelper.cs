using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    public void DeacitavtePanel()
    {
        gameObject.SetActive(false);
    }
    public void AcitavtePanel()
    {
        gameObject.SetActive(true);
    }
}
