// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Linq;
using DotNet.Auth.Entity;
using DotNet.Auth.Service;
using DotNet.Helper;
using DotNet.Utility;

namespace DotNet
{
    public class TreeTest
    {
        public void Test()
        {
            var list = AuthService.Dic.GetList();
            list.Add(new Dic { Id = "0", ParentId = "-1", Name = "数据字典" });
            var node = TreeHelper.BuildTree(list, "-1").FirstOrDefault();
            Write(node, 1);

            Console.WriteLine("=====================================");
            var child = TreeHelper.GetAllChilds(list, list.FirstOrDefault(p => p.Id.Equals("96")), true);
            foreach (var item in child)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine("=====================================");
            var level = TreeHelper.GetTreeLevel(list, list.FirstOrDefault(p => p.Id.Equals("0")));
            Console.WriteLine($"level={level}");
            //var sortPath = TreeHelper.GetNewNodeSortPath(list, 
            //    list.FirstOrDefault(p=>p.Id.Equals("16")));
            //Console.WriteLine($"sortPath={sortPath}");
        }

        public void Write(TreeNode node, int level)
        {
            Console.WriteLine($"{StringHelper.GetRepeatString("-", level)}{node.Text}({node.Id})");
            if (node.Children == null) return;
            foreach (var n in node.Children)
            {
                Write(n, level + 1);
            }
        }
    }
}