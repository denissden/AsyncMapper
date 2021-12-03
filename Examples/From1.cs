namespace MapperExperiments;

public class From1 
{
    public int IntValue;
    public string StringValue;

    public From1(int i, string s)
    {
        IntValue = i;
        StringValue = s;
    }

    public override string ToString()
    {
        return $"{IntValue} {StringValue}";
    }
}