namespace my_new_app.Model
{
    public class Balance
    {
        public string Date { get; set; }
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return $"(date: {Date} amount: {Amount})";
        }
    }

}
