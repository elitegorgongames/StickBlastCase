using UnityEngine;

public class PolishSettings : MonoBehaviour
{
    public static PolishSettings Instance;


    [Header("Stick Movement")]
    public float stickFirstHoldOffsetMovementTime;

    [Header("StickSpawnMovement")]
    public float moveToStartPointSpeed;

    [Header("Stick Placement")]
    public float stickPlacementTime;
    public AnimationCurve stickPlacementAC;

    [Header("Dissolve")]
    public Material dissolveMaterial;


    private void Awake()
    {
        Instance = this;
    }
}
