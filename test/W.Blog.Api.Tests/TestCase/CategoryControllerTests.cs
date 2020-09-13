namespace W.Blog.Api.Tests.TestCase
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using W.Blog.Entity.Entitys;

    [TestCategory("integration\\Api")]
    [TestClass()]
    public class CategoryControllerTests : TestBase
    {
        private readonly string GetAsyncUrl = "/api/category";

        private readonly string PostAsyncUrl = "/api/category";

        [TestMethod]
        public async Task GetAsync()
        {
            HttpResponseMessage response = await testServer.CreateClient()
                .GetAsync(GetAsyncUrl);
            response.EnsureSuccessStatusCode();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            string result = await response.Content.ReadAsStringAsync();
            List<CategoryEntity> data =
                JsonConvert.DeserializeObject<List<CategoryEntity>>(result);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task PostAsync()
        {
            CategoryEntity content = new CategoryEntity
            {

            };

            HttpResponseMessage response = await testServer.CreateClient()
                .PostAsync(PostAsyncUrl,
                new StringContent(JsonConvert.SerializeObject(content),
                Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
