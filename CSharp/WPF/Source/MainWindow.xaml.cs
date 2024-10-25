/*--[[
© IVN - Iterik Viscela Nova, © Matěj Zahradník [The Universality / Iterik Nova]
Development Standard:	IVN_ATK-ATL_Provisions:TERA:761:XR
––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––
Project leader:			The Universality - zahra.matej@gmail.com
Supervisor:				The Universality - zahra.matej@gmail.com
Scripted by:			The Universality - zahra.matej@gmail.com
--––––––––––––––––––––––––––––––––
Created at:				09:00 [UTC+1] | 25.10.2024 [D.M.Y]
Version:				0.00.01.U | 0.00.003.D
Lastly edited:			10:11 [UTC+1] | 25.10.2024 [D.M.Y]
--––––––––––––––––––––––––––––––––
Related document:		Not evidenced
Script purpose:			Tennis Calculator
]]*/

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tennis
{
	public partial class MainWindow:Window
	{
		// {Game, Gem, Set}
		// Game on win + 15 x2 +10 (15, 30, 40, Ad = Match point) 1 = 15, 2 = 30, 3 = 40, 4 = Ad
		// Gem on win + 1	|(Gem difference == 2 -)
		// Set on win + 1	|

		int[] V_Int_PlrA		= {0, 0, 0};
		int[] V_Int_PlrB		= {0, 0, 0};

		int V_Int_MatchCount	= 0;

		Label V_TLabel_MatchCount;
		Label V_TLabel_CurScore;
		Label V_TLabel_Outcome;

		TextBox V_TBox_TeamAName;
		TextBox V_TBox_TeamBName;

		CheckBox V_ChBox_CountMatches;

		public MainWindow()
		{
			InitializeComponent();

			V_TLabel_MatchCount	= TLabel_MatchCount;
			V_TLabel_CurScore	= TLabel_Score;
			V_TLabel_Outcome	= TLabel_Outcome;

			V_TBox_TeamAName	= TBox_TeamA;
			V_TBox_TeamBName	= TBox_TeamB;

			V_ChBox_CountMatches= ChBox_CountMatches;
		}

		 private void F_CompleteVictory_RNil(String PAR_TeamName)
		{
			if(ChBox_CountMatches.IsChecked == true)
			{
				V_TLabel_Outcome.Content	= PAR_TeamName+" has won the match within "+V_Int_MatchCount+" mathes.";

				return;
			}

			V_TLabel_Outcome.Content		= PAR_TeamName+" has won the match!";
		}

		private void F_ShowStates_RNil(int[] PAR_PA, int[] PAR_PB)
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
					break;
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
					break;
			}

			//System.out.println("Player A | Player B \n | Game | \t Gem | \t Set | \n|"+ V_Game[0]+" : "+V_Game[1]+"|\t |"+ PAR_PA[1]+" : "+PAR_PB[1]+"|\t |"+ PAR_PA[2]+" : "+PAR_PB[2]+"|\t |");
			V_TLabel_CurScore.Content = "\t"+V_TBox_TeamAName.Text+"\t | \t"+V_TBox_TeamBName.Text+"\n| Game \t | \t Gem \t | \t Set \t|\n| "+ V_Game[0]+" : "+V_Game[1]+"\t | \t"+ PAR_PA[1]+" : "+PAR_PB[1]+"\t | \t"+ PAR_PA[2]+" : "+PAR_PB[2]+"\t | \t";
		}

		private void F_DoCalculations_RNil()
		{
			V_TLabel_MatchCount.Content	= "";

			if(V_ChBox_CountMatches.IsChecked	== true)
			{
				V_Int_MatchCount++;

				V_TLabel_MatchCount.Content		= "Current match is: " + V_Int_MatchCount;
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

			F_ShowStates_RNil(V_Int_PlrA, V_Int_PlrB);

			if(V_Int_PlrA[2] == 2)
			{
				F_CompleteVictory_RNil(V_TBox_TeamAName.Text);
			}
			else if(V_Int_PlrB[2] == 2)
			{
				F_CompleteVictory_RNil(V_TBox_TeamBName.Text);
			}
		}

		// MARK: Event functions

		private void F_StartNewGame_RNil(object C_Sender, RoutedEventArgs C_Event)
		{
			V_Int_MatchCount		= 0;

			V_Int_PlrA				= new int[] {0, 0, 0};
			V_Int_PlrB				= new int[] {0, 0, 0};

			V_TLabel_Outcome.Content	= "Outcome";
			V_TLabel_MatchCount.Content	= "Current match is: " + V_Int_MatchCount;

			if(string.IsNullOrEmpty(V_TBox_TeamAName.Text))
			{
				V_TBox_TeamAName.Text	= "TEAM 1";
			}
			if(string.IsNullOrEmpty(V_TBox_TeamBName.Text))
			{
				V_TBox_TeamBName.Text	= "TEAM 2";
			}

			Btn_TeamAScore.Content		= TBox_TeamA.Text+" Score";
			Btn_TeamBScore.Content		= TBox_TeamB.Text+" Score";

			V_TLabel_CurScore.Content	= "\t"+TBox_TeamA.Text+" \t|\t "+TBox_TeamB.Text+"\n| Game \t | \t Gem \t | \t Set \t|\n| "+ V_Int_PlrA[0]+" : "+V_Int_PlrB[1]+"\t | \t"+ V_Int_PlrA[1]+" : "+V_Int_PlrB[1]+"\t | \t"+ V_Int_PlrA[2]+" : "+V_Int_PlrB[2]+"\t |";
		}

		private void F_TeamAScore_RNil(object C_Sender, RoutedEventArgs C_Event)
		{
			if(string.IsNullOrEmpty(V_TBox_TeamAName.Text))
			{
				F_StartNewGame_RNil(C_Sender, C_Event);
			}

			switch(V_Int_PlrA[0])
			{
				case 3:
					if(V_Int_PlrB[0]	== 4)
					{
						V_Int_PlrA[0]	= 3;
						V_Int_PlrB[0]	= 3;
					}
					else if(V_Int_PlrB[0]== 3)
					{
						V_Int_PlrA[0]++;
					}
					else
					{
						V_Int_PlrA[0]	= 0;
						V_Int_PlrA[1]++; // New set won
						V_Int_PlrB[0]	= 0;
					}
					break;
				case 4:
					V_Int_PlrA[0]	= 0;
					V_Int_PlrA[1]++; // New set won
					V_Int_PlrB[0]	= 0;

					break;
				default:
					V_Int_PlrA[0]++;
					break;
			}

			F_DoCalculations_RNil();
		}

		private void F_TeamBScore_RNil(object C_Sender, RoutedEventArgs C_Event)
		{
			if(string.IsNullOrEmpty(V_TBox_TeamBName.Text))
			{
				F_StartNewGame_RNil(C_Sender, C_Event);
			}

			switch (V_Int_PlrB[0])
			{
				case 3:
					if(V_Int_PlrA[0]	== 4)
					{
						V_Int_PlrA[0]	= 3;
						V_Int_PlrB[0]	= 3;
					}
					else if(V_Int_PlrA[0]== 3)
					{
						V_Int_PlrB[0]++;
					}
					else
					{
						V_Int_PlrB[0]	= 0;
						V_Int_PlrB[1]++; // New set won
						V_Int_PlrA[0]	= 0;
					}
					break;
				case 4:
					V_Int_PlrB[0]	= 0;
					V_Int_PlrB[1]++; // New set won
					V_Int_PlrA[0]	= 0;

					break;
				default:
					V_Int_PlrB[0]++;
					break;
			}

			F_DoCalculations_RNil();
		}
	}
}