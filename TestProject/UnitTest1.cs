using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace TestProject
{
    public class UnitTest1
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public UnitTest1()
        {
            // Arrange
            _server =  new TestServer(new WebHostBuilder()
           .UseStartup<Program>());//
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task Test1()
        {
            // Act
            var response = await _client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.Equal("Hello World!", responseString);
        }
    }
}