global using Cards;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

namespace Classes;

public class GamePlay
{
    public static void Main()
    {
        //BlackJack
        Console.Clear();

        Deck deck = new();
        
        Hand playerHand = new(deck.DealCard());
        deck.DealCard(playerHand);
        Hand dealerHand = new(deck.DealCard());
        
        if (playerHand.Value == 21) //Instant win
        {
            playerHand.InGame = false;
            dealerHand.InGame = false;
        
            Console.WriteLine("------Welcome to Blackjack-----\n");
            PrintCards(playerHand, "Player");
            PrintCards(dealerHand, "Dealer");
        }
        
        while (playerHand.InGame || dealerHand.InGame)
        {
            Console.Clear();
        
            Console.WriteLine("------Welcome to Blackjack-----\n");
            PrintCards(playerHand, "Player");
            if (playerHand.InGame) Wait(400);
            PrintCards(dealerHand, "Dealer");
            int playerValue = playerHand.Value;
            int dealerValue = dealerHand.Value;

            //<---------------------Players Turn----------------------->
            if (playerValue < 21 && playerHand.InGame && dealerHand.InGame == true)
            {
                string choices = "What would you like to do? \n\t 1. Hit \n\t 2. Fold\n";
                int number = GetInt(choices, 2);

                switch (number) 
                {
                    case 1:
                        deck.DealCard(playerHand);
                        break;
                    case 2:
                        playerHand.InGame = false;
                        break;
                };
            }
            else if (playerHand.Value > 21) 
            {
                playerHand.InGame = false;
                playerHand.BustedMessage = "You have busted. ";
            }
            else if (playerHand.Value == 21)
            {
                playerHand.InGame = false;
                playerHand.BustedMessage = "You have blackjack! ";
            }
            else playerHand.InGame = false;
            //<---------------------Dealers Turn----------------------->
            if (playerValue <= 21 && dealerValue < 21 && dealerHand.InGame) //If the player hasn't busted and the dealer doesn't have blackjack or has busted
            {
                if (DecideDealerHit(dealerValue, playerValue, !playerHand.InGame)) 
                    deck.DealCard(dealerHand);
                else if (playerHand.InGame)
                {
                    dealerHand.InGame = false;
                    dealerHand.BustedMessage = "The dealer stands";
                }
                else dealerHand.InGame = false;
            }
            else if (dealerHand.InGame && dealerValue > 21) 
            {
                dealerHand.InGame = false;
                dealerHand.BustedMessage = "The dealer has busted. ";
            } 
            else if (dealerHand.InGame && dealerValue == 21) 
            {
                dealerHand.InGame = false;
                dealerHand.BustedMessage = "The dealer has blackjack!";
            } 
            else if (dealerValue > 17) dealerHand.InGame = false;

            else deck.DealCard(dealerHand);

            if (!playerHand.InGame && dealerHand.InGame)
            {
                //Console.Write("Press any key to continue"); // - The dealer may still bust! (The dealer must get above 17) 
                //Console.ReadKey(true);
                Wait(600);
            }
            
        }
        Console.WriteLine();


        if (playerHand.Value == dealerHand.Value || (playerHand.Value > 21 && dealerHand.Value > 21)) Console.WriteLine("Its a push!");
        else if (playerHand.Value == 21) Console.WriteLine("You got blackjack!");
        else if (dealerHand.Value == 21) Console.WriteLine("The dealer got blackjack!");
        else if (playerHand.Value <= 21 && playerHand.Value > dealerHand.Value) Console.WriteLine("You won!");
        else if (dealerHand.Value > 21) Console.WriteLine("The dealer busted, You won! "); //If the dealer busts, the player wins regardless of their card total (https://officialgamerules.org/game-rules/blackjack/)
        else if (playerHand.Value > 21) Console.WriteLine("You busted, the dealer won. ");
        else if (dealerHand.Value <= 21) Console.WriteLine("The dealer won.");
        else Console.WriteLine("You won! ");
        Console.WriteLine($"\tYour score: {playerHand.Value} \n\tDealer score: {dealerHand.Value}");
        Console.WriteLine($"{Console.BufferHeight}, {Console.BufferWidth}");


        switch (GetInt("\nPlay again? \n\t1. Yes \n\t2. No\n", 2))
        {
            case 1: 
                Main();
                break;
            case 2:
                break;
        }
    }

    public static void Wait(int milliseconds) //Waits for a set period of time
    {
        DateTime time = DateTime.Now;
        while ((DateTime.Now - time).TotalMilliseconds < milliseconds);
    }

    public static void PrintCards(Hand person, string name)
    {

        Console.WriteLine(person.BustedMessage);
        Console.Write($"{name} Cards:\n\t");
        person.PrintCards();
        Console.WriteLine($"\tScore: {person.Value}");
    }

    public static bool DecideDealerHit(int dealerValue, int playerValue, bool playerOut)
    {
        if (dealerValue <= 17) return true; //The dealer is required to hit at 17 or below
        else if (playerOut && dealerValue < playerValue) return true; //Hit if the player is out and higher than the dealer; otherwise the dealer loses

        double dealerFactor = ((21 - dealerValue) / 10) ^ 2; //Factor the dealers closeness to 21
        double comparisonFactor = (playerValue - dealerFactor) / 10; //Factor in the value distance to the player
        double unclampedProbability = dealerFactor + (dealerFactor * comparisonFactor); //Put the factors together to determine probability
        double probability = Math.Clamp(unclampedProbability, 0.0, 1.0); //Clamp the value so it doesn't go lower than 0 or higher than 1

        Random rand = new();
        
        double hit = rand.NextDouble();
        if (hit >= probability) return true; //Decide if the dealer actually hits or misses
        else return false;
    }
    public static int GetInt(string message, int? size = null)
    {
        int num = 0;
        Console.Write(message);
        string? input = Console.ReadLine();
        
        while (input == null || !Int32.TryParse(input, out num) || num > size || num < 0) 
        {
            if (num > size || num < 0) return GetInt($"Please chose a number between 1 and {size}: ");
            return GetInt("Please enter a valid number: ", size);
        }
        return num;
    }
}