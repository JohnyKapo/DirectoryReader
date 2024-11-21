namespace ConsoleApplication.model
{
    public class Postfix
    {
        public string PostfixVal { get; set; }
        public int Count { get; set; } = 1;

        public Postfix(string postfixVal)
        {
            this.PostfixVal = postfixVal;
            this.Count = 1;
        }

        public Postfix(string postixVal, int count) 
        {
            this.PostfixVal = postixVal;
            this.Count = count;
        }

        public Postfix() { }
    }
}
