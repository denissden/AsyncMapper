namespace AsyncMapper.UnitTests
{
    public class From2 : From1
    {
        public int IntValue2;

        public From2(int i, int i2, string s) : base(i, s) => IntValue2 = i2;

        public override string ToString() => $"{base.ToString()} {IntValue2}";
    }
}