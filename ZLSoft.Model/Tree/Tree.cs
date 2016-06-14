using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLSoft.Model.PUB;
using ZLSoft.Pub;

namespace ZLSoft.Model.Tree
{
    public class Tree
    {
        #region 变量

        /// <summary>
        /// 树结构
        /// </summary>
        public IList<StrObjectDict> data;
        /// <summary>
        /// 原始数据
        /// </summary>
        public IList<StrObjectDict> datasList;

        #endregion

        #region 方法

        /// <summary>
        /// 转化原始数据为树结构
        /// </summary>
        /// <param name="colName">上级ID的字段名称</param>
        public void TransformTree(string colName)
        {
            TransformTree(colName, "ROOT", "Children","ID");
        }

        /// <summary>
        /// 转化原始数据为树结构
        /// </summary>
        /// <param name="colName">上级ID的字段名称</param>
        /// <param name="rootName">Root节点的值</param>
        /// <param name="childName">子节点的名称</param>
        /// <param name="IDName">主键的名称</param>
        /// <returns></returns>
        public List<StrObjectDict> TransformTree(string colName, string rootName, string childName, string IDName)
        {
            //获取根节点
            List<StrObjectDict> roots = new List<StrObjectDict>(this.datasList.FindAllBy(colName, rootName));
            this.data = this.datasList;

            foreach (var item in roots){
                this.BuildTree(item, new List<StrObjectDict>(this.data), colName ,childName,IDName);
            }
            this.data = roots;
            return roots;
          
        }

        /// <summary>
        /// 递归子节点生成树
        /// </summary>
        /// <param name="strObj"></param>
        /// <param name="items"></param>
        /// <param name="colName"></param>
        /// <param name="childName"></param>
        /// <param name="IDName"></param>
        private void BuildTree(StrObjectDict strObj, List<StrObjectDict> items, string colName, string childName, string IDName)
        {
            if (items.Count == 0)
            {
                return;
            }
            else
            {
                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    if (string.Equals(item[colName], strObj[IDName]))
                    {
                        if (!strObj.ContainsKey(childName))
                        {
                            strObj[childName] = new List<StrObjectDict>();
                        }
                        ((List<StrObjectDict>)(strObj[childName])).Add(item);
                        StrObjectDict temp = item;
                        items.Remove(item);
                        i--;
                        BuildTree(temp, items, colName, childName, IDName);
                    }
                    
                }
                
            }
        }



        #endregion




    }
}
