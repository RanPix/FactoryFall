using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MultiNetworkComponentsSorter : MonoBehaviour
{
    [field: SerializeField] public NetworkAnimator[] animatorComponents { get; private set; }
    [field: SerializeField] public string[] animatorNames { get; private set; }
    [field: SerializeField] public NetworkTransformChild[] transformComponents { get; private set; }
    [field: SerializeField] public string[] transformNames { get; private set; }
    public Dictionary<string, NetworkAnimator> animators { get; private set; } = new Dictionary<string, NetworkAnimator>();
    public Dictionary<string, NetworkTransformChild> transforms { get; private set; } = new Dictionary<string, NetworkTransformChild>();
    // Start is called before the first frame update
    void Awake()
    {
        if(animatorComponents.Length != animatorNames.Length)
            Debug.LogError("The number of animator components must be equal to the number of animator names");
        if(transformComponents.Length != transformNames.Length)
            Debug.LogError("The number of transform components must be equal to the number of transform names");

        for (int i = 0; i < animatorComponents.Length; i++)
        {
            animators.Add(animatorNames[i], animatorComponents[i]);
        }
        for (int i = 0; i < transformComponents.Length; i++)
        {
            transforms.Add(transformNames[i], transformComponents[i]);
        }
    }

    public NetworkTransformChild GetNetworkTransform(string componentName)
    {
        if (!transforms.ContainsKey(componentName))
            Debug.LogError($"There is no transform component with this name: {componentName}");

        return transforms[componentName];
    }
    public NetworkAnimator GetNetworkAnimator(string componentName)
    {
        if (!animators.ContainsKey(componentName))
            Debug.LogError($"There is no animator component with this name: {componentName}");
        return animators[componentName];
    }
}
