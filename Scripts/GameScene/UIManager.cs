using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public EnemyUnit EnemyBase;
    public PlayerUnit PlayerBase;

    public Slider PlayerSlider;
    public Slider EnemySlider;

    public GameObject PauseMenu;

    public GameObject AudioSettingPanel;

    public GameObject[] EnemyUnits;

    public GameObject[] PlayerUnitButtons = new GameObject[3];

    public TextMeshProUGUI CostText;
    public bool isWin;
    public bool isLose;

    public GameObject ResultPanel;
    public bool isPause = false;
    public int Cost;

    public AudioSource ResultAudio;

    public AudioClip WinAudio;
    public AudioClip LoseAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1; 
        Cost = 15;
        ResultPanel.SetActive(false);
        PauseMenu.SetActive(false);
      //  AudioSettingPanel = GameObject.Find("AudioCanvas");
      foreach(GameObject obj in FindObjectsByType<GameObject>())
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
        StartCoroutine(CostUp());
        StartCoroutine(SpawnEnemy());
        for(int i = 0; i < GameManager.instance.SelectUnitSpriteNumber.Count; i++)
        {
            PlayerUnitButtons[i].transform.Find("UnitImage").GetComponentInChildren<Image>().sprite = GameManager.instance.UnitSprites[GameManager.instance.SelectUnitSpriteNumber[i]];
            PlayerUnitButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = GameManager.instance.SelectUnitList[i].GetComponent<PlayerUnit>().UnitCost.ToString();
        }
        //PlayerSlider.maxValue = PlayerBase.GetComponent<PlayerUnit>().NowHp;
        //PlayerSlider.value = PlayerBase.GetComponent<PlayerUnit>().NowHp;
        //EnemySlider.maxValue = PlayerBase.GetComponent<EnemyUnit>().NowHp;
        //EnemySlider.value = PlayerBase.GetComponent<EnemyUnit>().NowHp;
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerSlider.value = PlayerBase.GetComponent<PlayerUnit>().NowHp;
        //EnemySlider.value = PlayerBase.GetComponent<EnemyUnit>().NowHp;
        if (EnemyBase.NowHp <=0)
        {
            if(isWin == false)
            {
                Time.timeScale = 0;
                ResultPanel.SetActive(true);
                ResultPanel.GetComponentInChildren<TextMeshProUGUI>().text = "WIN";
                AudioSource[] allAudioSources = FindObjectsByType<AudioSource>();
                foreach(AudioSource audioSource in allAudioSources)
                {
                    audioSource.Stop();
                }
                ResultAudio.clip = WinAudio;
                ResultAudio.Play();
                if (this.gameObject.scene.name == "Stage1")
                {
                    GameManager.instance.Gold += 100;
                    GameManager.instance.StageUnLock[1] = true;
                }
                else if (this.gameObject.scene.name == "Stage2")
                {
                    GameManager.instance.Gold += 150;
                    GameManager.instance.StageUnLock[2] = true;
                }
                else if (this.gameObject.scene.name == "Stage3")
                {
                    GameManager.instance.Gold += 200;
                }
                isWin = true;
            }
          
            
           
        }
        else if (PlayerBase.NowHp <= 0)
        {
            if(isLose == false)
            {
                Time.timeScale = 0;
                ResultPanel.SetActive(true);
                ResultPanel.GetComponentInChildren<TextMeshProUGUI>().text = "LOSE";
                AudioSource[] allAudioSources = FindObjectsByType<AudioSource>();
                foreach (AudioSource audioSource in allAudioSources)
                {
                    audioSource.Stop();
                }
                ResultAudio.clip = LoseAudio;
                ResultAudio.Play();
                isLose = true;
            }
            
         
        }
        if (isWin == false && isLose == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ViewPauseMenu();
                AudioSettingPanel.SetActive(false);

            }
        }
        CostText.text = Cost.ToString();
    }

    public void CreateUnit(int ButtonNumber)
    {
        if (Cost >= GameManager.instance.SelectUnitList[ButtonNumber].GetComponent<PlayerUnit>().UnitCost)
        {
             GameObject PlayerUnit = GameManager.instance.SelectUnitList[ButtonNumber];
            Instantiate(PlayerUnit, new Vector3(-5f, 0f, 0f), Quaternion.identity);
           
            Cost -= GameManager.instance.SelectUnitList[ButtonNumber].GetComponent<PlayerUnit>().UnitCost;
        }
        else
        {
            return;
        }
    }

    public void ViewPauseMenu()
    {
        isPause = !isPause;
        PauseMenu.SetActive(isPause);
        if(isPause == false)
        {
            Time.timeScale = 1f;
           // StartCoroutine(CostUp());
        }
        else if(isPause == true)
        {
            Time.timeScale = 0;
           // StopCoroutine(CostUp());
        }
    }

    public void ViewAudioSettingPanel()
    {
        AudioSettingPanel.SetActive(!AudioSettingPanel.activeSelf);
    }

    IEnumerator CostUp()
    {
        while (true)
        {
            Cost++;
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            int EnemyNumber = 0;
             if (this.gameObject.scene.name == "Stage1")
            {
                EnemyNumber = Random.Range(0, 3);
            }
            else if (this.gameObject.scene.name == "Stage2")
            {
                EnemyNumber = Random.Range(0, 6);
            }
            else if (this.gameObject.scene.name == "Stage3")
            {
                EnemyNumber = Random.Range(3, 6);
            }

            GameObject Enemy = Instantiate(EnemyUnits[EnemyNumber], new Vector3(5f, 0f, 0f), Quaternion.identity);
            yield return new WaitForSeconds(15f);
        }
    }

    public void ChangeScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
