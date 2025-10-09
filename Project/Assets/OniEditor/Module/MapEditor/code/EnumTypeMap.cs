//地图对象类型，编辑器放置物件时候，生成地图创建对象时候，得知道是什么类型，来做对应的操作
public enum MapObjectType
{
    None,
    Terrain,    //地形
    VFX,        //特效
    SpawnPoint, //出生点
    Collider,   //碰撞体
    TowerDefensePoint //防御塔
}


public enum TerrainLayer
{
    BG,
    Tile = 0,
    Hole = 1,
    Surface = 2,
    Detail = 3,
    Cliff = 4,
    Scatter = 5,
    Decor = 6,
    Block = 7,
}

public enum FeaturePointType
{
    Spawn,
    Supply,
}

//出怪点路线类型
public enum SpawnPointRouteType
{
    Main = 0,   //主路线
    Sub = 1,    //副路线
    Random = 2, //随机路线
    Fixed = 3,  //固定路线
    Boss = 4,  //Boss创建点位
}