/*
--[[� Mat�j Zahradn�k [The Universality / Iterik Nova], � Iterik Viscela Nova
Development Standard:	IVN_ATK-ATL_Provisions:TERA:761:XR
--��������������������������������
Project leader:		The_Universality - zahra.matej@gmail.com
Supervisor:			The_Universality - zahra.matej@gmail.com
Scripted by: 		The_Universality - zahra.matej@gmail.com
--��������������������������������
Created at: 		08:30 [UTC+1] | 26.11.2024 [D.M.Y]
Version: 			0.00.01.U | 0.00.007.D
Lastly edited:		11:49 [UTC+1] | 28.11.2024 [D.M.Y]
--��������������������������������
Related document: 	Documentation file: Not documented
Script purpose:		Fetching images by user option selection, save image ability, show previous image ability
]]
*/

using System.IO;
using System.Linq;
using System.Net.Http;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;

using Newtonsoft.Json;
using System;

namespace TestLearn_AnimeGirls_API;

public class SysC_ActiveSet : INotifyPropertyChanged
{
	private bool V_P_NSFWOptionsHidden;
	public bool V_G_SFWOptionsHidden
	{
		get => !V_P_NSFWOptionsHidden;
	}

	public bool V_G_NSFWOptionsHidden
	{
		get => V_P_NSFWOptionsHidden;
		set
		{
			if(V_P_NSFWOptionsHidden	!= value)
			{
				V_P_NSFWOptionsHidden	= value;
				OnPropertyChanged(nameof(V_G_NSFWOptionsHidden));
				OnPropertyChanged(nameof(V_G_SFWOptionsHidden));
				// nameof(Variable_X) returns the name of the variable as a string
				// (When I'm writing under line, it means this is a note for me to understand how does this line work)
			}
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}

public partial class MainWindow : Window
{
	string V_String_APIURL	= "https://api.waifu.pics/";
	List<Stream> V_StreamList_Images = new List<Stream>();
	Stream V_LatestImage;

	public MainWindow()
	{
		InitializeComponent();
		DataContext	= new SysC_ActiveSet { V_G_NSFWOptionsHidden = false};

		CBox_Category.Focus();

		//this.Icon	= new WindowIcon("avares://TestLearn_AnimeGirls_API/Assets/IVN_MainLogo_1to1.png");
	}

	// Pre declarations & auto trigger
	// ��������������������������������������������������������������������������������������������������������������������������������
	// Code functions

	async void F_Async_SaveFile_RNil(Image PAR_ImageControl)
	{
		var V_Any_TopLevel	= TopLevel.GetTopLevel(this); 
		// A type of service?
		// Documentation says "Get top level from the current control." :: <>Need to check on this<> ::

		var V_Any_File		= await V_Any_TopLevel.StorageProvider.SaveFilePickerAsync(new Avalonia.Platform.Storage.FilePickerSaveOptions
		{
			Title			= "Image save",
			FileTypeChoices	= new[] { FilePickerFileTypes.ImageJpg, FilePickerFileTypes.ImagePng, FilePickerFileTypes.ImageWebp }
		});

		if(V_Any_File		!= null && PAR_ImageControl.Source is Bitmap V_Bitmap_Image)
		{	// "Image_APIShow.Source is Bitmap V_Bitmap_Image" is technicaly same as "Bitmap V_Bitmap_Image = Image_APIShow.Source as Bitmap"
			// "Bitmap V_Bitmap_Image = Image_APIShow.Source as Bitmap" trys to cast the data types (can be used in scopes)
			// "Image_APIShow.Source is Bitmap V_Bitmap_Image" (can be used in conditional statements only)
			// checks whether the cast can be performed and saves it to variable if it can be performed [Value is DataType Variable] - It declares and initiates the variable in the conditional 'scope'
			FileStream V_FileS	= new FileStream(V_Any_File.Path.LocalPath.ToString(), FileMode.Create);

			V_Bitmap_Image.Save(V_FileS);

			V_FileS.Close();
		}

		V_Any_File			= null; // Prevents Appendation of new ['V_Any_File'] to the previous ['V_Any_File']
	}

	async void F_Async_GetImage_RNil(ComboBox PAR_nSFWOption, ComboBox PAR_CategoryOption, Image PAR_ImageDisplayer)
	{
		string V_String_nSFW			= PAR_nSFWOption.SelectedIndex	== 0 ? "sfw/" : "nsfw/";
		ComboBoxItem V_SelectedItem		= PAR_CategoryOption.SelectedValue as ComboBoxItem;

		HttpClient V_Client				= new HttpClient();
		HttpResponseMessage V_Response	= await V_Client.GetAsync(V_String_APIURL+V_String_nSFW+V_SelectedItem.Content.ToString().ToLower()); // Gets the API JSON with the URL to Image

		if(V_Response.IsSuccessStatusCode)
		{
			string V_String_Response	= await V_Response.Content.ReadAsStringAsync();

			dynamic V_Test				= JsonConvert.DeserializeObject<dynamic>(V_String_Response);

			try
			{
				HttpClient V_Http_ImageFetch= new HttpClient();
				HttpResponseMessage V_Image	= await V_Http_ImageFetch.GetAsync(V_Test.url.ToString()); // Fetch the image
				V_Image.EnsureSuccessStatusCode();

				V_LatestImage				= await V_Image.Content.ReadAsStreamAsync();

				PAR_ImageDisplayer.Source	= new Bitmap(V_LatestImage); // Convert image stream to Bitmap and make it Image control source
				V_StreamList_Images.Add(V_LatestImage);
			}
			catch
			{
				Debug.WriteLine("An error occured");
			}
		}
		else
		{
			Debug.WriteLine("Error fetching data");
		}
	}

	void F_GetDrawPreviousImage_RNil(Image PAR_ImageDisplayer)
	{Debug.WriteLine(V_StreamList_Images.Count);
		if(V_StreamList_Images.Count	< 2)
		{
			return;
		}

		Stream V_Stream_PreviousImg		= V_StreamList_Images.ElementAt(V_StreamList_Images.Count-2);
		V_StreamList_Images.RemoveAt(V_StreamList_Images.Count-2);
Debug.WriteLine(V_StreamList_Images.Count+" - after remove");

		V_Stream_PreviousImg.Position	= 0;

		PAR_ImageDisplayer.Source		= new Bitmap(V_Stream_PreviousImg);

		V_Stream_PreviousImg.Close();
		V_Stream_PreviousImg.Dispose();
	}

	void F_GetDrawLatestImage_RNil(Image PAR_ImageDisplayer)
	{
		if(V_StreamList_Images.Count< 1)
		{
			return;
		}

		V_StreamList_Images.Clear();
		
		V_LatestImage.Position		= 0;

		PAR_ImageDisplayer.Source	= new Bitmap(V_LatestImage);
		V_StreamList_Images.Add(V_LatestImage);
	}

	void F_QuitApplication_RNil()
	{
		// Bro... I miss my Application.Current.Shutdown() quite a bit
		(Application.Current.ApplicationLifetime	as IClassicDesktopStyleApplicationLifetime)?.MainWindow.Close();
	}

	// Code functions
	// ��������������������������������������������������������������������������������������������������������������������������������
	// Avalonia AXAML trigged functions

	void F_SaveFileRequest_RNil(object? C_Object, Avalonia.Interactivity.RoutedEventArgs C_Event)
	{
		F_Async_SaveFile_RNil(Image_APIShow);
	}

	void F_RequestImage_RNil(object? C_Object, Avalonia.Interactivity.RoutedEventArgs C_Event)
	{
		F_Async_GetImage_RNil(CBox_nSFW, CBox_Category, Image_APIShow);
	}

	void F_RequestPreviousImage_RNil(object? C_Object, Avalonia.Interactivity.RoutedEventArgs C_Event)
	{
		F_GetDrawPreviousImage_RNil(Image_APIShow);
	}

	void F_RequestLastImage_RNil(object? C_Object, Avalonia.Interactivity.RoutedEventArgs C_Event)
	{
		F_GetDrawLatestImage_RNil(Image_APIShow);
	}

	void F_QuitApp_RNil(object? C_Object, Avalonia.Interactivity.RoutedEventArgs C_Event)
	{
		F_QuitApplication_RNil();
	}

	void F_KeyUp_RNil(object? C_Object, Avalonia.Input.KeyEventArgs C_Key)
	{
		if(C_Key.KeyModifiers == KeyModifiers.Control)
		{
			switch(C_Key.Key)
			{
				case Key.N:

					F_Async_GetImage_RNil(CBox_nSFW, CBox_Category, Image_APIShow);
					break;

				case Key.S:

					F_Async_SaveFile_RNil(Image_APIShow);
					break;

				case Key.Z:

					F_GetDrawPreviousImage_RNil(Image_APIShow);
					break;

				case Key.Y:

					F_GetDrawLatestImage_RNil(Image_APIShow);
					break;

				case Key.Q:

					F_QuitApplication_RNil();
					break;
			}
		}
	}

	async void F_ShowMyGitHub_RNil(object? C_Object, Avalonia.Interactivity.RoutedEventArgs C_Event)
	{
		var V_Any_TopLevel	= TopLevel.GetTopLevel(this);

		await V_Any_TopLevel.Launcher.LaunchUriAsync(new Uri("https://github.com/TheUniversality/SchoolStuff"));
	}
}