namespace AsyncMapper.Examples
{
    public class To1
    {
        public int IntValue;
        public string StringValue = "";

        public override string ToString()
        {
            return $"{IntValue} {StringValue}";
        }
    }
}