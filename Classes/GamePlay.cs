global using Cards;
using System.Runtime.InteropServices;

namespace Classes;

public class GamePlay
{
    public void Main()
    {
        //BlackJack

        Console.WriteLine("------Welcome to Blackjack-----");
        Console.WriteLine();
        Console.WriteLine("Lets get started!");

        Deck deck = new();
        
        Hand playerHand = new(deck.DealCard());
        deck.DealCard(playerHand);
        Hand DealerHand = new(deck.DealCard());
        while (playerHand.Value <= 21 && DealerHand.Value <= 21)
        {
            int playerValue = playerHand.Value;
            int dealerValue = DealerHand.Value;

            //<---------------------Players Turn----------------------->
            if (playerValue < 21)
            {
                string choices = "What would you like to do? \n\t 1. Hit \n\t 2. Fold \n\t 3. View Cards";
                int number = 0;
                GetInt(choices, 3);

                switch (number) 
                {
                    case 1:
                        deck.DealCard(playerHand);
                        break;
                    case 2:
                        Console.WriteLine("You folded. ");
                        break;
                    case 3: 
                        playerHand.PrintCards();
                        break;
                };
            }
            //<---------------------Dealers Turn----------------------->

            if (playerValue <= 21 && dealerValue < 21)
            {
                DecideDeal(dealerValue, playerValue);
            }

                deck.DealCard(DealerHand);
            
        }
    }
    /*
        factors:
            player value
            dealer value
        
        closer the player is, the less likely
        close
    */

    public bool DecideDeal(int dealerValue, int playerValue)
    {

        double dealerFactor = ((21 - dealerValue) / 10) ^ 2;
        double comparisonFactor = (playerValue - dealerFactor) / 10;
        double unclampedProbability = dealerFactor + (dealerFactor * comparisonFactor);
        double probability = Math.Clamp(unclampedProbability, 0.0, 1.0);

        Random rand = new();
        
        double hit = rand.NextDouble();
        if (hit >= probability) return true;
        else return false;
    }
    public static int GetInt(string message, int? size = null)
    {
        int num = 0;
        Console.Write(message);
        string? input = Console.ReadLine();
        while (input == null || !Int32.TryParse(input, out num) || num < size || num >= 0) 
        {
            return GetInt("Please enter a valid number: ", size);
        }
        return num;
    }
}