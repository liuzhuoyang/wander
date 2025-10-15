using UnityEngine;

   [CreateAssetMenu(fileName = "all_formation_item", menuName = "Formation/FormationItemDataCollection")]
    public class FormationItemDataCollection : DataCollection<FormationItemData>
    {
        public override FormationItemData GetDataByKey(string key) => DataList.Find(x => x.itemName == key);
    }