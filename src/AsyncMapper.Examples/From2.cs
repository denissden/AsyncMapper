namespace AsyncMapper.Examples
{
    public class Dummy
    {
        public int id;
    }
    public class From2 : From1
    {
        public Dummy IntValue2;

        public From2(int i, int i2, string s) : base(i, s)
        {
            IntValue2 = new Dummy() { id = i2 };
        }

        public override string ToString() => $"{base.ToString()} {IntValue2}";
    }
}