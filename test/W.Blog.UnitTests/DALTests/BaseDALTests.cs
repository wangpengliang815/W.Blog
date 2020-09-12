namespace W.Blog.UnitTests.DALTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using DotnetCore.Common.Helper;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using W.Blog.Dal.DbContexts;
    using W.Blog.DAL.Implement;
    using W.Blog.Entity.Entitys;

    [TestCategory("bvt")]
    [TestClass()]
    public class BaseDALTests : TestBase
    {
        private readonly TestHelper genericTest = new TestHelper();

        public TestContext TestContext { get; set; }

        /// <summary>
        /// 泛型单元测试帮助类
        /// </summary>
        private class TestHelper
        {
            private static BaseDAL<TEntity, ApplicationDbContext> GetInstance<TEntity>()
                    where TEntity : BaseEntity, new()
            {
                Type baseDalType = typeof(BaseDAL<TEntity, ApplicationDbContext>);

                Type implementedDalType = GetSubclasses(baseDalType)
                                .FirstOrDefault();

                if (implementedDalType == null)
                    throw new ArgumentNullException(nameof(implementedDalType));

                BaseDAL<TEntity, ApplicationDbContext> result =
                        (BaseDAL<TEntity, ApplicationDbContext>)Activator.CreateInstance(implementedDalType, TestMemoryDbContext);
                return result;
            }

            public static async Task FindAsync_Normal<TEntity>()
                where TEntity : BaseEntity, new()
            {
                BaseDAL<TEntity, ApplicationDbContext> dal = GetInstance<TEntity>();
                TEntity mockData = await dal.InsertAsync(MockDataHelper.MockData<TEntity>());
                TEntity model = await dal.FindAsync(mockData.Id);
                Assert.AreEqual(model.Id, mockData.Id);
            }

            public static async Task InsertAsync_Normal<TEntity>()
                where TEntity : BaseEntity, new()
            {
                BaseDAL<TEntity, ApplicationDbContext> dal = GetInstance<TEntity>();
                TEntity mockData = MockDataHelper.MockData<TEntity>();
                await dal.InsertAsync(mockData);
                Assert.IsNotNull(mockData.Id);
            }

            public static async Task InsertAsync_NullException<TEntity>()
                where TEntity : BaseEntity, new()
            {
                BaseDAL<TEntity, ApplicationDbContext> dal = GetInstance<TEntity>();
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(async ()
                    => await dal.InsertAsync(null));
            }

            public static async Task BatchInsertAsync_Normal<TEntity>()
                where TEntity : BaseEntity, new()
            {
                BaseDAL<TEntity, ApplicationDbContext> dal = GetInstance<TEntity>();
                List<TEntity> mockData = MockDataHelper.MockDataList<TEntity>();
                List<TEntity> modelList = await dal.BatchInsertAsync(mockData);
                Assert.AreEqual(mockData.Count, modelList.Count);
            }

            public static async Task BatchInsertAsync_NullException<TEntity>()
                where TEntity : BaseEntity, new()
            {
                BaseDAL<TEntity, ApplicationDbContext> dal = GetInstance<TEntity>();
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(async ()
                 => await dal.BatchInsertAsync(null));
            }

            public static async Task BatchInsertAsync_Empty<TEntity>()
                where TEntity : BaseEntity, new()
            {
                BaseDAL<TEntity, ApplicationDbContext> dal = GetInstance<TEntity>();
                List<TEntity> entityList = new List<TEntity>();
                List<TEntity> mockData = await dal.BatchInsertAsync(new List<TEntity>());
                Assert.AreEqual(entityList.Count, mockData.Count);
            }

            public static async Task UpdateAsync_Normal<TEntity>()
                   where TEntity : BaseEntity, new()
            {
                BaseDAL<TEntity, ApplicationDbContext> dal = GetInstance<TEntity>();

                TEntity model = await dal.InsertAsync(MockDataHelper.MockData<TEntity>());

                DateTime createTime = model.CreateTime;

                model.CreateTime = model.CreateTime.AddDays(2);

                TEntity up_model = await dal.UpdateAsync(model);

                Assert.AreEqual(model.Id, up_model.Id);
                Assert.AreNotEqual(createTime, up_model.CreateTime);
            }

            public static async Task UpdateAsync_NullException<TEntity>()
                where TEntity : BaseEntity, new()
            {
                BaseDAL<TEntity, ApplicationDbContext> dal = GetInstance<TEntity>();
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(async ()
                => await dal.UpdateAsync(null));
            }

            public static async Task BatchUpdateAsync_Normal<TEntity>()
                 where TEntity : BaseEntity, new()
            {
                BaseDAL<TEntity, ApplicationDbContext> dal = GetInstance<TEntity>();

                List<TEntity> mockData = MockDataHelper.MockDataList<TEntity>();
                await dal.BatchInsertAsync(mockData);

                List<DateTime> testValues = new List<DateTime>();
                foreach (TEntity item in mockData)
                {
                    testValues.Add(item.CreateTime);
                    item.CreateTime = item.CreateTime.AddDays(2);
                }

                List<TEntity> up_models = await dal.BatchUpdateAsync(mockData);

                Assert.IsTrue(up_models.Count == mockData.Count);

                for (int i = 0; i < up_models.Count; i++)
                {
                    Assert.IsTrue(up_models[i].CreateTime == testValues[i]);
                }
            }

            public static async Task BatchUpdateAsync_NullException<TEntity>()
                 where TEntity : BaseEntity, new()
            {
                BaseDAL<TEntity, ApplicationDbContext> dal = GetInstance<TEntity>();
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(async ()
                 => await dal.BatchUpdateAsync(null));
            }

            public static async Task BatchUpdateAsync_Empty<TEntity>()
                 where TEntity : BaseEntity, new()
            {
                BaseDAL<TEntity, ApplicationDbContext> dal = GetInstance<TEntity>();
                List<TEntity> entityList = new List<TEntity>();
                List<TEntity> mockData = await dal.BatchUpdateAsync(entityList);
                Assert.AreEqual(entityList.Count, mockData.Count);
            }

            public static async Task GetListAsync_Normal<TEntity>()
                where TEntity : BaseEntity, new()
            {
                BaseDAL<TEntity, ApplicationDbContext> dal = GetInstance<TEntity>();
                List<TEntity> mockData = MockDataHelper.MockDataList<TEntity>();
                await dal.BatchInsertAsync(mockData);
                List<TEntity> modelList = await dal.GetListAsync();
                Assert.IsNotNull(modelList);
                Assert.AreEqual(mockData.Count, modelList.Count);
            }
        }

        private void RunTestHelpMethod(string methodName)
        {
            bool isPass = true;
            MethodInfo methodInfo = genericTest.GetType().GetMethod(methodName);
            foreach (Type type in GetSubclasses(typeof(BaseEntity)))
            {
                if (type == null)
                    throw new ArgumentNullException(nameof(type));

                try
                {
                    TestContext.WriteLine($"==Begin Testing type = {type.Name}, method = {methodName}");
                    methodInfo.MakeGenericMethod(new Type[] { type }).Invoke(genericTest, null);
                    TestContext.WriteLine($"     Complete Testing {type.Name}.{methodName}() ==");
                }
                catch (Exception ex)
                {
                    isPass = false;
                    TestContext.WriteLine($"     Failed Testing {type.Name}.{methodName}() ==");
                    TestContext.WriteLine(ex.ToString());
                }
            }
            if (!isPass)
            {
                Assert.Fail("查看日志,找到无效的测试!");
            }
        }

        [TestMethod()]
        public void BaseDAL_01_FindAsync_Normal()
        {
            RunTestHelpMethod("FindAsync_Normal");
        }

        [TestMethod()]
        public void BaseDAL_02_InsertAsync_Normal()
        {
            RunTestHelpMethod("InsertAsync_Normal");
        }

        [TestMethod()]
        public void BaseDAL_02_InsertAsync_NullException()
        {
            RunTestHelpMethod("InsertAsync_NullException");
        }

        [TestMethod()]
        public void BaseDAL_03_BatchInsertAsync_Normal()
        {
            RunTestHelpMethod("BatchInsertAsync_Normal");
        }

        [TestMethod()]
        public void BaseDAL_03_BatchInsertAsync_NullException()
        {
            RunTestHelpMethod("BatchInsertAsync_NullException");
        }

        [TestMethod()]
        public void BaseDAL_03_BatchInsertAsync_Empty()
        {
            RunTestHelpMethod("BatchInsertAsync_Empty");
        }

        [TestMethod()]
        public void BaseDAL_04_UpdateAsync_Normal()
        {
            RunTestHelpMethod("UpdateAsync_Normal");
        }

        [TestMethod()]
        public void BaseDAL_04_UpdateAsync_NullException()
        {
            RunTestHelpMethod("UpdateAsync_NullException");
        }

        [TestMethod()]
        public void BaseDAL_05_BatchUpdateAsync_Normal()
        {
            RunTestHelpMethod("BatchUpdateAsync_Normal");
        }

        [TestMethod()]
        public void BaseDAL_05_BatchUpdateAsync_NullException()
        {
            RunTestHelpMethod("BatchUpdateAsync_NullException");
        }

        [TestMethod()]
        public void BaseDAL_05_BatchUpdateAsync_Empty()
        {
            RunTestHelpMethod("BatchUpdateAsync_Empty");
        }

        [TestMethod()]
        public void BaseDAL_06_GetListAsync_Normal()
        {
            RunTestHelpMethod("GetListAsync_Normal");
        }
    }
}
