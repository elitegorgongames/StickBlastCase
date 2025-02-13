using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PostProcessingSettings : MonoBehaviour
{

    public static PostProcessingSettings Instance;

    private bool _isEnable;


    private void Awake()
    {
        Instance = this;
    }




    public void PostProcessing()
    {
        StartCoroutine(PostProcessingToggle());
    }

    IEnumerator PostProcessingToggle()
    {

        if (_isEnable)
        {
            yield break;
        }

        
        Camera.main.gameObject.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;
        _isEnable = true;

        yield return new WaitForSeconds(.5f);
        Camera.main.gameObject.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = false;
        _isEnable = false;
        Debug.Log("post processing");
    }
}
