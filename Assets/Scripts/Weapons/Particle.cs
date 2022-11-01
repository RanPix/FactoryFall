using UnityEngine;

[System.Serializable]
public struct NamedParticle
{
    public GameObject particleObject;
}

public class Particle : MonoBehaviour
{
    public NamedParticle particle;

}
