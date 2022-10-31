using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NamedParticle
{
    public string name;
    public GameObject particleObject;
    public GameObject PlaceForSit;
}

public class Particle : MonoBehaviour
{
    public NamedParticle particle;

}
