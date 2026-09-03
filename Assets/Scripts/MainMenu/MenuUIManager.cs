using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{

    public GameObject[] UIObjects = new GameObject[4];
    public GameObject AudioSettingPanel;

    public TextMeshProUGUI GoldNumberText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < UIObjects.Length; i++)
        {
            UIObjects[i].SetActive(false);
        }
        // AudioSettingPanel = GameObject.Find("AudioCanvas");
        foreach (GameObject obj in FindObjectsByType<GameObject>())
        {
            if (obj.scene.name == "DontDestroyOnLoad")
            {
                if (obj.gameObject.name == "AudioCanvas")
                {
                    AudioSettingPanel = obj.transform.Find("AudioSettingPanel").gameObject;
                }
            }
        }
        AudioSettingPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void ViewObjects(int ObjectsNumber)
    {
        for(int i = 0; i < UIObjects.Length; i++)
        {
            UIObjects[i].SetActive(false);
        }
        UIObjects[ObjectsNumber].SetActive(true);
    }

    public void ViewStage()
    {
        for(int i = 0; i < UIObjects[0].transform.childCount; i++)
        {
            UIObjects[0].transform.GetChild(i).GetComponent<Button>().interactable = GameManager.instance.StageUnLock[i];

        }
    }

    public void ViewUnit()
    {
        for (int i = 0; i < GameManager.instance.UnitUnLocks.Length; i++)
        {
            UIObjects[1].transform.Find("UnitButton").transform.GetChild(i).transform.Find("Panel").gameObject.SetActive(!GameManager.instance.UnitUnLocks[i]);
        }

        for (int i = 0; i < UIObjects[1].transform.Find("SelectUnitButton").transform.childCount; i++)
        {
            //if (GameManager.instance.SelectUnitSpriteNumber[i] == 999)
            //{
            //    UIObjects[1].transform.Find("SelectUnitButton").transform.GetChild(i).transform.Find("Image").GetComponent<Image>().sprite = null;

            //}
            if (i < GameManager.instance.SelectUnitSpriteNumber.Count)
            {
                UIObjects[1].transform.Find("SelectUnitButton").transform.GetChild(i).transform.Find("Image").GetComponent<Image>().sprite = GameManager.instance.UnitSprites[(int)GameManager.instance.SelectUnitSpriteNumber[i]];
            }
            else
            {
                UIObjects[1].transform.Find("SelectUnitButton").transform.GetChild(i).transform.Find("Image").GetComponent<Image>().sprite = null;
            }
        }
    }

    public void ViewUpgrade()
    {
        for (int i = 0; i < GameManager.instance.UnitUnLocks.Length; i++)
        {
            UIObjects[2].transform.Find("UnitButton").transform.GetChild(i).transform.Find("Panel").gameObject.SetActive(!GameManager.instance.UnitUnLocks[i]);
            if(GameManager.instance.UnitUnLocks[i] == true)
            {
                UIObjects[2].transform.Find("UnitButton").transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
        UIObjects[2].transform.Find("GoldNumberText").GetComponent<TextMeshProUGUI>().text = GameManager.instance.Gold.ToString();
    }
    public void ViewSetting()
    {

    }

    public void UnLcokUnit(int ButtonNumber)
    {
        int.TryParse(UIObjects[2].transform.Find("UnitButton").transform.GetChild(ButtonNumber).GetComponentInChildren<TextMeshProUGUI>().text, out int UnitGold);
        if (UnitGold <= GameManager.instance.Gold)
        {
            if (GameManager.instance.UnitUnLocks[ButtonNumber] == false)
            {
                GameManager.instance.UnitUnLocks[ButtonNumber] = true;
                //int UnitGold = 0;
                //int.TryParse(UIObjects[2].transform.Find("UnitButton").transform.GetChild(ButtonNumber).GetComponentInChildren<TextMeshProUGUI>().text,out int UnitGold);
                GameManager.instance.Gold -= UnitGold;
                ViewUpgrade();
            }
        }
    }

    public void SelectUnitSprite(int UnitNumber)
    {
        if (GameManager.instance.SelectUnitSpriteNumber.Count < 3)
        {
            GameManager.instance.SelectUnitSpriteNumber.Add(UnitNumber);
            ViewUnit();
        }
        ViewUnit();
    }

    public void SelectUnitObject(GameObject PlayerUnit)
    {
        if (GameManager.instance.SelectUnitList.Count < 3)
        {
            GameManager.instance.SelectUnitList.Add(PlayerUnit);
            ViewUnit();
        }
        ViewUnit();
    }

    public void UnSelectUnit(int ButtonNumber)
    {
        if (GameManager.instance.SelectUnitList.Count > ButtonNumber)
        {
            GameManager.instance.SelectUnitList.RemoveAt(ButtonNumber);
            GameManager.instance.SelectUnitSpriteNumber.RemoveAt(ButtonNumber);
            ViewUnit();
        }
        ViewUnit();
    }

    public void UnSelectImageChange(Image image)
    {
        image.sprite = null;
        ViewUnit();
    }

    public void GoStage(int ButtonNumber)
    {

    }

    public void ViewAudioSettingPanel()
    {
        AudioSettingPanel.SetActive(!AudioSettingPanel.activeSelf);
    }
    public void ChangeScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void SaveData()
    {
        GameManager.instance.JsonSave();
    }

}
