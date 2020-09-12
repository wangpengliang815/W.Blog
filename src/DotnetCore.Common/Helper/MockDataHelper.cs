namespace DotnetCore.Common.Helper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using W.Blog.Entity.Entitys;

    public class MockDataHelper
    {
        /// <summary>
        /// 该方法只针对PrimaryKey是int类型
        /// 如果是string类型的Guid请调整该方法的判断条件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        private static bool IsPrimaryKey<TEntity>(PropertyInfo property)
             where TEntity : BaseEntity, new()
        {
            bool result = false;
            if (property.GetCustomAttribute(typeof(KeyAttribute), true) != null
               || property.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)
               || property.Name.Equals(new TEntity().GetType().Name + "Id", StringComparison.OrdinalIgnoreCase))
            {
                result = true;
            }
            return result;
        }

        private static TEntity BuildEntity<TEntity>() where TEntity : BaseEntity, new()
        {
            TEntity entity = new TEntity();
            PropertyInfo[] properties = entity.GetType().GetProperties();
            foreach (PropertyInfo item in properties)
            {
                // 排除主键
                if (!IsPrimaryKey<TEntity>(item))
                {
                    if (item.PropertyType == typeof(int))
                    {
                        item.SetValue(entity, Faker.RandomNumber.Next(10));
                    }
                    if (item.PropertyType == typeof(string))
                    {
                        item.SetValue(entity, Faker.Name.FullName());
                    }
                    // 可为空类型处理
                    if (item.PropertyType.IsGenericType && item.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        Type columnType = item.PropertyType.GetGenericArguments()[0];
                        if (columnType == typeof(DateTime))
                            item.SetValue(entity, DateTime.Now);
                        if (columnType == typeof(bool))
                            item.SetValue(entity, false);
                    }
                }
            }
            return entity;
        }

        /// <summary>
        /// 构造Mock数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static TEntity MockData<TEntity>()
          where TEntity : BaseEntity, new()
        {
            return BuildEntity<TEntity>();
        }

        /// <summary>
        /// 构造Mock数据集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="count">默认构造100条</param>
        /// <returns></returns>
        public static List<TEntity> MockDataList<TEntity>(int count = 10000)
           where TEntity : BaseEntity, new()
        {
            List<TEntity> list = new List<TEntity>();
            for (int i = 0; i < count; i++)
            {
                TEntity entity = MockData<TEntity>();
                list.Add(entity);
            }
            return list;
        }
    }
}
