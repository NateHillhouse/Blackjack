namespace Cards;


public class Deck
{
    Stack<Card> CardStack { get; set; } = [];
    public Stack<Card> CardDeck => CardStack;
    Random rand = new();
    
    public Deck() //Add each value to the deck upon initialization and shuffle
    {
        for (int number = 1; number <= 13; number ++) //Add each value of the deck
        {
            foreach (Suite suite in Enum.GetValues<Suite>()) //Add each suite to the deck, matched to each number 
            {
                CardStack.Push(new Card(suite, number));
            }
        }
        ShuffleDeck();
    }


    //Function for adding a Card to the deck
    public void AddCard(params Card[] cardsToAdd)
    {
        foreach (Card card in cardsToAdd) CardStack.Push(card);
    }

    public void ShuffleDeck()
    {

        List<Card> CardList = CardStack.ToList();
        CardStack = [];

        while (CardList.Count > 0) 
        {
            int cardNumber = rand.Next(CardList.Count);
            CardStack.Push(CardList[cardNumber]);
            CardList.Remove(CardList[cardNumber]);
        }
    }

    public Card DealCard()
    {
        return CardStack.Pop();
    }
    public void DealCard(Hand hand)
    {
        hand.AddCard(CardStack.Pop());
    }
}


public class Hand
{
    List<Card> CardStack { get; set; } = []; //Keeps the hand from being edited unexpectedly
    public List<Card> Cards => CardStack; //Reads CardStack but doesn't edit; needs to be done through AddCard() 
    public int Value => CheckForAce();

    //<---------------------------- Initialization ----------------------------->
    public Hand(Card card) => AddCard(card); //Initialize the deck with one card
    public Hand() => CardStack = []; //Initialize a blank deck

    //<------------------------------------------------------------------------->


    public void AddCard(Card cardToAdd)
    {
        CardStack.Add(cardToAdd);
    }

    int CheckForAce()
    {
        int value = CardStack.Sum(c => c.CardValue);
        if (value <= 21) return value;
        
        int numberAces = 0;
        foreach (Card card in CardStack)
            if (card.CardValue == 1) numberAces ++;
        return value - (11 * numberAces);
    }

    public void PrintCards()
    {
        Console.Write("Your hand: ");
        foreach (Card card in Cards)
        {
            string printSuite = "";
            printSuite = card.Suite switch
            {
                Suite.Spades => "♠",
                Suite.Clubs => "♣",
                Suite.Diamonds => "♢",
                Suite.Hearts => "♡",
                _ => ""
            };
            Console.Write($"{card.CardValue}{printSuite}   ");
        }
        Console.WriteLine();
    }
}