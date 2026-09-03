using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static AudioManager;

public class GameManager : MonoBehaviour
{
    public string path;
    public static GameManager instance = null;

    public AudioManager audioManager;
    public List<GameObject> SelectUnitList = new List<GameObject>();

    public Sprite[] UnitSprites = new Sprite[6];
    public bool[] UnitUnLocks = new bool[6];
   // public int[] SelectUnitSpriteNumber = new int[3];
    public List<int> SelectUnitSpriteNumber = new List<int>();
    public bool[] StageUnLock = new bool[3];

    public int Gold;
    public float MasterVolume;
    public float BGMVolume;
    public float SFXVolume;

    public Slider[] SoundSliders;

    public int ScreenWidthSize;
    public int ScreenHeightSize;
    public bool isFullScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [System.Serializable]
    public class SaveData
    {
        // public List<GameObject> SelectUnitList = new List<GameObject>();
        public List<string> SelectUnitStringList = new List<string>();
        public bool[] UnitUnLocks = new bool[6];
        public List<int> SelectUnitSpriteNumber = new List<int>();

        public bool[] StageUnLock = new bool[3];

        public float MasterVolume;
        public float BGMVolume;
        public float SFXVolume;
        public int ScreenWidthSize;
        public int ScreenHeightSize;
        public bool isFullScreen;

        public int Gold;
    }

    private void Awake()
    {
        if( null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
       
        for(int i = 0; i<3; i++)
        {
            UnitUnLocks[i] = true;
        }
        StageUnLock[0] = true;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        path = Path.Combine(Application.dataPath, "Game_data.json");
        JsonLoad();
        audioManager.SetAudioVolume(AudioMixerType.Master, MasterVolume);
        audioManager.SetAudioVolume(AudioMixerType.BGM, BGMVolume);
        audioManager.SetAudioVolume(AudioMixerType.SFX, SFXVolume);
        SoundSliders[0].value = MasterVolume;
        SoundSliders[1].value = BGMVolume;
        SoundSliders[2].value = SFXVolume;
        Screen.SetResolution(ScreenWidthSize, ScreenHeightSize, isFullScreen);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {

            JsonSave();
        }
    }

  

    public void JsonSave()
    {
        SaveData saveData = new SaveData();

        for (int i = 0; i < SelectUnitList.Count; i++)
        {
            saveData.SelectUnitStringList.Add(SelectUnitList[i].gameObject.name);
        }
        for (int i = 0; i < SelectUnitSpriteNumber.Count; i++)
        {
            saveData.SelectUnitSpriteNumber.Add(SelectUnitSpriteNumber[i]);
        }
        for (int i = 0; i < UnitUnLocks.Length; i++)
        {
            saveData.UnitUnLocks[i] = UnitUnLocks[i];
        }
        for (int i = 0; i < StageUnLock.Length; i++)
        {
            saveData.StageUnLock[i] = StageUnLock[i];
        }
        saveData.Gold = Gold;
        saveData.MasterVolume = MasterVolume;
        saveData.BGMVolume = BGMVolume;
        saveData.SFXVolume = SFXVolume;
        saveData.ScreenWidthSize = ScreenWidthSize;
        saveData.ScreenHeightSize = ScreenHeightSize;
        saveData.isFullScreen = isFullScreen;

        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(path, json);
    }

    public void JsonLoad()
    {
        SaveData saveData = new SaveData();

        if(!File.Exists(path))
        {
            SelectUnitList.Add(Resources.Load<GameObject>("Prefabs/Blue_Archer"));
            SelectUnitList.Add(Resources.Load<GameObject>("Prefabs/Blue_Shielder"));
            SelectUnitList.Add(Resources.Load<GameObject>("Prefabs/Blue_Sword"));
            SelectUnitSpriteNumber.Add(0);
            SelectUnitSpriteNumber.Add(1);
            SelectUnitSpriteNumber.Add(2);
            UnitUnLocks[0] = true;
            UnitUnLocks[1] = true;
            UnitUnLocks[2] = true;
            UnitUnLocks[3] = false;
            UnitUnLocks[4] = false;
            UnitUnLocks[5] = false;
            StageUnLock[0] = true;
            StageUnLock[1] = false;
            StageUnLock[2] = false;
            Gold = 50;
            MasterVolume = 0.5f;
            BGMVolume = 0.5f;
            SFXVolume = 0.5f;
            ScreenWidthSize = 1920;
            ScreenHeightSize = 1080;
            isFullScreen = false;
            JsonSave();
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);
            if(saveData != null)
            {
                for(int i = 0; i<saveData.SelectUnitStringList.Count; i++)
                {
                    SelectUnitList.Add(Resources.Load<GameObject>("Prefabs/" + saveData.SelectUnitStringList[i]));
                }
                for (int i = 0; i < saveData.SelectUnitSpriteNumber.Count; i++)
                {
                    SelectUnitSpriteNumber.Add(saveData.SelectUnitSpriteNumber[i]);
                }
                for (int i = 0; i < saveData.UnitUnLocks.Length; i++)
                {
                    UnitUnLocks[i] = saveData.UnitUnLocks[i];
                }
                for (int i = 0; i < saveData.StageUnLock.Length; i++)
                {
                    StageUnLock[i] = saveData.StageUnLock[i];
                }
                Gold = saveData.Gold;
                MasterVolume = saveData.MasterVolume;
                BGMVolume = saveData.BGMVolume;
                SFXVolume = saveData.SFXVolume;
                ScreenWidthSize = saveData.ScreenWidthSize;
                ScreenHeightSize = saveData.ScreenHeightSize;
                isFullScreen = saveData.isFullScreen;
            }
        }
    }
}
