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
        /// �ڴ����ݿ�Context
        /// </summary>
        protected static ApplicationDbContext TestMemoryDbContext =>
        new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

        /// <summary>
        /// ��ȡָ��������������
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="assembly">ָ���ĳ��򼯣����δָ���� ��ʹ��baseType���ڵĳ���</param>
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
