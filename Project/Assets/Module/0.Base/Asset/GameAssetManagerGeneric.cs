using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;
using System;
using SimpleAudioSystem;


//经常需要初始化的资源才需要预加载
public class GameAssetManagerGeneric : Singleton<GameAssetManagerGeneric>
{
    public TMP_FontAsset font;
    public Material fontMaterialTitle;
    public Material fontMaterialContent; 

    public Dictionary<string, AudioClip> dictSFXClip;
    public Dictionary<string, AudioClip> dictSFXGroupClip;

    public async UniTask Init()
    {
        await InitFont();
        // await InitVFXAsset(); //后续可优化放到动态加载里
        await InitAudioAsset();
        return;
    }

    async UniTask InitFont()
    {
        //string fontCode = Utility.GetFont(PlayerPrefs.GetString("loc"));
        string fontName = UtilityLocalization.GetFontName();

        //fontName = "font_" + fontCode;
/*  
        //中文字体包含简体和繁体，简体和繁体共用字体
        if (PlayerPrefs.GetString("loc") == "zht" || PlayerPrefs.GetString("loc") == "zhs")
        {
            fontName = "font_zh";
        }
        else
        {
            fontName = "font_" + Utility.GetFont(PlayerPrefs.GetString("loc")); //;
        }*/

        font = await GameAsset.GetAssetAsync<TMP_FontAsset>(fontName);

        fontMaterialTitle = font.material;
        fontMaterialContent = await GameAsset.GetAssetAsync<Material>($"{fontName}_content");
    }

    public void ResetDynamicFont()
    {
        foreach (TMP_FontAsset fallbackFont in font.fallbackFontAssetTable)
        {
            // 重置字体资产的缓存
            fallbackFont.ClearFontAssetData(true);
        }
    }

    #region 读取音频资源
    async UniTask InitAudioAsset()
    {
        dictSFXClip = new Dictionary<string, AudioClip>();
        dictSFXGroupClip = new Dictionary<string, AudioClip>();
        await LoadAsset(AllAudio.dictSFXData.Keys, LoadAudio);
    }

    async UniTask LoadAudio(string audioName)
    {
        AudioData data = AllAudio.dictSFXData[audioName];
        AudioClip clip = await GameAsset.GetAssetAsync<AudioClip>(data.clipName);
        dictSFXGroupClip.Add(audioName, clip);
    }

    async UniTask LoadAudioGroup(string audioName)
    {
        AudioGroupData data = AllAudio.dictSFXGroupData[audioName];
        AudioClip clip = await GameAsset.GetAssetAsync<AudioClip>(data.clipName);
        dictSFXGroupClip.Add(audioName, clip);
    }
    #endregion

    // #region 读取VFX资源
    // Dictionary<string, GameObject> vfxPrefabDict; //特效预制体
    // async UniTask InitVFXAsset()
    // {
    //     vfxPrefabDict = new Dictionary<string, GameObject>();
    //     await LoadAsset(AllVFX.dictData.Keys, LoadVFX);
    // }

    // async UniTask LoadVFX(string vfxName)
    // {
    //     GameObject vfx = await GameAsset.GetAssetAsync<GameObject>(vfxName);
    //     vfxPrefabDict.Add(vfxName, vfx);
    // }

    // public GameObject GetVFXPrefab(string vfxName)
    // {
    //     return vfxPrefabDict[vfxName];
    // }
    // #endregion

/*
    async UniTask LoadUnitPrefab(string key)
    {
        
    }*/

    async UniTask LoadAsset<T>(IEnumerable<T> items, Func<T, UniTask> loadFunc, int batchSize = 50)
    {
        List<UniTask> batchTasks = new List<UniTask>(batchSize);

        foreach (var item in items)
        {
            batchTasks.Add(loadFunc(item));

            if (batchTasks.Count >= batchSize)
            {
                await UniTask.WhenAll(batchTasks);
                batchTasks.Clear();
            }
        }

        if (batchTasks.Count > 0)
        {
            await UniTask.WhenAll(batchTasks);
        }
    }
}