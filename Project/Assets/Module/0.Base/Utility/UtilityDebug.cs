using UnityEngine;

public static class UtilityDebug 
{
   /// <summary>
   /// 获取Transform的完整路径，格式为：父节点/父节点/当前节点
   /// </summary>
   /// <param name="target">目标Transform</param>
   /// <returns>完整路径字符串</returns>
   public static string GetFullTransformPath(Transform target)
   {
       if (target == null) return "null";
       
       string path = target.name;
       Transform parent = target.parent;
       
       while (parent != null)
       {
           path = parent.name + "/" + path;
           parent = parent.parent;
       }
       
       return path;
   }
}
