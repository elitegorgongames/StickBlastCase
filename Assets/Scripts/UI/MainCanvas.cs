using UnityEngine;

public class MainCanvas : MonoBehaviour
{

    public GameObject failPanelObject;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.Instance.FailEvent += EnableFailPanel;
    }

    private void OnDestroy()
    {
        EventManager.Instance.FailEvent -= EnableFailPanel;
    }

    private void EnableFailPanel()
    {
        failPanelObject.SetActive(true);
    }

    private void DisableFailPanel()
    {
        failPanelObject.SetActive(false);
    }
}
