import java.nio.charset.StandardCharsets;
import java.util.InputMismatchException;
import java.util.Scanner;

public class NumbGuess
{
	public static void main(String[] args)
	{
		Scanner V_Scanner = new Scanner(System.in, StandardCharsets.UTF_8); // Where's my std::cin >> var; for real.

		int V_Int_NumMax; // Maximal guess number - Player 2 is to set this
		int V_Int_NumMin; // Minimal guess number - Player 2 is to set this
		int V_Int_GuessC = 0; // 

		int V_Int_NumToQuess; // Player 1 is to set this
		// Number between ['V_Int_NumMin'] and ['V_Int_NumMax'] (with these numbers included) Player 2 guesses

		while (true)
		{ // Player 2 input for ['V_Int_NumMin'] and ['V_Int_NumMax']
			try
			{
				System.out.print("Player 2: Enter the minimal number: ");
				int V_Int_BufNum= V_Scanner.nextInt();
				V_Int_NumMin	= V_Int_BufNum;
				System.out.println("Player 2: Enter the maximal number.");

				do { // Checks for the ['V_Int_BufNum'] number to be bigger that the ['V_Int_NumMin'] + 1

					System.out.println("The Maximal number must NOT be less than "+(V_Int_NumMin+2));

					V_Int_BufNum = V_Scanner.nextInt();
				} while (V_Int_NumMin + 2 > V_Int_BufNum);

				V_Int_NumMax = V_Int_BufNum;

				break; // When all valid, exit the loop
			}
			catch (InputMismatchException V_E)
			{ // If the input isn't INT, notify the console
				System.out.println("Enter WHOLE NUMBER!");
				V_Scanner.nextLine(); //Prevents infinite loop with active Scanner buffer
			}
		}

		System.out.println("Player 1: Enter number for player 2 to guess. Number can be any from " + V_Int_NumMin + " to " + V_Int_NumMax);

		// Player 1 is now to set number to guess
		while (true)
		{
			try
			{
				int V_Int_Num1 = V_Scanner.nextInt();

				// If the ['V_Int_NumToQuess'] number is between ['V_Int_NumMin'] and ['V_Int_NumMax'] (with these numbers included)
				// the input is valid
				if (V_Int_Num1 >= V_Int_NumMin && V_Int_Num1 <= V_Int_NumMax) {
					V_Int_NumToQuess = V_Int_Num1;

					System.out.print("\033[H\033[2J"); // Clears the console
					System.out.flush();

					break; // When all valid, exit the loop
				}
				else
				{ // Notify Player 1 that the number is not within the interval
					System.out.println("Number can be any number from " + V_Int_NumMin + " to " + V_Int_NumMax + " ONLY you MORON!");
				}
			}
			catch (InputMismatchException V_E)
			{// If the input isn't INT, notify the console
				System.out.println("Enter WHOLE NUMBER!");
				V_Scanner.nextLine(); //Prevents infinite loop with active Scanner buffer
			}
		}

		System.out.println("Player 2: Guess the number between"+V_Int_NumMin+" and "+V_Int_NumMax+".");

		while (true)
		{
			try
			{
				int V_Int_Num1 = V_Scanner.nextInt();

				if (V_Int_NumMin <= V_Int_Num1 && V_Int_Num1 <= V_Int_NumMax)
				{
					V_Int_GuessC++;
					if (V_Int_Num1 == V_Int_NumToQuess)
					{ // If guess is equal to ['V_Int_NumToQuess'], promt the user with: ↓ :and5 exit the application
						System.out.println("You got the correct number! I took you "+V_Int_GuessC+" attempt(s)");

						System.exit(0);
					}
					else if (V_Int_Num1 > V_Int_NumToQuess)
					{ // if guess is less than the ['V_Int_NumToQuess'], promt the user with: ↓
						System.out.println("Wrong number, it's less than that");
					}
					else
					{ // else guess is mrore than the ['V_Int_NumToQuess'], promt the user with: ↓
						System.out.println("Wrong number, it's more than that");
					}
					V_Scanner.nextLine(); //Prevents infinite loop with active Scanner buffer
				}
				else
				{System.out.println("You're outside the scope of "+V_Int_NumMin+" "+V_Int_NumMax);}
			}
			catch (Exception e)
			{// If the input isn't INT, notify the console
				System.out.println("Enter WHOLE NUMBER you DUMBFUCK!"); // >:(
				V_Scanner.nextLine(); //Prevents infinite loop with active Scanner buffer
			}
		}
	}
}
