using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if DEBUG
using Debug;
#endif

namespace CrystalMusic.ViewModels
{
	public class MainViewModel : Helpers.Observable
	{
		public Views.MainWindow View { get; private set; } = null;
		private Models.Player Player { get; set; }
		public string FileName { private get => Player.PlayFileName; set => Player.PlayFileName = value; }
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
		public Dictionary<int, string> Devices { get; private set; }

		private Helpers.RelayCommand playCommand;
		public Helpers.RelayCommand PlayCommand { get => playCommand = playCommand ?? new Helpers.RelayCommand(OnPlayButtonClicked, Player.CanPlay); }
		private Helpers.RelayCommand stopCommand;
		public Helpers.RelayCommand StopCommand { get => stopCommand = stopCommand ?? new Helpers.RelayCommand(OnStopButtonClicked, Player.CanStop); }
		private Helpers.RelayCommand rewindCommand;
		public Helpers.RelayCommand RewindCommand { get => rewindCommand = rewindCommand ?? new Helpers.RelayCommand(OnRewindButtonClicked); }
		private Helpers.RelayCommand selectFileCommand;
		public Helpers.RelayCommand SelectFileCommand { get => selectFileCommand = selectFileCommand ?? new Helpers.RelayCommand(SelectFile); }
		public void Initialize(Views.MainWindow mainWindow)
		{
			this.View = mainWindow;
			this.View.Closed += this.OnClosed;
			this.Player = new Models.Player();
			this.Devices = Player.GetDevices();
		}
		private void SelectFile()
		{
			var dialog = new Microsoft.Win32.OpenFileDialog
			{
				Filter = "MP3(*.mp3)|*.mp3",
				Multiselect = true
			};

			if (dialog.ShowDialog() == true)
			{
				this.FileName = dialog.FileName;
				this.Player.CreateFileReader();
			}
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
		private void OnClosed(object sender, EventArgs e)
		{
			this.Player.Dispose();
		}
	}
}
