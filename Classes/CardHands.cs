namespace Cards;


public class Deck
{
    //The deck of cards
    //List<Card> CardStack { get; set; } = [];
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
}


public class Hand
{
    List<Card> CardStack { get; set; } = []; //Keeps the hand from being edited unexpectedly
    public List<Card> Cards => CardStack; //Reads CardStack but doesn't edit; needs to be done through AddCard() 


    //<---------------------------- Initialization ----------------------------->
    public Hand(Card card) => AddCard(card); //Initialize the deck with one card
    public Hand() => CardStack = []; //Initialize a blank deck
    //<------------------------------------------------------------------------->


    public void AddCard(Card cardToAdd)
    {
        CardStack.Add(cardToAdd);
    }

    public void PrintCards()
    {
        foreach (Card card in Cards)
        {
            Console.Write($"{card.CardValue} of {card.Suite}, ");
        }
    }
}