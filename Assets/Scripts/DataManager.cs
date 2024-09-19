using System;
using System.Linq;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    [SerializeField] private RecourcesInfo[] ObjectData;
    public FlyingObject flyingObject; 
    public MoveText moveText;
    public RecourcesInfo GetDataResources(ResourcesType resourcesType)
    {

        return ObjectData.FirstOrDefault(a => a.resourcesType == resourcesType);
    }
}
[Serializable]
public class RecourcesInfo 
{
   public Sprite icon;
   public GameObject model;
   public ResourcesType resourcesType;
}

public enum ResourcesType
{
    Tree,
    Stone,
    Ice,
    Cactus,
    Diamond
}
