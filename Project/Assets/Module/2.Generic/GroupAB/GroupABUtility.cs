public static class GroupABUtility
{
    #region 战斗结束插屏广告
    // 章节大于等于2 & 每日看广告次数少于1 & 非付费用户
    public static bool OnShowEndInterstitialAd()
    {
        return false;
    }
    #endregion
}