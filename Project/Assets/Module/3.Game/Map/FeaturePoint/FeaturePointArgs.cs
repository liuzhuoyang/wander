using System;

[Serializable]
public class FeaturePointArgs
{
    public float posX;
    public float posY;
}

[Serializable]
public class SupplyPointArgs : FeaturePointArgs{}

[Serializable]
public class DefenseTowerPointArgs : FeaturePointArgs
{
    public string towerKey;
    public bool isActiveOnStart;
}

