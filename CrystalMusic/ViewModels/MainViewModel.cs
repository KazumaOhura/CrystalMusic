using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MSAPI = Microsoft.WindowsAPICodePack;
using System.Windows.Media.Imaging;
using System.IO;
using System.Collections.ObjectModel;

#if DEBUG
using Debug;
#endif

namespace CrystalMusic.ViewModels
{
	public class MainViewModel : Helpers.Observable
	{
		public Views.MainWindow View { get; private set; } = null;
		private Models.Player _player;
		public Models.Player Player { get => this._player; set => Set(ref this._player, value); }
		private Models.Setting _setting;
		public Models.Setting Setting { get => this._setting; set => Set(ref this._setting, value); }
		//TODO: ViewModelとModelが曖昧になっている
		public string FilePath { private get => Player.PlayFilePath; set => this.Player.PlayFilePath = value; }
		private float soundVolume = 50f;
		public int SoundVolume
		{
			get => (int)this.soundVolume;
			set
			{
				this.Set(ref this.soundVolume, value);
				this.Player.SoundVolume = this.soundVolume / 100f;
			}
		}
		private Dictionary<string, string> _musics = new Dictionary<string, string>();
		public Dictionary<string, string> Musics { get => this._musics; private set => Set(ref this._musics, value); }
		private ObservableCollection<string> _titles = new ObservableCollection<string>();
		public ObservableCollection<string> Titles { get => this._titles; private set => this._titles = value; }
		private Dictionary<int, string> _devices = new Dictionary<int, string>();
		public Dictionary<int, string> Devices { get => this._devices; private set => Set(ref this._devices, value); }

		/// <summary>
		/// MP3
		/// </summary>
		static private readonly string MP3 = @"*.mp3;";
		/// <summary>
		/// WAVE
		/// </summary>
		static private readonly string WAVE = @"*.wav;*.wave;";
		/// <summary>
		/// AAC
		/// </summary>
		static private readonly string AAC = @"*.m4a;*.aac;*.mp4;";
		/// <summary>
		/// FLAC
		/// </summary>
		static private readonly string FLAC = @"*.flac;";
		/// <summary>
		/// ファイルのダイアログフィルター
		/// </summary>
		static private readonly string FILTER = $@"Audio Files|{MP3}{WAVE}{AAC}";

		private Helpers.RelayCommand playCommand;
		public Helpers.RelayCommand PlayCommand { get => playCommand = playCommand ?? new Helpers.RelayCommand(OnPlayButtonClicked, Player.CanPlay); }
		private Helpers.RelayCommand stopCommand;
		public Helpers.RelayCommand StopCommand { get => stopCommand = stopCommand ?? new Helpers.RelayCommand(OnStopButtonClicked, Player.CanStop); }
		private Helpers.RelayCommand rewindCommand;
		public Helpers.RelayCommand RewindCommand { get => rewindCommand = rewindCommand ?? new Helpers.RelayCommand(OnRewindButtonClicked); }
		private Helpers.RelayCommand selectFolderCommand;
		public Helpers.RelayCommand SelectFolderCommand { get => selectFolderCommand = selectFolderCommand ?? new Helpers.RelayCommand(SelectFolder); }
		private Helpers.RelayCommand selectFileCommand;
		public Helpers.RelayCommand SelectFileCommand { get => selectFileCommand = selectFileCommand ?? new Helpers.RelayCommand(SelectFile); }
		public void Initialize(Views.MainWindow mainWindow)
		{
			this.View = mainWindow;
			this.View.Closed += this.OnClosed;
			this.View.Devices.DropDownClosed += OnSelectedDevice;
			this.View.AudioList.SelectionChanged += OnAudioSelected;

			//this.Setting = Models.Config.Read();
			this.Setting = new Models.Setting();
			this.Player = new Models.Player();
			this.Devices = Player.GetDevices();
		}
		private void SelectFile()
		{
			var dialog = new Microsoft.Win32.OpenFileDialog
			{
				InitialDirectory = this.Setting.Folder,
				Filter = FILTER,
				Multiselect = true
			};
			if (dialog.ShowDialog() == true)
			{
				foreach(string str in dialog.FileNames)
				{
					AddMusic(str);
				}
				//this.Player.CreateFileReader();
			}
			this.PlayCommand.OnCanExecuteChanged();
		}
		private void SelectFolder()
		{
			var dialog = new MSAPI::Dialogs.CommonOpenFileDialog
			{
				IsFolderPicker = true,
				Title = "Select Folder",
				InitialDirectory = @"C:\Music"
			};
			if (dialog.ShowDialog() == MSAPI::Dialogs.CommonFileDialogResult.Ok)
			{
				this.Setting.Folder = dialog.FileName;
			}
			this.PlayCommand.OnCanExecuteChanged();
		}
		private void AddMusic(string str)
		{
			string title = TagLib.File.Create(str).Tag.Album ?? Path.GetFileName(str);
			if (!this.Setting.Musics.ContainsKey(title)) this.Setting.Musics.Add(title, str);
			if(!this.Musics.ContainsKey(title)) this.Musics.Add(Path.GetFileName(str), str);
			if(!this.Titles.Contains(title)) this.Titles.Add(title);
		}
		private void OnAudioSelected(object sender, EventArgs e)
		{
			var item = (string)this.View.AudioList.SelectedItem;
			this.FilePath = this.Musics[item];
			this.Player.CreateFileReader();
			this.PlayCommand.OnCanExecuteChanged();
		}
		private void OnPlayButtonClicked()
		{
			this.Player.Play();
			this.View.PlayButton.Visibility = System.Windows.Visibility.Hidden;
			this.View.StopButton.Visibility = System.Windows.Visibility.Visible;
			this.StopCommand.OnCanExecuteChanged();
			#if DEBUG
			DebugConsole.WriteLine("Player.CanPlay():" + this.Player.CanPlay().ToString());
			DebugConsole.WriteLine("Player.CanStop():" + this.Player.CanStop().ToString());
			#endif
		}
		private void OnStopButtonClicked()
		{
			this.Player.Stop();
			this.View.PlayButton.Visibility = System.Windows.Visibility.Visible;
			this.View.StopButton.Visibility = System.Windows.Visibility.Hidden;
			this.PlayCommand.OnCanExecuteChanged();
			#if DEBUG
			DebugConsole.WriteLine("Player.CanPlay():" + this.Player.CanPlay().ToString());
			DebugConsole.WriteLine("Player.CanStop():" + this.Player.CanStop().ToString());
			#endif
		}
		private void OnRewindButtonClicked()
		{
			this.Player.Rewind();
		}
		private void OnSelectedDevice(object sender, EventArgs e)
		{
			KeyValuePair<int, string> item = (KeyValuePair<int, string>)this.View.Devices.SelectedItem;
			this.Player.SetDevice(item.Key);
			if(this.Player.OutputDevice.PlaybackState == NAudio.Wave.PlaybackState.Playing)
			{
				this.StopCommand.Execute(null);
				this.PlayCommand.Execute(null);
			}
		}
		private void OnClosed(object sender, EventArgs e)
		{
			//Models.Config.Save(this.Setting);
			this.Player.Dispose();
		}
	}
}
