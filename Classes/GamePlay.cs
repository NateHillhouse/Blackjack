global using Cards;
using System.Runtime.InteropServices;

namespace Classes;

public class GamePlay
{
    public static void Main()
    {
        //BlackJack
        Console.Clear();
        Console.WriteLine("------Welcome to Blackjack-----");
        Console.WriteLine();
        Console.WriteLine("Lets get started!\n\n");

        Deck deck = new();
        
        Hand playerHand = new(deck.DealCard());
        deck.DealCard(ref playerHand);
        Hand DealerHand = new(deck.DealCard());
        string playerOutMessage = "";

        while (playerHand.InGame || DealerHand.InGame)
        {
            Console.Clear();
            PrintCards(playerHand, DealerHand, playerOutMessage);
            int playerValue = playerHand.Value;
            int dealerValue = DealerHand.Value;

            //<---------------------Players Turn----------------------->
            if (playerValue < 21 && playerHand.InGame)
            {
                string choices = "What would you like to do? \n\t 1. Hit \n\t 2. Fold \n\t 3. View Cards\n";
                int number = 0;
                number = GetInt(choices, 3);

                switch (number) 
                {
                    case 1:
                        deck.DealCard(ref playerHand);
                        break;
                    case 2:
                        playerHand.InGame = false;
                        break;
                    case 3: 
                        playerHand.PrintCards();
                        break;
                };
            }
            else if (playerHand.Value > 21) 
            {
                playerHand.InGame = false;
                playerOutMessage = "You have busted. ";
            }
            else if (playerHand.Value == 21)
            {
                playerHand.InGame = false;
                playerOutMessage = "You have blackjack! ";
            }
            //<---------------------Dealers Turn----------------------->
            if (playerValue <= 21 && dealerValue < 21 && DealerHand.InGame) //If the player hasn't busted and the dealer doesn't have blackjack or has busted
            {
                if (DecideDeal(dealerValue, playerValue)) deck.DealCard(ref DealerHand);
                else if (playerHand.InGame)
                {
                    DealerHand.InGame = false;
                    Console.WriteLine("The dealer stands");
                }
                else DealerHand.InGame = false;
            }
            else if (DealerHand.InGame && dealerValue > 21) 
            {
                DealerHand.InGame = false;
                Console.WriteLine("The dealer has busted. ");
            } 
            else if (DealerHand.InGame && dealerValue == 21) 
            {
                DealerHand.InGame = false;
                Console.WriteLine("The dealer has blackjack!");
            } 
            
        }
        Console.WriteLine();

        if (playerHand.Value == DealerHand.Value || (playerHand.Value > 21 && DealerHand.Value > 21)) Console.WriteLine("Its a push!");
        else if (playerHand.Value == 21) Console.WriteLine("You got blackjack!");
        else if (DealerHand.Value == 21) Console.WriteLine("The dealer got blackjack!");
        else if (playerHand.Value <= 21 && playerHand.Value > DealerHand.Value) Console.WriteLine("You won!");
        else if (playerHand.Value > 21) Console.WriteLine("You busted, the dealer won. ");
        else if (DealerHand.Value <= 21) Console.WriteLine("The dealer won.");
        Console.WriteLine($"\n\tYour score: {playerHand.Value} \n\tDealer score: {DealerHand.Value}");
    }

    public static void PrintCards(Hand player, Hand dealer, string message = "")
    {
        if (player.InGame == false) Console.WriteLine(message);
        Console.Write($"Your Cards:\n\t");
        player.PrintCards();
        Console.WriteLine($"\tScore: {player.Value}");

        Console.Write($"\nDealers Cards:\n\t");
        dealer.PrintCards();
        Console.WriteLine($"\tDealer Score: {dealer.Value}");
        Console.WriteLine();
    }

    public static bool DecideDeal(int dealerValue, int playerValue)
    {
        if (dealerValue <= 17) return true;

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
        
        while (input == null || !Int32.TryParse(input, out num) || num > size || num < 0) 
        {
            if (num > size || num < 0) return GetInt($"Please chose a number between 0 and {size}: ");
            return GetInt("Please enter a valid number: ", size);
        }
        return num;
    }
}