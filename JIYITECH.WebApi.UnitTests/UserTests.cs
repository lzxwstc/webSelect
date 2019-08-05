using JIYITECH.WebApi.Entities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JIYITECH.WebApi.UnitTests
{
    public class UserTests
    {
        private readonly HttpClient client;
        public UserTests()
        {
            var server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>());
            client = server.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token.GenerateTokenWithRole("admin")}");
        }

        [Fact]
        public async Task Login_ShouldBe_Ok()
        {
            // Arrange
            User user = new User()
            {
                userName = "downer",
                password = "123"
            };

            // Act
            var response = await client.GetAsync($"/api/user/Login?userName={user.userName}&password={user.password}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetUserById_ShouldBe_Ok()
        {
            // Arrange
            var id = 1;

            // Act
            var response = await client.GetAsync($"/api/user/{id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
