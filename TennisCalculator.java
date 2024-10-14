import java.nio.charset.StandardCharsets;
import java.util.Scanner;

public class TennisCalculator {
	public static void main(String[] args) {
		Scanner V_Scanner = new Scanner(System.in, StandardCharsets.UTF_8);

		System.out.println("Enter A for player A or D for played B");

		// {Game, Gem, Set}
		// Game on win + 15 x2 +10 (15, 30, 40, Ad = Match point) 1 = 15, 2 = 30, 3 = 40, 4 = Ad
		// Gem on win + 1	|(Gem difference == 2 -)
		// Set on win + 1	|

		int[] V_Int_PlrA = {0, 0, 0};
		int[] V_Int_PlrB = {0, 0, 0};

		char V_WinSide = 'A';

		while(true)
		{

			V_WinSide = V_Scanner.next().charAt(0);

			if(V_WinSide == 'a' || V_WinSide == 'A')
			{

				switch (V_Int_PlrA[0])
				{
					case 3:
						if(V_Int_PlrB[0] == 4)
						{
							V_Int_PlrA[0] = 3;
							V_Int_PlrB[0] = 3;
						}
						else
						{
							V_Int_PlrA[0]++;
						}
						break;
					case 4:
						V_Int_PlrA[0] = 0;
						V_Int_PlrA[1]++; // New set won
						V_Int_PlrB[0] = 0;

						break;
					default:
						V_Int_PlrA[0]++;
						break;
				}
			}
			else if(V_WinSide == 'd' || V_WinSide == 'D')
			{

				switch (V_Int_PlrB[0])
				{
					case 3:
						if(V_Int_PlrA[0] == 4)
						{
							V_Int_PlrA[0] = 3;
							V_Int_PlrB[0] = 3;
						}
						else
						{
							V_Int_PlrB[0]++;
						}
						break;
					case 4:
						V_Int_PlrB[0] = 0;
						V_Int_PlrB[1]++; // New set won
						V_Int_PlrA[0] = 0;

						break;
					default:
						V_Int_PlrB[0]++;
						break;
				}
			}
			else
			{
				System.out.println("Wrong option entered, you must enter \"A\" for player A or \"D\" for player B");
			}

			if(V_Int_PlrA[1] > 5 || V_Int_PlrB[1] > 5)
			{
				if(V_Int_PlrA[1] > V_Int_PlrB[1]+1)
				{
					V_Int_PlrA[2]++; // Set won

					V_Int_PlrA[1] = 0;
					V_Int_PlrB[1] = 0;
				}
				else if(V_Int_PlrB[1] > V_Int_PlrA[1]+1)
				{
					V_Int_PlrB[2]++; // Set won
					
					V_Int_PlrA[1] = 0;
					V_Int_PlrB[1] = 0;
				}
			}

			if(V_Int_PlrA[2] == 2 || V_Int_PlrB[2] == 2)
			{
				System.err.println("\n *********** \n Match ended! \n *********** \n");
				F_PrintStates(V_Int_PlrA, V_Int_PlrB);

				System.exit(0);
			}

			F_PrintStates(V_Int_PlrA, V_Int_PlrB);
		}
		
	}

	public static void F_PrintStates(int[] PAR_PA, int[] PAR_PB)
	{
		String[] V_Game = {"A", "B"};

		switch (PAR_PA[0]) {
			case 4:
				V_Game[0] = "Ad";
				break;
			case 3:
				V_Game[0] = "40";
				break;
			case 2:
				V_Game[0] = "30";
				break;
			case 1:
				V_Game[0] = "15";
				break;
			default:
				V_Game[0] = "0";
		}
		switch (PAR_PB[0]) {
			case 4:
				V_Game[1] = "Ad";
				break;
			case 3:
				V_Game[1] = "40";
				break;
			case 2:
				V_Game[1] = "30";
				break;
			case 1:
				V_Game[1] = "15";
				break;
			default:
				V_Game[1] = "0";
		}

		System.out.println("Player A | Player B \n | Game | \t Gem | \t Set | \n|"+ V_Game[0]+" : "+V_Game[1]+"|\t |"+ PAR_PA[1]+" : "+PAR_PB[1]+"|\t |"+ PAR_PA[2]+" : "+PAR_PB[2]+"|\t |");
	}
}
