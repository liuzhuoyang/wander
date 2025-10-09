using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;

//音效播放管理
public static class AudioHandler
{
    public static void PlayUIPop()
    {
        AudioControl.Instance.PlaySFX("sfx_ui_pop_1");
    }

    //物品掉落音效
    public static void OnPlayItemDropSFX(string itemName)
    {
        ItemData itemArgs = AllItem.dictData[itemName];
        string sfxName = AllItem.dictData[itemName].sfxDrop;

        //没有定义的，都播放item
        if (string.IsNullOrEmpty(itemArgs.sfxDrop))
            sfxName = "sfx_item_drop_generic";

        AudioControl.Instance.PlaySFX(sfxName);
    }

    //物品收集音效
    public static void OnPlayItemCollectSFX(string itemName)
    {
        ItemData itemArgs = AllItem.dictData[itemName];
        string sfxName = AllItem.dictData[itemName].sfxCollect;

        //没有定义的，都播放item
        if (string.IsNullOrEmpty(itemArgs.sfxCollect))
            sfxName = "sfx_item_collect_generic";

        AudioControl.Instance.PlaySFX(sfxName);
    }
}

public class AudioControl : Singleton<AudioControl>
{
    #region Fileds 字段
    // 背景音乐音量
    float bgmVolume = -10f;
    // 环境音量
    float ambientVolume = -14f;
    // 音效音量
    float sfvVolume = 0;

    // 是否开启音效
    bool isSound;
    // 是否开启音乐
    bool isMusic;

    // 背景音乐音源
    AudioSource m_bgm;
    // 环境音源
    AudioSource m_ambient;
    // 音效音源
    // AudioSource m_sfx;
    SFXPlayerManager sfxPlayerManager;

    // 音频混合器
    public AudioMixer audioMixer;
    // 背景音乐混合器组
    public AudioMixerGroup audioGroupBGM;
    // 音效混合器组
    public AudioMixerGroup audioGroupSFX;
    // 环境音效混合器组
    public AudioMixerGroup audioGroupAmbient;

    #endregion

    #region 初始化
    public void Init()
    {
        m_bgm = this.gameObject.AddComponent<AudioSource>();
        m_bgm.outputAudioMixerGroup = audioGroupBGM;
        m_bgm.loop = true;
        m_bgm.playOnAwake = false;

        m_ambient = this.gameObject.AddComponent<AudioSource>();
        m_ambient.outputAudioMixerGroup = audioGroupAmbient;
        m_ambient.loop = true;
        m_ambient.playOnAwake = false;

        // m_sfx = this.gameObject.AddComponent<AudioSource>();
        // m_sfx.outputAudioMixerGroup = audioGroupSFX;

        sfxPlayerManager = new SFXPlayerManager();
        sfxPlayerManager.Init(this);

        bool isSoundOn = PlayerPrefs.GetInt("sound", 1) == 1;
        bool isMusicOn = PlayerPrefs.GetInt("music", 1) == 1;

        if (isMusicOn)
            UnMuteMusic();
        else
            MuteMusic();

        if (isSoundOn)
            UnMuteSound();
        else
            MuteSound();
    }
    #endregion

    #region 声音控制方法
    public void MuteMusic()
    {
        isMusic = false;
        float bVolume = isMusic ? bgmVolume : -80f;
        float aVolume = isMusic ? ambientVolume : -80f;
        audioMixer.SetFloat("group_bgm", bVolume);
        audioMixer.SetFloat("group_ambient", aVolume);
    }

    public void UnMuteMusic()
    {
        isMusic = true;
        float bVolume = isMusic ? bgmVolume : -80f;
        float aVolume = isMusic ? ambientVolume : -80f;
        audioMixer.SetFloat("group_bgm", bVolume);
        audioMixer.SetFloat("group_ambient", aVolume);
        RestartBGM();
    }

    public void MuteSound()
    {
        isSound = false;
        float volume = isSound ? sfvVolume : -80f;
        audioMixer.SetFloat("group_sfx", volume);
    }

    public void UnMuteSound()
    {
        isSound = true;
        float volume = isSound ? sfvVolume : -80f;
        audioMixer.SetFloat("group_sfx", volume);
    }
    #endregion

    #region BGM环境音播放方法
    public async void PlayAmbient(string audioName)
    {
        if (string.IsNullOrEmpty(audioName))
        {
            m_ambient.Stop();
            return;
        }

        //判断这个路径的音乐是否当前的
        //当前正在播放的音乐文件
        string oldName;
        if (m_ambient.clip == null)
            oldName = "";
        else
            oldName = m_ambient.clip.name;

        if (oldName != audioName)
        {
            m_ambient.clip = await GameAsset.GetAudioAsync(audioName);

            if (m_ambient.clip != null)
                m_ambient.Play();
        }
    }

    //播放音乐
    public async void PlayBGM(string audioName)
    {
    #if DEBUG_MESSAGE_ON
        Debug.Log("=== Sound: play bgm: " + audioName);
    #endif
        m_bgm.clip = await GameAsset.GetAudioAsync(audioName);
        if (m_bgm.clip != null)
            m_bgm.Play();
    }

    //停止音乐
    public void StopBGM()
    {
        m_bgm.Stop();
        m_bgm.clip = null;
    }

    void RestartBGM()
    {
        m_bgm.Play();
    }
    #endregion

    #region 音效播放
    public void PlaySFX(string clipName, float volumeMultiplier = 1)
    {
        sfxPlayerManager.PlaySFX(clipName, volumeMultiplier);
    }

    public void StopSFX(string clipName)
    {
        sfxPlayerManager.StopSFX(clipName);
    }

    public void StopAllSFX()
    {
        sfxPlayerManager.StopAllSFX();
    }

    /*
    private IEnumerator TrackSFXInstance(string clipName, float duration)
    {
        // 等待音效播放完成后，减少实例计数
        yield return new WaitForSeconds(duration);
        if (sfxInstanceCount.ContainsKey(clipName))
        {
            sfxInstanceCount[clipName]--;
            if (sfxInstanceCount[clipName] <= 0)
            {
                sfxInstanceCount.Remove(clipName);
            }
        }
    }
    */
    #endregion

    #region 音效播放器封装类
    public class SFXPlayerManager
    {
        // 最大实例数
        const int MaxInstances = 12;

        List<SFXPlayer> sfxPlayerList;
        //初始化播放器
        public void Init(AudioControl audioControl)
        {
            sfxPlayerList = new List<SFXPlayer>();
            for (int i = 0; i < MaxInstances; i++)
            {
                SFXPlayer player = new SFXPlayer();
                GameObject playerObject = new GameObject("SFXPlayer");
                playerObject.transform.SetParent(audioControl.transform);
                player.source = playerObject.AddComponent<AudioSource>();
                player.source.outputAudioMixerGroup = audioControl.audioGroupSFX;
                sfxPlayerList.Add(player);
            }
        }

        public void PlaySFX(string clipName, float volumeMultiplier = 1)
        {
            SFXPlayer player = GetActivePlayer(clipName);
            player.PlaySFX(clipName, volumeMultiplier);
        }

        public void StopSFX(string clipName)
        {
            SFXPlayer player = sfxPlayerList.Find(p => p.IsPlaying() && p.clipName == clipName);
            if (player != null)
                player.Stop();
        }

        public void StopAllSFX()
        {
            foreach (SFXPlayer player in sfxPlayerList)
            {
                player.Stop();
            }
        }

        SFXPlayer GetActivePlayer(string clipName)
        {
            //如果已经有播放器正在播放这个音效，则返回这个播放器
            SFXPlayer activePlayers = sfxPlayerList.Find(p => p.IsPlaying() && p.clipName == clipName);
            if (activePlayers != null)
            {
                return activePlayers;
            }

            //如果没有任何播放器正在播放这个音效，则返回第一个可用的播放器
            List<SFXPlayer> availablePlayers = sfxPlayerList.FindAll(p => !p.IsPlaying());
            if (availablePlayers.Count > 0)
            {
                return availablePlayers[0];
            }
            else
            {
            #if DEBUG_MESSAGE_ON
                Debug.LogWarning($"=== AudioControl: 没有可用的音效播放器，可能因为超出最大同时播放限制,自动选择第一个播放器 ===");
            #endif
                sfxPlayerList[0].Stop();
                return sfxPlayerList[0];
            }
        }
    }

    //音效播放器
    public class SFXPlayer
    {
        const float MinInterval = 0.1f;

        public AudioSource source;
        public string clipName;
        public float lastPlayTime;

        public void PlaySFX(string clipName, float volumeMultiplier = 1)
        {
            if (string.IsNullOrEmpty(clipName))
                return;
            float currentTime = Time.time;

            this.clipName = clipName;
            // 检查上次播放时间，确保播放间隔不小于最小间隔
            if ((currentTime - lastPlayTime) < MinInterval)
            {
                return;
            }

            try
            {
                // 获取音频剪辑并播放
                AudioClip clip = GameAssetGenericManager.Instance.GetAudioClip(this.clipName);
                if (clip != null)
                {
                    source.PlayOneShot(clip, volumeMultiplier);
                    lastPlayTime = currentTime;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"=== Audio: '{clipName}' error: {ex.Message} ===");
            }
        }

        public bool IsPlaying()
        {
            return source.isPlaying;
        }

        public void Stop()
        {
            source.Stop();
        }
    }
    #endregion
}
