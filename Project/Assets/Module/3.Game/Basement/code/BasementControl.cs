using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RTSDemo.Grid;
using RTSDemo.Basement.Skill;

namespace RTSDemo.Basement
{
    public class BasementControl : Singleton<BasementControl>
    {
        [SerializeField] private BasementDataCollection basementDataCollection;
        [SerializeField] private GameObject ui_activeskill_caster_prefab;
        private Transform basementRoot;
        private BasementBasic currentBasement;

        public BasementBasic m_currentBasement => currentBasement;

        public async Task Init()
        {
            basementRoot = new GameObject("[Basement]").transform;
        }
        public void CleanUp()
        {
            Destroy(basementRoot.gameObject);
        }
        public async Task CreateBasement(string basementKey, Vector2 center)
        {
            var basementData = basementDataCollection.GetDataByKey(basementKey);
            var basementObj = Instantiate(await GameAsset.GetPrefabAsync(basementKey, basementData.basementPrefab));
            basementObj.transform.SetParent(basementRoot);
            basementObj.transform.position = center;

            currentBasement = basementObj.AddComponent<BasementBasic>();
            currentBasement.Init(basementData);
        }

        #region 基地技能
        public void OnChooseAbilityTargetPosition(Action<Vector2> onLocationChosen)
        {
            //@todo 这里可以采用Popup的方式呼出技能发射菜单替代
            Instantiate(ui_activeskill_caster_prefab).GetComponent<UI_BasementSkillCaster>().Init(onLocationChosen);
        }
        #endregion
    }
}