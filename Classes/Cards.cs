namespace Classes;

public readonly struct Cards(Suite suite, int number)
{
    private readonly Suite CardSuite { get; } = suite;
    public readonly Suite GetSuite => CardSuite;
    private readonly int Value { get; } = number;
    public readonly int CardValue => Value;

    public readonly Color CardColor
    {
        get => CardSuite switch
        {
            Suite.Clubs => Color.Black,
            Suite.Spades => Color.Black,
            Suite.Hearts => Color.Red,
            Suite.Diamonds => Color.Red,
            _ => Color.Blank
        };
    }
}

public enum Suite {Spades, Diamonds, Clubs, Hearts}
public enum Color {Red, Black, Blank}