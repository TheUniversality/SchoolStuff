/*--[[
© IVN - Iterik Viscela Nova, © Matěj Zahradník [The Universality / Iterik Nova]
Development Standard:	IVN_ATK-ATL_Provisions:TERA:761:XR
––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––
Project leader:			The Universality - zahra.matej@gmail.com
Supervisor:				The Universality - zahra.matej@gmail.com
Scripted by:			The Universality - zahra.matej@gmail.com
--––––––––––––––––––––––––––––––––
Created at:				09:00 [UTC+1] | 25.10.2024 [D.M.Y]
Version:				0.02.01.U | 0.00.009.D
Lastly edited:			23:33 [UTC+1] | 28.10.2024 [D.M.Y]
--––––––––––––––––––––––––––––––––
Related document:		Not evidenced
Script purpose:			Tennis Calculator
]]*/

using System.Windows;
using System.Windows.Controls;
using System.Text.Json;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using System.Windows.Input;
using System;

namespace Tennis
{
	public class SysC_JSONDataHandler
	{
		public string A		{ get; set; }
		public string B		{ get; set; }
		public bool Finish	{ get; set; }
		public int[] Score	{ get; set; }
	}

	public partial class MainWindow:Window
	{
		bool V_Bool_Finished	= false;
		bool V_Bool_SkipWarning	= false;
		bool V_Bool_FileOpened	= true;

		int[] V_Int_PlrA		= {0, 0, 0}; // TeamA {0 = Game, 1 = Gem, 2 = Set}
		int[] V_Int_PlrB		= {0, 0, 0}; // TeamB {0 = Game, 1 = Gem, 2 = Set}

		List<int> V_IntList_ScoreLog		= new List<int>{};
		List<int> V_IntList_UndoneScore		= new List<int>{};

		int V_Int_MatchCount	= 0;	// Match counting

		Label V_TLabel_MatchCount;
		Label V_TLabel_TeamName;
		Label V_TLabel_Outcome;

		TextBox V_TBox_TeamAName;
		TextBox V_TBox_TeamBName;

		StackPanel V_SPanel_Round;
		StackPanel V_SPanel_Game;
		StackPanel V_SPanel_Gem;
		StackPanel V_SPanel_Set;

		ScrollViewer V_ScrollViewer;

		MenuItem V_MenuI_Recent;

		public MainWindow()
		{
			InitializeComponent();

			V_TLabel_TeamName	= TLabel_TeamName;
			V_TLabel_Outcome	= TLabel_Outcome;

			V_TBox_TeamAName	= TBox_TeamA;
			V_TBox_TeamBName	= TBox_TeamB;

			V_SPanel_Round		= SPanel_Round;
			V_SPanel_Game		= SPanel_Game;
			V_SPanel_Gem		= SPanel_Gem;
			V_SPanel_Set		= SPanel_Set;

			V_ScrollViewer		= ScrollViewer_ScoreLog;

			V_MenuI_Recent		= MenuI_Recent;

			Btn_NewGame.Focus();
		}

		private void F_CompleteVictory_RNil(String PAR_TeamName)
		{

			V_TLabel_Outcome.Content		= PAR_TeamName+" has won the match within "+V_Int_MatchCount+" rounds.";
		}

		private void F_ShowStates_RNil(int[] PAR_PA, int[] PAR_PB)
		{
			String[] V_Game		= {"A", "B"}; // Translation of 0-4 ("enum") into String for tennis counting {0 = TeamA, 1 = TeamB}

			switch (PAR_PA[0]) {
				case 4:
					V_Game[0]	= "Ad";
					break;
				case 3:
					V_Game[0]	= "40";
					break;
				case 2:
					V_Game[0]	= "30";
					break;
				case 1:
					V_Game[0]	= "15";
					break;
				default:
					V_Game[0]	= "0";
					break;
			}

			switch (PAR_PB[0]) {
				case 4:
					V_Game[1]	= "Ad";
					break;
				case 3:
					V_Game[1]	= "40";
					break;
				case 2:
					V_Game[1]	= "30";
					break;
				case 1:
					V_Game[1]	= "15";
					break;
				default:
					V_Game[1]	= "0";
					break;
			}

			if(V_Bool_Finished		== true)
			{
				return;
			}

			Label V_TLabel_Round	= new Label();
			V_TLabel_Round.Content	= "# "+V_Int_MatchCount;
			Grid.SetColumn(V_TLabel_Round, 0);


			Label V_TLabel_Game		= new Label();
			V_TLabel_Game.Content	= V_Game[0]+":"+V_Game[1];
			Grid.SetColumn(V_TLabel_Game, 1);


			Label V_TLabel_Gem		= new Label();
			V_TLabel_Gem.Content	= V_Int_PlrA[1]+":"+V_Int_PlrB[1];
			Grid.SetColumn(V_TLabel_Gem, 2);


			Label V_TLabel_Set		= new Label();
			V_TLabel_Set.Content	= V_Int_PlrA[2]+":"+V_Int_PlrB[2];
			Grid.SetColumn(V_TLabel_Set, 3);


			V_TLabel_Round.HorizontalAlignment	= HorizontalAlignment.Center;
			V_TLabel_Game.HorizontalAlignment	= HorizontalAlignment.Center;
			V_TLabel_Gem.HorizontalAlignment	= HorizontalAlignment.Center;
			V_TLabel_Set.HorizontalAlignment	= HorizontalAlignment.Center;


			V_SPanel_Round.Children.Add(V_TLabel_Round);
			V_SPanel_Game.Children.Add(V_TLabel_Game);
			V_SPanel_Gem.Children.Add(V_TLabel_Gem);
			V_SPanel_Set.Children.Add(V_TLabel_Set);

			ScrollViewer_ScoreLog.ScrollToBottom();
		}

		private void F_DoCalculations_RNil()
		{

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

			if(V_Bool_Finished		== true)
			{
				return;
			}

			F_ShowStates_RNil(V_Int_PlrA, V_Int_PlrB);

			if(V_Int_PlrA[2]		== 2)
			{
				F_CompleteVictory_RNil(V_TBox_TeamAName.Text);

				V_Bool_Finished		= true;
			}
			else if(V_Int_PlrB[2]	== 2)
			{
				F_CompleteVictory_RNil(V_TBox_TeamBName.Text);

				V_Bool_Finished		= true;
			}
		}

		private void F_AddPlayerPoint_RNil(int[] PAR_Winner, int[] PAR_Opponent, int PAR_Team_Int, bool PAR_FileLoading_Bool, bool PAR_UndoingAction_Bool)
		{
			if(V_Bool_Finished			== true)
			{
				MessageBox.Show("This math has ended! Click \"Start new game\" if you want to start new match.", "Match ended", MessageBoxButton.OK, MessageBoxImage.Warning);

				return;
			}

			if(PAR_UndoingAction_Bool	== false)
			{
				V_IntList_UndoneScore.Clear();
			}

			V_Int_MatchCount++;

			V_IntList_ScoreLog.Add(PAR_Team_Int);

			switch(PAR_Winner[0])
			{
				case 3:
					if(PAR_Opponent[0]	== 4)
					{
						PAR_Winner[0]	= 3;
						PAR_Opponent[0]	= 3;
					}
					else if(PAR_Opponent[0]== 3)
					{
						PAR_Winner[0]++;
					}
					else
					{
						PAR_Winner[0]	= 0;
						PAR_Winner[1]++; // New set won
						PAR_Opponent[0]	= 0;
					}
					break;
				case 4:
					PAR_Winner[0]	= 0;
					PAR_Winner[1]++; // New set won
					PAR_Opponent[0]	= 0;

					break;
				default:
					PAR_Winner[0]++;
					break;
			}

			F_DoCalculations_RNil();

			if(PAR_FileLoading_Bool	!= true)
			{
				V_Bool_FileOpened	= false;
			}
		}

		private void F_SaveMatchLog_RNil()
		{
			var V_JSON_MatchData	= new
			{
				A		= V_TBox_TeamAName.Text,
				B		= V_TBox_TeamBName.Text,
				Score	= V_IntList_ScoreLog.ToArray()
			};

			string V_String_SerializedJSON	= JsonSerializer.Serialize(V_JSON_MatchData);

			SaveFileDialog V_SFD_MatchData	= new SaveFileDialog();
			V_SFD_MatchData.Filter			= "JSON file (*.json)| *.json";
			V_SFD_MatchData.FilterIndex		= 1;
			V_SFD_MatchData.Title			= "Save "+V_TBox_TeamAName.Text+" & "+V_TBox_TeamBName.Text+" match";
			V_SFD_MatchData.DefaultDirectory= Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			V_SFD_MatchData.ShowDialog();

			if(V_SFD_MatchData.FileName		!= "")
			{
				using (StreamWriter V_StreamWriter		= new StreamWriter(V_SFD_MatchData.FileName))
				{
					V_StreamWriter.Write(V_String_SerializedJSON);
					
					V_IntList_ScoreLog.Clear();
				}
			}
			else
			{
				MessageBoxResult V_MBQ_Result	= MessageBox.Show("There was an error saving your file. Do you want to retry?", "Save fail", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.No);

				switch(V_MBQ_Result)
				{
					case MessageBoxResult.Yes:

						F_SaveMatchLog_RNil();
						break;

					case MessageBoxResult.No:

						V_IntList_ScoreLog.Clear();
						return;
				}
			}
		}

		private void F_AddRecentMatch_RNil(string PAR_FilePath)
		{
			if(File.Exists(PAR_FilePath))
			{
				foreach (MenuItem I_MenuI in V_MenuI_Recent.Items)
				{
					if(PAR_FilePath		== I_MenuI.Header.ToString())
					{
						return;
					}
				}

				MenuItem V_MenuI_New	= new MenuItem();
				V_MenuI_New.Header		= PAR_FilePath;
				V_MenuI_New.Click		+= new RoutedEventHandler(F_LoadDirectGame_RNil);
				V_MenuI_Recent.Items.Add(V_MenuI_New);
			}
		}

		private void F_ShowMBHint_RNil()
		{
			MessageBox.Show("Enter Team1 & Team2 name into the text fields. Then press \"Ctrl+N\" or hit the \"Start new match\" button to start your match. \n ***** \n"  +
			"To add points to your team press \"A\" key or click the left \"... Score\" button to add point to Team1 or press \"D\" or click the right \"... Score\" button to add point to Team2 \n ***** \n"+
			"You can then save your match by pressing \"Ctrl+S\" or clicking the \"Save match data\" button. To load save matches, press \"Ctrl+O\" or click the \"Load match\" button.", "Hint", MessageBoxButton.OK, MessageBoxImage.Question);
		}

		private void F_ShowMBUpdateNews_RNil()
		{
			if(File.Exists($"{Environment.CurrentDirectory}\\ReleaseNews.txt")	== true)
			{
				StreamReader V_SReader_ReleaseNotes	= new StreamReader($"{Environment.CurrentDirectory}\\ReleaseNews.txt");

				MessageBox.Show(V_SReader_ReleaseNotes.ReadToEnd(), "Release news", MessageBoxButton.OK, MessageBoxImage.Information);

				return;
			}
			
			MessageBox.Show("Release note file must have been deleted!", "Unable to get news", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		// MARK: Event functions

		private void F_StartNewGame_RNil(object C_Sender, RoutedEventArgs C_Event)
		{
			if(V_Bool_FileOpened	== false && V_Bool_SkipWarning	== false)
			{
				// MBQ - Message Box Result
				MessageBoxResult V_MBQ_Result	= MessageBox.Show("Do you want to save the match data?", "Match download", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

				switch(V_MBQ_Result)
				{
					case MessageBoxResult.Yes:

						F_SaveMatchLog_RNil();
						break;
				}
			}

			V_IntList_ScoreLog.Clear();

			if(Btn_NewGame == C_Event.Source as Button)
			{
				V_Bool_FileOpened	= false;
			}

			V_Bool_SkipWarning		= false;

			V_SPanel_Round.Children.Clear();
			V_SPanel_Game.Children.Clear();
			V_SPanel_Gem.Children.Clear();
			V_SPanel_Set.Children.Clear();

			V_Bool_Finished			= false;

			V_Int_MatchCount		= 0;

			V_Int_PlrA				= new int[] {0, 0, 0};
			V_Int_PlrB				= new int[] {0, 0, 0};

			V_TLabel_Outcome.Content	= "Outcome";

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

			V_TLabel_TeamName.Content	= TBox_TeamA.Text+" | "+TBox_TeamB.Text;
		}

		private void F_ProcessMatchData_RNil(object C_Sender, RoutedEventArgs C_Event, string PAR_FilePath)
		{
			string V_String_MatchData= File.ReadAllText(PAR_FilePath);

			SysC_JSONDataHandler V_ClassMatchData	= JsonSerializer.Deserialize<SysC_JSONDataHandler>(V_String_MatchData);

			F_AddRecentMatch_RNil(PAR_FilePath);

			if(V_ClassMatchData.A	== null)
			{
				MessageBox.Show("Unable to load. The JSON file wasn't created with this application! ", "Load fail", MessageBoxButton.OK, MessageBoxImage.Error);

				return;
			}

			V_TBox_TeamAName.Text	= V_ClassMatchData.A;
			V_TBox_TeamBName.Text	= V_ClassMatchData.B;

			V_Bool_SkipWarning		= true;

			F_StartNewGame_RNil(C_Sender, C_Event);

			V_Bool_FileOpened		= true;

			for(int I_Index=0; I_Index < V_ClassMatchData.Score.Length; I_Index++)
			{
				if (V_ClassMatchData.Score[I_Index] == 0)
				{
					F_AddPlayerPoint_RNil(V_Int_PlrA, V_Int_PlrB, V_ClassMatchData.Score[I_Index], true, false);
				}
				else
				{
					F_AddPlayerPoint_RNil(V_Int_PlrB, V_Int_PlrA, V_ClassMatchData.Score[I_Index], true, false);
				}
			}
		}

		private void F_LoadGame_RNil(object C_Sender, RoutedEventArgs C_Event)
		{
			if(V_Bool_FileOpened	== false && V_Bool_SkipWarning	== false)
			{
				// MBQ - Message Box Result
				MessageBoxResult V_MBQ_Result	= MessageBox.Show("Loading match data will result in current match data deletion. Do you want to save current match before loading another one?", "Match discard", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);

				switch(V_MBQ_Result)
				{
					case MessageBoxResult.Yes:

						F_SaveMatchLog_RNil();
						break;

					case MessageBoxResult.Cancel:

						return;
				}
			}

			OpenFileDialog V_OFD_MatchData	= new OpenFileDialog();
			V_OFD_MatchData.Filter			= "JSON file (*.json)| *.json";
			V_OFD_MatchData.FilterIndex		= 1;
			V_OFD_MatchData.Title			= "Load match data";
			V_OFD_MatchData.ShowDialog();

			if(V_OFD_MatchData.FileName		!= "")
			{
				try
				{
					F_ProcessMatchData_RNil(C_Sender, C_Event, V_OFD_MatchData.FileName);
				}
				catch (Exception ex)
				{
					MessageBox.Show($"An error occurred when reading the file: {ex.Message}", "Load fail", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			else
			{
				MessageBox.Show("No file entered, or another error detected", "Load fail", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void F_LoadDirectGame_RNil(object C_Sender, RoutedEventArgs C_Event)
		{
			if(V_Bool_FileOpened	== false && V_Bool_SkipWarning	== false)
			{
				// MBQ - Message Box Result
				MessageBoxResult V_MBQ_Result	= MessageBox.Show("Loading match data will result in current match data deletion. Do you want to save current match before loading another one?", "Match discard", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);

				switch(V_MBQ_Result)
				{
					case MessageBoxResult.Yes:

						F_SaveMatchLog_RNil();
						break;

					case MessageBoxResult.Cancel:

						return;
				}
			}

			MenuItem V_MenuI_UsedControl	= C_Event.Source as MenuItem;

			if(File.Exists(V_MenuI_UsedControl.Header.ToString()))
			{
				try
				{
					F_ProcessMatchData_RNil(C_Sender, C_Event, V_MenuI_UsedControl.Header.ToString());
				}
				catch (Exception ex)
				{
					MessageBox.Show($"An error occurred when reading the file: {ex.Message}", "Load fail", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			else
			{
				MessageBox.Show("No file entered, or another error detected", "Load fail", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void F_DownLoadGame_RNil(object C_Sender, RoutedEventArgs C_Event)
		{
			F_SaveMatchLog_RNil();
		}

		private void F_TeamAScore_RNil(object C_Sender, RoutedEventArgs C_Event)
		{
			F_AddPlayerPoint_RNil(V_Int_PlrA, V_Int_PlrB, 0, false, false);
		}

		private void F_TeamBScore_RNil(object C_Sender, RoutedEventArgs C_Event)
		{
			F_AddPlayerPoint_RNil(V_Int_PlrB, V_Int_PlrA, 1, false, false);
		}

		private void F_ShowHint_RNil(object C_Sender, RoutedEventArgs C_Event)
		{
			F_ShowMBHint_RNil();
		}

		private void F_ShowUpdateNews_RNil(object C_Sender, RoutedEventArgs C_Event)
		{
			F_ShowMBUpdateNews_RNil();
		}

		private void F_Quit_RNil(object C_Sender, RoutedEventArgs C_Event)
		{
			Application.Current.Shutdown();
		}

		private void F_UndoProcessing_RNil(object C_Sender, RoutedEventArgs C_Event)
		{
			if(V_IntList_ScoreLog.Count < 1)
			{
				return;
			}

			V_IntList_UndoneScore.Add(V_IntList_ScoreLog.ElementAt(V_IntList_ScoreLog.Count-1));

			V_IntList_ScoreLog.RemoveAt(V_IntList_ScoreLog.Count-1);

			V_Bool_SkipWarning		= true;

			int[] V_Array_ScoreLog	= V_IntList_ScoreLog.ToArray();

			F_StartNewGame_RNil(C_Sender, C_Event);

			V_Bool_FileOpened		= true;

			for(int I_Index=0; I_Index < V_Array_ScoreLog.Length; I_Index++)
			{
				if (V_Array_ScoreLog[I_Index] == 0)
				{
					F_AddPlayerPoint_RNil(V_Int_PlrA, V_Int_PlrB, V_Array_ScoreLog[I_Index], true, true);
				}
				else
				{
					F_AddPlayerPoint_RNil(V_Int_PlrB, V_Int_PlrA, V_Array_ScoreLog[I_Index], true, true);
				}
			}

			V_Bool_SkipWarning		= false;
			V_Bool_FileOpened		= false;
		}

		private void F_Undo_RNil(object C_Sender, RoutedEventArgs C_Event)
		{
			F_UndoProcessing_RNil(C_Sender, C_Event);
		}

		private void F_Redo_RNil(object C_Sender, RoutedEventArgs C_Event)
		{
			if(V_IntList_UndoneScore.Count < 1)
			{
				return;
			}

			if(V_IntList_UndoneScore.ToArray()[V_IntList_UndoneScore.Count-1]	== 0)
			{
				F_AddPlayerPoint_RNil(V_Int_PlrA, V_Int_PlrB, 0, false, true);
			}
			else
			{
				F_AddPlayerPoint_RNil(V_Int_PlrB, V_Int_PlrA, 1, false, true);
			}

			V_IntList_UndoneScore.RemoveAt(V_IntList_UndoneScore.Count-1);
		}

		private void F_KeyBoardInput_RNil(object C_Sender, KeyEventArgs C_Event)
		{
			if(Keyboard.Modifiers	== ModifierKeys.Control)
			{
				switch(C_Event.Key)
				{
					case Key.N:

						F_StartNewGame_RNil(C_Sender, C_Event);
						break;

					case Key.S:

						F_SaveMatchLog_RNil();
						break;

					case Key.O:

						F_LoadGame_RNil(C_Sender, C_Event);
						break;

					case Key.Z:

						F_UndoProcessing_RNil(C_Sender, C_Event);
						break;

					case Key.Y:

						if(V_IntList_UndoneScore.Count < 1)
						{
							return;
						}

						if(V_IntList_UndoneScore.ToArray()[V_IntList_UndoneScore.Count-1]	== 0)
						{
							F_AddPlayerPoint_RNil(V_Int_PlrA, V_Int_PlrB, 0, false, true);
						}
						else
						{
							F_AddPlayerPoint_RNil(V_Int_PlrB, V_Int_PlrA, 1, false, true);
						}

						V_IntList_UndoneScore.RemoveAt(V_IntList_UndoneScore.Count-1);

						break;
				}
			}

			if(C_Event.Key	== Key.Escape || C_Event.Key	== Key.Enter)
			{
				Btn_NewGame.Focus();
			}

			if(V_TBox_TeamAName.IsKeyboardFocused	== false && V_TBox_TeamBName.IsKeyboardFocused	== false)
			{
				switch(C_Event.Key)
				{
					case Key.A:
					case Key.D1:

						F_AddPlayerPoint_RNil(V_Int_PlrA, V_Int_PlrB, 0, false, false);
						break;

					case Key.D:
					case Key.D2:

						F_AddPlayerPoint_RNil(V_Int_PlrB, V_Int_PlrA, 1, false, false);
						break;

					case Key.F1:

						F_ShowMBHint_RNil();

						break;

					case Key.F12:

						F_ShowMBUpdateNews_RNil();

						break;
				}
			}
		}

	}
}