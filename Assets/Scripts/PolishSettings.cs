using UnityEngine;

public class PolishSettings : MonoBehaviour
{
    public static PolishSettings Instance;


    [Header("Stick Movement")]
    public float stickFirstHoldOffsetMovementTime;

    [Header("StickSpawnMovement")]
    public float moveToStartPointSpeed;
    public float moveToStartPointTime;

    [Header("Stick Placement")]
    public float stickPlacementTime;
    public AnimationCurve stickPlacementAC;

    [Header("Stick part settle movement")]
    public float stickSettleScaleMultiplier;
    public float stickSettleScaleTime;
    public AnimationCurve stickSettleScaleAC;

    [Header("Dissolve")]
    public Material dissolveMaterial;

    [Header("DiamondSettings")]
    public float diamondMoveSpeed;
    public float diamondMoveSpeedInterval;
    public float diamondScaleTime;
    public Vector3 diamondTargetScale;
    public AnimationCurve diamondMoveAC;
    public Transform diamondTargetTransform;

   


    private void Awake()
    {
        Instance = this;
    }
}
