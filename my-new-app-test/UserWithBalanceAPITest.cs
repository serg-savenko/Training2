using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using my_new_app.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

            public Balance(string date, decimal amount)
            {
                this.Date = date;
                this.Amount = amount;
            }

            public override bool Equals(object obj)
            {
                return obj is Balance balance &&
                       Date == balance.Date &&
                       Amount == balance.Amount;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Date, Amount);
            }
        }

        private class UserWithBalance
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Age { get; set; }
            public string RegistrationCity { get; set; }
            public string Currency { get; set; }
            public decimal BalanceNet { get; set; }
            public List<Balance> Balance { get; set; } = new List<Balance>();

            public static UserWithBalance CreatePredefinedUser()
            {
                return new UserWithBalance
                {
                    Id = 10,
                    Name = "name10",
                    Surname = "surname10",
                    Age = 22,
                    RegistrationCity = "lviv",
                    Currency = "uah",
                    BalanceNet = 8m,
                    Balance = new List<Balance>() { new Balance("2021-01-01", 1), new Balance("2021-01-02", 1),
                        new Balance("2021-01-03", -5), new Balance("2021-01-01", 10), new Balance("2021-01-01", 1)
                    }
                };
            }
            public static UserWithBalance CreateUserWithoutId()
            {
                return new UserWithBalance
                {
                    Name = "new",
                    Surname = "new surname",
                    Age = 20,
                    RegistrationCity = "kyiv",
                    Currency = "uah",
                    BalanceNet = 0,
                };
            }

            public bool EqualsWithoutId(object obj)
            {
                return obj is UserWithBalance balance &&
                       Name == balance.Name &&
                       Surname == balance.Surname &&
                       Age == balance.Age &&
                       RegistrationCity == balance.RegistrationCity &&
                       Currency == balance.Currency &&
                       BalanceNet == balance.BalanceNet &&
                       Balance.SequenceEqual(balance.Balance);
            }
            public bool EqualsWithId(object obj)
            {
                return obj is UserWithBalance balance &&
                       Id == balance.Id &&
                       EqualsWithoutId(balance);
            }
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
        public async void TestGetListOfUsers()
        {
            var predefinedUser = UserWithBalance.CreatePredefinedUser();
            var response = await httpClient.GetAsync("/userwithbalance");
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var users = JsonConvert.DeserializeObject<List<UserWithBalance>>(content);
            Assert.True(users.Count >= 1);
            Assert.True(predefinedUser.EqualsWithId(users[0]));
        }

        [Fact]
        public async void TestGetUserWithBalanceById()
        {
            var predefinedUser = UserWithBalance.CreatePredefinedUser();
            var response = await httpClient.GetAsync("/userwithbalance/10");
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var user = JsonConvert.DeserializeObject<UserWithBalance>(content);
            Assert.True(predefinedUser.EqualsWithId(user));
        }

        [Fact]
        public async void TestGetNonExistingUserWithBalanceById()
        {
            var predefinedUser = UserWithBalance.CreatePredefinedUser();
            var response = await httpClient.GetAsync("/userwithbalance/0");
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async void AddUserWithBalance()
        {
            var newUser = UserWithBalance.CreateUserWithoutId();
            var response = await httpClient.PostAsync(
                "/userwithbalance", new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "text/json"));
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var savedUser = JsonConvert.DeserializeObject<UserWithBalance>(content);
            Assert.True(savedUser.Id > 0);
            Assert.True(newUser.EqualsWithoutId(savedUser));
        }

        [Fact]
        public async void EditUserWithBalanceById()
        {
            var newUser = UserWithBalance.CreateUserWithoutId();
            var response = await httpClient.PostAsync(
                "/userwithbalance", new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "text/json"));
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var savedUser = JsonConvert.DeserializeObject<UserWithBalance>(content);

            savedUser.Name = "ModifiedName";

            response = await httpClient.PutAsync(
                "/userwithbalance", new StringContent(JsonConvert.SerializeObject(savedUser), Encoding.UTF8, "text/json"));
            content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var modifiedUser = JsonConvert.DeserializeObject<UserWithBalance>(content);

            Assert.True(savedUser.EqualsWithId(modifiedUser));
        }

        [Fact]
        public async void DeleteAddedUserWithBalance()
        {
            var newUser = UserWithBalance.CreateUserWithoutId();
            var response = await httpClient.PostAsync(
                "/userwithbalance", new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "text/json"));
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var savedUsed = JsonConvert.DeserializeObject<UserWithBalance>(content);

            response = await httpClient.DeleteAsync($"/userwithbalance/{savedUsed.Id}");
            content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            response = await httpClient.GetAsync($"/userwithbalance/{savedUsed.Id}");
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async void AddBalanceToNewUser()
        {
            var newUser = UserWithBalance.CreateUserWithoutId();
            var response = await httpClient.PostAsync(
                "/userwithbalance", new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "text/json"));
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var savedUser = JsonConvert.DeserializeObject<UserWithBalance>(content);

            var newBalance = new Balance("2022-01-01", 2.00m);
            response = await httpClient.PostAsync(
                $"/userwithbalance/{savedUser.Id}/balance",
                new StringContent(JsonConvert.SerializeObject(newBalance), Encoding.UTF8, "text/json"));
            content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            savedUser = JsonConvert.DeserializeObject<UserWithBalance>(content);

            Assert.Equal(2, savedUser.BalanceNet);
            Assert.Equal(savedUser.Balance[0], newBalance);
        }

        [Fact]
        public async void AddUserWithAgeLessThen16IsForbidden()
        {
            var newUser = UserWithBalance.CreateUserWithoutId();
            newUser.Age = 12;
            var response = await httpClient.PostAsync(
                "/userwithbalance", new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "text/json"));
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("The field Age must be between 16 and 300", content);
        }
    }
}
