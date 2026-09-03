using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{

    private static DontDestroyObject instance = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
       // gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewAudioSetting()
    {
        this.transform.Find("AudioSettingPanel").gameObject.SetActive(!this.gameObject.activeSelf);
    }
}
