using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private float swayIntensityX;
    [SerializeField] private float swayIntensityY;
    [SerializeField] private float maxSway;
    [SerializeField] private float minSway;

    public Vector3 normalWeaponPosition;

    public Transform weapon;
    [SerializeField] private PlayerOverride[] playerOverrides;


    private float currentTimeX;
    private float currentTimeY;

    private float xPos;
    private float yPos;

    private Vector3 smoothV;
    private Vector2 mouseSwayInput;
    
    private PlayerControls controls;

    public bool canSway = true;


    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Enable();
    }

    private void Update()
    {
        if(!weapon || !canSway)
            return;

        foreach (PlayerOverride player in playerOverrides)
        {
            /*
            if (currentSpeed>=0)
            {
                float playerMultiplier = (currentSpeed == 0) ? 1 : currentSpeed;
                
                currentTimeX += player.speedX / 10 * Time.deltaTime * playerMultiplier;
                currentTimeY += player.speedY / 10 * Time.deltaTime * playerMultiplier;
            }
            */

            xPos = player.bobX.Evaluate(currentTimeX) * player.intensityX;
            yPos = player.bobY.Evaluate(currentTimeY) * player.intensityY;
        }

        mouseSwayInput = controls.Player.Look.ReadValue<Vector2>();

        float xSway = -mouseSwayInput.x * swayIntensityX;
        float ySway = -mouseSwayInput.y * swayIntensityY;

        xSway = Mathf.Clamp(xSway, minSway, maxSway);
        ySway = Mathf.Clamp(ySway, minSway, maxSway);

        xPos += xSway;
        yPos += ySway;
    }

    private void LateUpdate()
    {
        if(!weapon || !canSway)
            return;

        Vector3 target = new Vector3(xPos + normalWeaponPosition.x, yPos + normalWeaponPosition.y, normalWeaponPosition.z);
        Vector3 desiredPos = Vector3.SmoothDamp(weapon.localPosition, target, ref smoothV, 0.1f);
        weapon.localPosition = desiredPos;
    }

    [System.Serializable]
    public struct PlayerOverride        
    {
        [Header("X Settings")] 
        public float intensityX;
        public AnimationCurve bobX;

        [Header("Y Settings")] 
        public float intensityY;
        public AnimationCurve bobY;
    }
}
