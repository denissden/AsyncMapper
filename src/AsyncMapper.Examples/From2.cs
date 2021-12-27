namespace AsyncMapper.Examples
{
    public class From2
    {
        public int IntValue2;
        public int IntValue;
        public string StringValue;

        public From2(int i, int i2, string s)
        {
            IntValue = i;
            IntValue2 = i2;
            StringValue = s;
        }

        public override string ToString() => $"{base.ToString()} {IntValue2}";
    }
}