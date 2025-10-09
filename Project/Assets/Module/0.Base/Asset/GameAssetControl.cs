using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;

public static class GameAssetControl
{
    public static async void AssignIcon(string targetName, Image target)
    {
        await AssignSpriteAsync(targetName, target, "icon_");
    }

    public static async void AssignIcon(string targetName, Image target, Action callback)
    {
        await AssignSpriteAsync(targetName, target, "icon_", callback);
    }

    public static async void AssignPicture(string targetName, Image target)
    {
        await AssignSpriteAsync(targetName, target, "pic_");
    }

    public static async void AssignSpriteUI(string targetName, Image target)
    {
        await AssignSpriteAsync(targetName, target);
    }

    public static async void AssignSpriteUI(string targetName, Image target, Action callback)
    {
        await AssignSpriteAsync(targetName, target, "", callback);
    }

    async static UniTask AssignSpriteAsync(string targetName, Image target, string prefix = "", Action callback = null)
    {
        if (target == null) return;
        
        target.sprite = Resources.Load<Sprite>("sprite/misc/pixel_alpha");
        Sprite result = await GameAsset.GetSpriteAsync(prefix + targetName);
        
        // 再次检查target是否还存在
        if (target == null)
        {
            Debug.LogWarning($"Image(UI) Target is destroied for {targetName}");
            return;
        }
        
        target.sprite = result;
        callback?.Invoke();
    }
}