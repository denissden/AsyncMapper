namespace MapperExperiments;

public class To2 : To1
{
    public int IntValue2;

    public override string ToString() => $"{base.ToString()} {IntValue2}";
}