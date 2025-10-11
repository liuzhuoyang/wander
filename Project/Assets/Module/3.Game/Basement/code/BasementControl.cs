using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RTSDemo.Grid;
using BattleActor.Basement.Skill;
using Sirenix.OdinInspector;

namespace BattleActor.Basement
{
    public class BasementControl : Singleton<BasementControl>
    {
        [SerializeField] private BasementDataCollection basementDataCollection;
        [SerializeField] private GameObject ui_activeskill_caster_prefab;
        [SerializeField] private Transform ui_root;
        private Transform basementRoot;
        private GameObject basementwall_prefab;
        private BasementBasic currentBasement;
        private Dictionary<Vector2Int, BasementWall> DictWallGridCoord;

        //墙体坐标
        private int minY;
        private int maxY;
        private int minX;
        private int maxX;

        public BasementBasic m_currentBasement => currentBasement;

        private const string DECAL_EDGE_SPRITE_KEY = "basement_decal_edge";
        private const string DECAL_CORNER_SPRITE_KEY = "basement_decal_corner";

        public async Task Init()
        {
            basementRoot = new GameObject("[Basement]").transform;
            DictWallGridCoord = new Dictionary<Vector2Int, BasementWall>();
            basementwall_prefab = await GameAsset.GetPrefabAsync("battle_base_wall");
        }
        public void CleanUp()
        {
            Destroy(basementRoot.gameObject);
            basementwall_prefab = null;
        }
        public void CreateBasement(string basementKey, Vector2Int lbPoint, Vector2Int gridSize)
        {
            minX = lbPoint.x;
            minY = lbPoint.y;
            maxX = lbPoint.x + gridSize.x - 1;
            maxY = lbPoint.y + gridSize.y - 1;

            Vector2 lbPos = RTS_GridWorldSystem.Instance.GetWorldPosFromGrid(lbPoint - Vector2Int.one);
            Vector2 rtPos = RTS_GridWorldSystem.Instance.GetWorldPosFromGrid(new Vector2Int(maxX, maxY) + Vector2Int.one);
            Vector2 basementSize = rtPos - lbPos;

            var basementObj = new GameObject("Basement");
            basementObj.transform.SetParent(basementRoot);
            basementObj.transform.position = new Vector3((lbPos.x + rtPos.x) * 0.5f, (lbPos.y + rtPos.y) * 0.5f, 0);

            currentBasement = basementObj.AddComponent<BasementBasic>();
            currentBasement.Init(basementDataCollection.GetDataByKey(basementKey), basementSize);

            CreateWall(basementKey, new RectInt(minX, minY, gridSize.x - 1, gridSize.y - 1));
        }

        //创建显示对象
        //isPreview: 是否为预览, 拖动拓展格子展开时为预览模式
        public void CreateViewObject(string basementKey, int x, int y, Vector2 pos, BaseWallDirection direction, bool isPreview)
        {
            GameObject viewObj;
            viewObj = Instantiate(basementwall_prefab, currentBasement.transform);

            (int xCoord, int yCoord) = GetAdjustCoordinates(x, y, direction);

            BasementWall wall = viewObj.GetComponent<BasementWall>();
            bool isCorner = IsCorner(direction);
            string wallSprite = isCorner ? basementKey + "_wall_corner" : basementKey + "_wall_edge";
            string decalSprite = isCorner ? DECAL_CORNER_SPRITE_KEY : DECAL_EDGE_SPRITE_KEY;

            wall.Init(xCoord, yCoord, wallSprite, decalSprite, pos, direction, isPreview);
            wall.OnRefreView();

            DictWallGridCoord.Add(new Vector2Int(xCoord, yCoord), wall);
        }
        //清理墙体，根据旧的边界值，清理边界外的墙体
        private void ClearWall(int oldMinX, int oldMaxX, int oldMinY, int oldMaxY, bool isPreview = false)
        {
            // 右边界
            if (oldMaxX != maxX || isPreview)
            {
                for (int y = oldMinY; y <= oldMaxY; y++)
                {
                    if (DictWallGridCoord.ContainsKey(new Vector2Int(oldMaxX + 1, y)))
                    {
                        DictWallGridCoord[new Vector2Int(oldMaxX + 1, y)].OnKill();
                        DictWallGridCoord.Remove(new Vector2Int(oldMaxX + 1, y));
                    }
                }
                // 清理右上角
                if (DictWallGridCoord.ContainsKey(new Vector2Int(oldMaxX + 1, oldMaxY + 1)))
                {
                    DictWallGridCoord[new Vector2Int(oldMaxX + 1, oldMaxY + 1)].OnKill();
                    DictWallGridCoord.Remove(new Vector2Int(oldMaxX + 1, oldMaxY + 1));
                }
                // 清理右下角
                if (DictWallGridCoord.ContainsKey(new Vector2Int(oldMaxX + 1, oldMinY - 1)))
                {
                    DictWallGridCoord[new Vector2Int(oldMaxX + 1, oldMinY - 1)].OnKill();
                    DictWallGridCoord.Remove(new Vector2Int(oldMaxX + 1, oldMinY - 1));
                }
            }

            // 左边界
            if (oldMinX != minX || isPreview)
            {
                for (int y = oldMinY; y <= oldMaxY; y++)
                {
                    if (DictWallGridCoord.ContainsKey(new Vector2Int(oldMinX - 1, y)))
                    {
                        DictWallGridCoord[new Vector2Int(oldMinX - 1, y)].OnKill();
                        DictWallGridCoord.Remove(new Vector2Int(oldMinX - 1, y));
                    }
                }
                // 清理左上角
                if (DictWallGridCoord.ContainsKey(new Vector2Int(oldMinX - 1, oldMaxY + 1)))
                {
                    DictWallGridCoord[new Vector2Int(oldMinX - 1, oldMaxY + 1)].OnKill();
                    DictWallGridCoord.Remove(new Vector2Int(oldMinX - 1, oldMaxY + 1));
                }
                // 清理左下角
                if (DictWallGridCoord.ContainsKey(new Vector2Int(oldMinX - 1, oldMinY - 1)))
                {
                    DictWallGridCoord[new Vector2Int(oldMinX - 1, oldMinY - 1)].OnKill();
                    DictWallGridCoord.Remove(new Vector2Int(oldMinX - 1, oldMinY - 1));
                }
            }

            // 下边界
            if (oldMinY != minY || isPreview)
            {
                for (int x = oldMinX; x <= oldMaxX; x++)
                {
                    if (DictWallGridCoord.ContainsKey(new Vector2Int(x, oldMinY - 1)))
                    {
                        DictWallGridCoord[new Vector2Int(x, oldMinY - 1)].OnKill();
                        DictWallGridCoord.Remove(new Vector2Int(x, oldMinY - 1));
                    }
                }
                // 清理左下角
                if (DictWallGridCoord.ContainsKey(new Vector2Int(oldMinX - 1, oldMinY - 1)))
                {
                    DictWallGridCoord[new Vector2Int(oldMinX - 1, oldMinY - 1)].OnKill();
                    DictWallGridCoord.Remove(new Vector2Int(oldMinX - 1, oldMinY - 1));
                }
                // 清理右下角
                if (DictWallGridCoord.ContainsKey(new Vector2Int(oldMaxX + 1, oldMinY - 1)))
                {
                    DictWallGridCoord[new Vector2Int(oldMaxX + 1, oldMinY - 1)].OnKill();
                    DictWallGridCoord.Remove(new Vector2Int(oldMaxX + 1, oldMinY - 1));
                }
            }

            // 上边界
            if (oldMaxY != maxY || isPreview)
            {
                for (int x = oldMinX; x <= oldMaxX; x++)
                {
                    if (DictWallGridCoord.ContainsKey(new Vector2Int(x, oldMaxY + 1)))
                    {
                        DictWallGridCoord[new Vector2Int(x, oldMaxY + 1)].OnKill();
                        DictWallGridCoord.Remove(new Vector2Int(x, oldMaxY + 1));
                    }
                }
                // 清理左上角
                if (DictWallGridCoord.ContainsKey(new Vector2Int(oldMinX - 1, oldMaxY + 1)))
                {
                    DictWallGridCoord[new Vector2Int(oldMinX - 1, oldMaxY + 1)].OnKill();
                    DictWallGridCoord.Remove(new Vector2Int(oldMinX - 1, oldMaxY + 1));
                }
                // 清理右上角
                if (DictWallGridCoord.ContainsKey(new Vector2Int(oldMaxX + 1, oldMaxY + 1)))
                {
                    DictWallGridCoord[new Vector2Int(oldMaxX + 1, oldMaxY + 1)].OnKill();
                    DictWallGridCoord.Remove(new Vector2Int(oldMaxX + 1, oldMaxY + 1));
                }
            }
        }
        //创建墙体，检查矩阵边界格子，根据格子坐标创建对应的墙体
        //这里顺序也会影响动画创建顺序
        private void CreateWall(string basementKey, RectInt basementRect, bool isPreview = false)
        {
            float halfCell = RTS_GridWorldSystem.Instance.GetGridWidth() * 0.5f;
            // 添加左上角的墙体
            if (!DictWallGridCoord.ContainsKey(new Vector2Int(basementRect.xMin, basementRect.yMax)))
            {
                CreateViewObject(basementKey, basementRect.xMin, basementRect.yMax, RTS_GridWorldSystem.Instance.GetGridNode(basementRect.xMin, basementRect.yMax).worldPos + new Vector2(-halfCell, halfCell),
                    BaseWallDirection.CornerTopLeft, isPreview);
            }

            // 检查左侧边缘是否有缺漏
            for (int y = basementRect.yMax; y >= basementRect.yMin; y--)
            {
                if (!DictWallGridCoord.ContainsKey(new Vector2Int(basementRect.xMin, y)))
                {
                    CreateViewObject(basementKey, basementRect.xMin, y, RTS_GridWorldSystem.Instance.GetGridNode(basementRect.xMin, y).worldPos + new Vector2(-halfCell * 2, 0),
                        BaseWallDirection.Left, isPreview);
                }
            }

            // 添加左下角的墙体
            if (!DictWallGridCoord.ContainsKey(new Vector2Int(basementRect.xMin, basementRect.yMin)))
            {
                CreateViewObject(basementKey, basementRect.xMin, basementRect.yMin, RTS_GridWorldSystem.Instance.GetGridNode(basementRect.xMin, basementRect.yMin).worldPos + new Vector2(-halfCell, -halfCell),
                    BaseWallDirection.CornerBottomLeft, isPreview);
            }

            // 检查底部边缘是否有缺漏
            for (int x = basementRect.xMin; x <= basementRect.xMax; x++)
            {
                if (!DictWallGridCoord.ContainsKey(new Vector2Int(x, basementRect.yMin)))
                {
                    CreateViewObject(basementKey, x, basementRect.yMin, RTS_GridWorldSystem.Instance.GetGridNode(x, basementRect.yMin).worldPos + new Vector2(0, -halfCell * 2),
                        BaseWallDirection.Bottom, isPreview);
                }
            }

            // 添加右下角的墙体
            if (!DictWallGridCoord.ContainsKey(new Vector2Int(basementRect.xMax, basementRect.yMin)))
            {
                CreateViewObject(basementKey, basementRect.xMax, basementRect.yMin, RTS_GridWorldSystem.Instance.GetGridNode(basementRect.xMax, basementRect.yMin).worldPos + new Vector2(halfCell, -halfCell),
                    BaseWallDirection.CornerBottomRight, isPreview);
            }

            // 检查右侧边缘是否有缺漏
            for (int y = basementRect.yMin; y <= basementRect.yMax; y++)
            {
                if (!DictWallGridCoord.ContainsKey(new Vector2Int(basementRect.xMax, y)))
                {
                    CreateViewObject(basementKey, basementRect.xMax, y, RTS_GridWorldSystem.Instance.GetGridNode(basementRect.xMax, y).worldPos + new Vector2(halfCell * 2, 0),
                        BaseWallDirection.Right, isPreview);
                }
            }

            // 添加右上角的墙体
            if (!DictWallGridCoord.ContainsKey(new Vector2Int(basementRect.xMax, basementRect.yMax)))
            {
                CreateViewObject(basementKey, basementRect.xMax, basementRect.yMax, RTS_GridWorldSystem.Instance.GetGridNode(basementRect.xMax, basementRect.yMax).worldPos + new Vector2(halfCell, halfCell),
                    BaseWallDirection.CornerTopRight, isPreview);
            }

            // 检查顶部边缘是否有缺漏
            for (int x = basementRect.xMax; x >= basementRect.xMin; x--)
            {
                if (!DictWallGridCoord.ContainsKey(new Vector2Int(x, basementRect.yMax)))
                {
                    CreateViewObject(basementKey, x, basementRect.yMax, RTS_GridWorldSystem.Instance.GetGridNode(x, basementRect.yMax).worldPos + new Vector2(0, halfCell * 2),
                        BaseWallDirection.Top, isPreview);
                }
            }
            // 播放创建动画
            List<BasementWall> listWallByOrder = DictWallGridCoord.Values.ToList();
            for (int i = 0; i < listWallByOrder.Count; i++)
            {
                listWallByOrder[i].OnShowAnimation();
            }
        }

        #region 辅助方法
        private bool IsCorner(BaseWallDirection baseWallDirection) => baseWallDirection == BaseWallDirection.CornerTopLeft
            || baseWallDirection == BaseWallDirection.CornerTopRight
            || baseWallDirection == BaseWallDirection.CornerBottomLeft
            || baseWallDirection == BaseWallDirection.CornerBottomRight;
        //获得墙体坐标根据格子的偏移，比如格子(1,1)，墙体方向为Left，则墙体坐标为(0,1)
        private (int, int) GetAdjustCoordinates(int x, int y, BaseWallDirection direction)
        {
            switch (direction)
            {
                case BaseWallDirection.Left:
                    x -= 1;
                    break;
                case BaseWallDirection.Right:
                    x += 1;
                    break;
                case BaseWallDirection.Top:
                    y += 1;
                    break;
                case BaseWallDirection.Bottom:
                    y -= 1;
                    break;
                case BaseWallDirection.CornerTopLeft:
                    x -= 1;
                    y += 1;
                    break;
                case BaseWallDirection.CornerTopRight:
                    x += 1;
                    y += 1;
                    break;
                case BaseWallDirection.CornerBottomLeft:
                    x -= 1;
                    y -= 1;
                    break;
                case BaseWallDirection.CornerBottomRight:
                    x += 1;
                    y -= 1;
                    break;
            }
            return (x, y);
        }
        #endregion

        #region 基地技能
        public void OnChooseAbilityLocation(Action<Vector2> onLocationChosen)
        {
            Instantiate(ui_activeskill_caster_prefab, ui_root).GetComponent<UI_BasementSkillCaster>().Init(onLocationChosen);
        }
        #endregion
    }
}