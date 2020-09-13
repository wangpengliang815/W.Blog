namespace W.Blog.UnitTests.BLLTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using DotnetCore.Common.Helper;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using W.Blog.BLL.Implement;
    using W.Blog.Dal.DbContexts;
    using W.Blog.DAL.Implement;
    using W.Blog.Entity.Entitys;

    [TestCategory("bvt")]
    [TestClass()]
    public class BaseBLLTests : TestBase
    {
        private readonly TestHelper genericTest = new TestHelper();

        public TestContext TestContext { get; set; }

        /// <summary>
        /// 泛型单元测试帮助类
        /// </summary>
        private class TestHelper
        {
            public static BaseBLL<TEntity, TDAL, ApplicationDbContext> GetBLLInstance<TEntity, TDAL>()
                where TEntity : BaseEntity, new()
                where TDAL : IBaseDAL<TEntity, ApplicationDbContext>
            {
                Type genericBaseBllType = GetSubclasses(typeof(BaseBLL))
                                          .FirstOrDefault(p => p.IsGenericType);

                if (genericBaseBllType == null)
                    throw new Exception("genericBaseBllType=null");

                Type realBaseType = genericBaseBllType.MakeGenericType(
                    typeof(TEntity),
                    typeof(TDAL),
                    typeof(ApplicationDbContext));

                Type implementedBllType = GetSubclasses(realBaseType).FirstOrDefault();
                if (implementedBllType == null)
                    throw new Exception("implementedBllType=null");

                BaseBLL<TEntity, TDAL, ApplicationDbContext> result =
                    (BaseBLL<TEntity, TDAL, ApplicationDbContext>)Activator.CreateInstance(implementedBllType, typeof(TDAL));
                return result;
            }

            public static object GetDalInstance<TEntity>()
        where TEntity : BaseEntity, new()
            {
                Type baseDalType = typeof(BaseDAL<TEntity, ApplicationDbContext>);

                Type implementedDalType = GetSubclasses(baseDalType)
                                .FirstOrDefault();

                if (implementedDalType == null)
                    return null;

                object result =
                        Activator.CreateInstance(implementedDalType, TestMemoryDbContext);
                return result;
            }

            public static async Task FindAsync_Normal<TEntity, TDAL>()
                where TEntity : BaseEntity, new()
                where TDAL : IBaseDAL<TEntity, ApplicationDbContext>
            {
                BaseBLL<TEntity, TDAL, ApplicationDbContext> bll =
                    GetBLLInstance<TEntity, TDAL>();
                TEntity mockData = await bll.InsertAsync(MockDataHelper.MockData<TEntity>());
                TEntity model = await bll.FindAsync(mockData.Id);
                Assert.AreEqual(model.Id, mockData.Id);
            }

            public static async Task FindAsync_ArgumentException<TEntity, TDAL>()
                where TEntity : BaseEntity, new()
                where TDAL : IBaseDAL<TEntity, ApplicationDbContext>
            {
                BaseBLL<TEntity, TDAL, ApplicationDbContext> bll =
                   GetBLLInstance<TEntity, TDAL>();
                await Assert.ThrowsExceptionAsync<ArgumentException>(async ()
                   => await bll.FindAsync(0));
            }

            public static async Task InsertAsync_Normal<TEntity, TDAL>()
                where TEntity : BaseEntity, new()
                where TDAL : IBaseDAL<TEntity, ApplicationDbContext>
            {
                BaseBLL<TEntity, TDAL, ApplicationDbContext> bll =
                   GetBLLInstance<TEntity, TDAL>();
                TEntity mockData = await bll.InsertAsync(MockDataHelper.MockData<TEntity>());
                await bll.FindAsync(mockData.Id);
                Assert.IsNotNull(mockData.Id);
            }

            public static async Task InsertAsync_NullException<TEntity, TDAL>()
                where TEntity : BaseEntity, new()
                where TDAL : IBaseDAL<TEntity, ApplicationDbContext>
            {
                BaseBLL<TEntity, TDAL, ApplicationDbContext> bll =
                     GetBLLInstance<TEntity, TDAL>();
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(async ()
                    => await bll.InsertAsync(null));
            }

            public static async Task BatchInsertAsync_Normal<TEntity, TDAL>()
                where TEntity : BaseEntity, new()
                where TDAL : IBaseDAL<TEntity, ApplicationDbContext>
            {
                BaseBLL<TEntity, TDAL, ApplicationDbContext> bll =
                     GetBLLInstance<TEntity, TDAL>();
                List<TEntity> mockData = MockDataHelper.MockDataList<TEntity>();
                List<TEntity> modelList = await bll.BatchInsertAsync(mockData);
                Assert.AreEqual(mockData.Count, modelList.Count);
            }

            public static async Task BatchInsertAsync_NullException<TEntity, TDAL>()
                where TEntity : BaseEntity, new()
                where TDAL : IBaseDAL<TEntity, ApplicationDbContext>
            {
                BaseBLL<TEntity, TDAL, ApplicationDbContext> bll =
                   GetBLLInstance<TEntity, TDAL>();
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(async ()
                 => await bll.BatchInsertAsync(null));
            }

            public static async Task UpdateAsync_Normal<TEntity, TDAL>()
                 where TEntity : BaseEntity, new()
                 where TDAL : IBaseDAL<TEntity, ApplicationDbContext>
            {
                BaseBLL<TEntity, TDAL, ApplicationDbContext> bll =
                    GetBLLInstance<TEntity, TDAL>();

                TEntity model = await bll.InsertAsync(MockDataHelper.MockData<TEntity>());

                DateTime createTime = model.CreateTime;
                model.CreateTime = model.CreateTime.AddDays(2);

                TEntity up_model = await bll.UpdateAsync(model);

                Assert.AreEqual(model.Id, up_model.Id);
                Assert.AreNotEqual(createTime, up_model.CreateTime);
            }

            public static async Task UpdateAsync_NullException<TEntity, TDAL>()
                where TEntity : BaseEntity, new()
                where TDAL : IBaseDAL<TEntity, ApplicationDbContext>
            {
                BaseBLL<TEntity, TDAL, ApplicationDbContext> bll =
                    GetBLLInstance<TEntity, TDAL>();

                await Assert.ThrowsExceptionAsync<ArgumentNullException>(async ()
                => await bll.UpdateAsync(null));
            }

            public static async Task BatchUpdateAsync_Normal<TEntity, TDAL>()
                 where TEntity : BaseEntity, new()
                 where TDAL : IBaseDAL<TEntity, ApplicationDbContext>
            {
                BaseBLL<TEntity, TDAL, ApplicationDbContext> bll =
                    GetBLLInstance<TEntity, TDAL>();

                List<TEntity> mockData = MockDataHelper.MockDataList<TEntity>();
                await bll.BatchInsertAsync(mockData);

                List<DateTime> testValues = new List<DateTime>();
                foreach (TEntity item in mockData)
                {
                    testValues.Add(item.CreateTime);
                    item.CreateTime = item.CreateTime.AddDays(2);
                }

                List<TEntity> up_models = await bll.BatchUpdateAsync(mockData);

                Assert.IsTrue(up_models.Count == mockData.Count);

                for (int i = 0; i < up_models.Count; i++)
                {
                    Assert.IsTrue(up_models[i].CreateTime == testValues[i]);
                }
            }

            public static async Task BatchUpdateAsync_NullException<TEntity, TDAL>()
                 where TEntity : BaseEntity, new()
                 where TDAL : IBaseDAL<TEntity, ApplicationDbContext>
            {
                BaseBLL<TEntity, TDAL, ApplicationDbContext> bll =
                    GetBLLInstance<TEntity, TDAL>();
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(async ()
                 => await bll.BatchUpdateAsync(null));
            }

            public static async Task GetListAsync_Normal<TEntity, TDAL>()
                where TEntity : BaseEntity, new()
                where TDAL : IBaseDAL<TEntity, ApplicationDbContext>
            {
                BaseBLL<TEntity, TDAL, ApplicationDbContext> bll =
                    GetBLLInstance<TEntity, TDAL>();
                List<TEntity> mockData = MockDataHelper.MockDataList<TEntity>();
                await bll.BatchInsertAsync(mockData);
                List<TEntity> modelList = await bll.GetListAsync();
                Assert.IsNotNull(modelList);
                Assert.AreEqual(mockData.Count, modelList.Count);
            }
        }

        public void RunGenericityClassTestMethod<TEntity>(string methodName)
                 where TEntity : BaseEntity, new()
        {
            object dalInstance = TestHelper.GetDalInstance<TEntity>();
            Type dalType = dalInstance.GetType().GetInterfaces().FirstOrDefault(p => !p.IsGenericType);
            MethodInfo methodInfo = genericTest.GetType().GetMethod(methodName);
            methodInfo.MakeGenericMethod(new Type[] { typeof(TEntity), dalType }).Invoke(genericTest, null);
        }

        private void RunTestHelpMethod(string methodName)
        {
            bool isPass = true;
            MethodInfo methodInfo = new BaseBLLTests().GetType().GetMethod("RunGenericityClassTestMethod");
            foreach (Type type in GetSubclasses(typeof(BaseEntity)))
            {
                if (type == null)
                    throw new Exception("type=null");
                try
                {
                    TestContext.WriteLine($"==Begin Testing type = {type.Name}, method = {methodName}");
                    methodInfo.MakeGenericMethod(new Type[] { type }).Invoke(new BaseBLLTests(), new object[] { methodName });
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
                Assert.Fail("请查看日志,找到无效的测试!");
            }
        }

        [TestMethod()]
        public void BaseBLL_01_FindAsync_NormalAsync()
        {
            RunTestHelpMethod("FindAsync_Normal");
        }

        [TestMethod()]
        public void BaseBLL_01_FindAsync_ArgumentException()
        {
            RunTestHelpMethod("FindAsync_ArgumentException");
        }

        [TestMethod()]
        public void BaseBLL_02_InsertAsync_Normal()
        {
            RunTestHelpMethod("InsertAsync_Normal");
        }

        [TestMethod()]
        public void BaseBLL_02_InsertAsync_NullException()
        {
            RunTestHelpMethod("InsertAsync_NullException");
        }

        [TestMethod()]
        public void BaseBLL_03_BatchInsertAsync_Normal()
        {
            RunTestHelpMethod("BatchInsertAsync_Normal");
        }

        [TestMethod()]
        public void BaseBLL_03_BatchInsertAsync_NullException()
        {
            RunTestHelpMethod("BatchInsertAsync_NullException");
        }

        [TestMethod()]
        public void BaseBLL_04_UpdateAsync_Normal()
        {
            RunTestHelpMethod("UpdateAsync_Normal");
        }

        [TestMethod()]
        public void BaseBLL_04_UpdateAsync_NullException()
        {
            RunTestHelpMethod("UpdateAsync_NullException");
        }

        [TestMethod()]
        public void BaseBLL_05_BatchUpdateAsync_Normal()
        {
            RunTestHelpMethod("BatchUpdateAsync_Normal");
        }

        [TestMethod()]
        public void BaseBLL_05_BatchUpdateAsync_NullException()
        {
            RunTestHelpMethod("BatchUpdateAsync_NullException");
        }

        [TestMethod()]
        public void BaseBLL_06_GetListAsync_Normal()
        {
            RunTestHelpMethod("GetListAsync_Normal");
        }
    }
}