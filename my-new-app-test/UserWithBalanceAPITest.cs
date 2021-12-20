using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using my_new_app.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace my_new_app_test
{
    public class UserWithBalanceAPITest
    {
        private readonly ITestOutputHelper output;
        private readonly HttpClient httpClient;

        private class Balance
        {
            public string Date { get; set; }
            public decimal Amount { get; set; }
        }

        private class UserWithBalance
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Age { get; set; }
            public string RegistrationCity { get; set; }
            public string currency { get; set; }
            public decimal BalanceNet { get; set; }
            public List<Balance> Balance { get; set; }
        }

        public UserWithBalanceAPITest(ITestOutputHelper output)
        {
            this.output = output;
            var server = new TestServer(new WebHostBuilder().ConfigureAppConfiguration(config => {
                var newConfig = new ConfigurationBuilder().AddJsonFile("integrationtestsettings.json").Build();
                config.AddConfiguration(newConfig);
            }).UseStartup<my_new_app.Startup>());
            httpClient = server.CreateClient();
        }


        [Fact]
        public async void GetListOfUsersWithBalance()
        {
            var response = await httpClient.GetAsync("/userwithbalance");
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var users = JsonConvert.DeserializeObject<List<UserWithBalance>>(content);
            Assert.True(users.Count >= 1);
            var u = users[0];
            Assert.Equal(10, u.Id);
            Assert.Equal("name10", u.Name);
            Assert.Equal("surname10", u.Surname);
            Assert.Equal(22, u.Age);
            Assert.Equal("lviv", u.RegistrationCity);
            Assert.Equal("uah", u.currency);
            Assert.Equal(8.0m, u.BalanceNet);
            Assert.Equal(5, u.Balance.Count);
            Assert.Equal("2021-01-01", u.Balance[0].Date);
            Assert.Equal(1, u.Balance[0].Amount);
        }

        [Fact]
        public async void GetUserWithBalanceById()
        {
            var response = await httpClient.GetAsync("/userwithbalance/10");
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var u = JsonConvert.DeserializeObject<UserWithBalance>(content);
            Assert.Equal(10, u.Id);
            Assert.Equal("name10", u.Name);
            Assert.Equal("surname10", u.Surname);
            Assert.Equal(22, u.Age);
            Assert.Equal("lviv", u.RegistrationCity);
            Assert.Equal("uah", u.currency);
            Assert.Equal(8.0m, u.BalanceNet);
            Assert.Equal(5, u.Balance.Count);
            Assert.Equal("2021-01-01", u.Balance[0].Date);
            Assert.Equal(1, u.Balance[0].Amount);
        }

        [Fact]
        public async void AddUserWithBalance()
        {
            var response = await httpClient.PostAsync("/userwithbalance", new StringContent("{ \"name\": \"new\", \"surname\": \"new\", \"age\": 10, \"registrationCity\": \"kyiv\", \"currency\": \"uah\", \"balance\": [] }", Encoding.UTF8, "text/json"));
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var u = JsonConvert.DeserializeObject<UserWithBalance>(content);
            Assert.True(u.Id > 0);
            Assert.Equal("new", u.Name);
            Assert.Equal("new", u.Surname);
            Assert.Equal(10, u.Age);
            Assert.Equal("kyiv", u.RegistrationCity);
            Assert.Equal("uah", u.currency);
            Assert.Equal(0, u.BalanceNet);
            Assert.Empty(u.Balance);
        }

        [Fact]
        public async void EditUserWithBalanceById()
        {
            var response = await httpClient.PostAsync("/userwithbalance", new StringContent("{ \"name\": \"new\", \"surname\": \"new\", \"age\": 10, \"registrationCity\": \"kyiv\", \"currency\": \"uah\", \"balance\": [] }", Encoding.UTF8, "text/json"));
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var u = JsonConvert.DeserializeObject<UserWithBalance>(content);

            u.Name = "ModifiedName";

            response = await httpClient.PutAsync("/userwithbalance", new StringContent(JsonConvert.SerializeObject(u), Encoding.UTF8, "text/json"));
            content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            u = JsonConvert.DeserializeObject<UserWithBalance>(content);

            Assert.True(u.Id > 0);
            Assert.Equal("ModifiedName", u.Name);
            Assert.Equal("new", u.Surname);
            Assert.Equal(10, u.Age);
            Assert.Equal("kyiv", u.RegistrationCity);
            Assert.Equal("uah", u.currency);
            Assert.Equal(0, u.BalanceNet);
            Assert.Empty(u.Balance);
        }

        [Fact]
        public async void DeleteAddedUserWithBalance()
        {
            var response = await httpClient.PostAsync("/userwithbalance", new StringContent("{ \"name\": \"new\", \"surname\": \"new\", \"age\": 10, \"registrationCity\": \"kyiv\", \"currency\": \"uah\", \"balance\": [] }", Encoding.UTF8, "text/json"));
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var u = JsonConvert.DeserializeObject<UserWithBalance>(content);

            response = await httpClient.DeleteAsync($"/userwithbalance/{u.Id}");
            content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            response = await httpClient.GetAsync($"/userwithbalance/{u.Id}");
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async void AddBalanceToNewUser()
        {
            var response = await httpClient.PostAsync("/userwithbalance", new StringContent("{ \"name\": \"new\", \"surname\": \"new\", \"age\": 10, \"registrationCity\": \"kyiv\", \"currency\": \"uah\", \"balance\": [] }", Encoding.UTF8, "text/json"));
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var u = JsonConvert.DeserializeObject<UserWithBalance>(content);

            response = await httpClient.PostAsync($"/userwithbalance/{u.Id}/balance", new StringContent("{ \"date\": \"2022-01-01\", \"amount\": 2.00 }", Encoding.UTF8, "text/json"));
            content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            u = JsonConvert.DeserializeObject<UserWithBalance>(content);

            Assert.Equal(2, u.BalanceNet);
            Assert.Single(u.Balance);
        }
    }
}
