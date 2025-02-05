using UnityEngine;

public class Stick : MonoBehaviour
{

    public bool isPicked;
    public bool isPlaced;
    public StickType stickType;
  

 
    public Transform calculationTransformStartPoint;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public Transform GetCalculationTransformStartPoint()
    {
        return calculationTransformStartPoint;
    }

}


public enum StickType
{
    Vertical,
    DoubleLengthVertical,
    DoubleLengthHorizontal,
    Horizontal,
    LType,    
    DownwardsLType,
    MirroredLType,
    DoubleLengthLType,
    UType,
    DownwardsUType
};
