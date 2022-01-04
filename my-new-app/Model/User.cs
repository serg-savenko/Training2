using LiteDB;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace my_new_app.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [Range(16, 300)]
        public int Age { get; set; }
        public string RegistrationCity { get; set; }
        public string Currency { get; set; }
        [BsonIgnore]
        public decimal BalanceNet => Balance.Sum(b => b.Amount);
        public List<Balance> Balance { get; set; } = new List<Balance>();

        public override string ToString()
        {
            return $"id: {Id} name: {Name} surname: {Surname} age: {Age} registrationcity: {RegistrationCity} currency: {Currency} balance: {string.Join(',', Balance)}";
        }
    }

}
