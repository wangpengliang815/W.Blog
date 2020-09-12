namespace W.Blog.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.EntityFrameworkCore;
    using W.Blog.Dal.DbContexts;

    public class TestBase
    {
        /// <summary>
        /// 内存数据库Context
        /// </summary>
        protected static ApplicationDbContext TestMemoryDbContext =>
        new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

        /// <summary>
        /// 获取指定类型所有子类
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="assembly">指定的程序集；如果未指定， 则使用baseType所在的程序集</param>
        /// <returns></returns>
        public static List<Type> GetSubclasses(Type baseType, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetAssembly(baseType);
            }

            List<Type> result = assembly.GetTypes()
                                 .Where(p => p.IsSubclassOf(baseType))
                                 .ToList();
            return result;
        }
    }
}
