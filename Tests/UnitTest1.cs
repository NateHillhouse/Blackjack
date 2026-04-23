using Classes;
using Cards;

namespace Tests;

public class Tests
{
    Deck CardDeck { get; set; } = new();

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void DeckAndHandInitialization()
    {
        //Check the proper deck count
        Assert.That(CardDeck.CardDeck.Count > 0);
        Assert.That(CardDeck.CardDeck.Count == 52);

        //Check if shuffling works
        Stack<Card> BaseDeck = CardDeck.CardDeck;
        CardDeck.ShuffleDeck();
        Assert.That(CardDeck.CardDeck != BaseDeck);

        //Check if the hands add cards properly 
        Hand hand = new();
        Assert.That(hand.Cards.Count == 0);
        Assert.That(new Hand(new Card(Suite.Spades, 1)).Cards.Count == 1); //Initializes a hand with one card and checks if the count is correct

        CardDeck.DealCard(ref hand);
        int count = hand.Cards.Count;
        CardDeck.DealCard(ref hand);
        Assert.That(hand.Cards.Count != count);
    }
}
