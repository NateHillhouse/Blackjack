using System.IO.Compression;

namespace Cards;

public interface ICards
{
    public void AddCard(params Card[] cards);
    int GetValue();
}

public class Deck : ICards
{
    Stack<Card> CardStack { get; set; } = []; //Keeps track of the cards
    public Stack<Card> CardDeck => CardStack; //Shows the cards without allowing for unexpected changes
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

    public int GetValue()
    {
        return CardStack.Peek().CardValue;
    }
}


public class Hand : ICards
{
    List<Card> CardStack { get; set; } = []; //Keeps the hand from being edited unexpectedly
    public List<Card> Cards => CardStack; //Reads CardStack but doesn't edit; needs to be done through AddCard() 
    public int Value => GetValue();
    public bool InGame { get; set; } = true;
    public string BustedMessage = "";

    //<---------------------------- Initialization ----------------------------->
    public Hand(Card card) => AddCard(card); //Initialize the deck with one card
    public Hand() => CardStack = []; //Initialize a blank deck

    //<------------------------------------------------------------------------->


    public void AddCard(params Card[] cardsToAdd)
    {
        foreach (Card card in cardsToAdd) CardStack.Add(card);
    }


    public int GetValue()
    {
        int value = 0;
        int numberAces = 0;

        foreach (Card card in CardStack)
        {
            if (card.CardValue == 1) //If the card is an Ace, add 11 points00
            {
                value += 11;
                numberAces ++;
            }
            else if (card.CardValue > 10) value += 10; //If the card is a face card, add 10 points
            else value += card.CardValue;
        }

        while (value > 21 && numberAces > 0) //If the player is over 21, reduce each available ace back to 1
        {
            value -= 10;
            numberAces --;
        }
        return value;
    }

    public void PrintCards()
    {
        int cardHeight = Console.BufferHeight/6;
        (int x, int y) CursorLocation = Console.GetCursorPosition();
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
            
            string val = card.CardValue switch
            {
                1 => "A",
                11 => "J",
                12 => "Q",
                13 => "K",
                _ => card.CardValue.ToString()
            };

            //Console.Write($"{val}{printSuite}   ");
            CursorLocation = Console.GetCursorPosition();
            PrintFullCard(val, printSuite, cardHeight, ref CursorLocation);
        }
        Console.SetCursorPosition(0, CursorLocation.y + cardHeight + 1);
    }

    public void PrintFullCard(string cardValue, string suite, int cardHeight, ref (int x, int y) location)
    {
        
        int shift = cardHeight; 
        
            //Write top border
            Console.WriteLine();
            Console.SetCursorPosition(location.x, location.y);
            Console.Write("┌");
            
            Console.SetCursorPosition(location.x + shift, location.y);
            Console.WriteLine("┐");

            Console.SetCursorPosition(location.x, location.y + shift);
            Console.Write("└");

            Console.SetCursorPosition(location.x + shift, location.y + shift);
            Console.Write("┘");


            for (int i = shift-1; i > 0; i--) 
            {
                Console.SetCursorPosition(location.x + i, location.y);
                Console.Write("-");
                Console.SetCursorPosition(location.x + i, location.y + shift);
                Console.Write("-");
                
                Console.SetCursorPosition(location.x, location.y + i);
                Console.Write("|");
                Console.SetCursorPosition(location.x + shift, location.y + i);
                Console.Write("|");
            }
            
            int yValueShift = shift/5;
            int xValueShift = shift/4;
            Console.SetCursorPosition(location.x + xValueShift, location.y + yValueShift);
            Console.Write(cardValue);
            Console.SetCursorPosition(location.x + xValueShift, location.y + yValueShift+1);
            Console.Write(suite);

            Console.SetCursorPosition(location.x + cardHeight - xValueShift, location.y + cardHeight - yValueShift-1);
            Console.Write(suite);
            Console.SetCursorPosition(location.x + cardHeight - xValueShift, location.y + cardHeight - yValueShift);
            if (cardValue == "10") Console.SetCursorPosition(location.x + cardHeight - xValueShift-1, location.y + cardHeight - yValueShift);
            Console.Write(cardValue);
            
        Console.SetCursorPosition(location.x + shift + 1, location.y);

    }
}