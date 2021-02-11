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

		private Helpers.RelayCommand stopCommand;
		public Helpers.RelayCommand StopCommand { get => stopCommand = stopCommand ?? new Helpers.RelayCommand(OnStopButtonClicked, Player.CanStop); }
		private Helpers.RelayCommand playCommand;
		public Helpers.RelayCommand PlayCommand { get => playCommand = playCommand ?? new Helpers.RelayCommand(OnPlayButtonClicked, Player.CanPlay); }

		public void Initialize(Views.MainWindow mainWindow)
		{
			this.View = mainWindow;
		}

		private void OnStopButtonClicked()
		{
			this.Player.Stop();
			//this.View.PlayButton.Visibility = System.Windows.Visibility.Visible;
			this.View.StopButton.Visibility = System.Windows.Visibility.Hidden;
			//this.PlayCommand.OnCanExecuteChanged();
			#if DEBUG
			//DebugConsole.WriteLine("Player.CanPlay():" + this.Player.CanPlay().ToString());
			DebugConsole.WriteLine("Player.CanStop():" + this.Player.CanStop().ToString());
      #endif
     }

		private void OnPlayButtonClicked()
		{
			this.Player.Play();
			this.View.PlayButton.Visibility = System.Windows.Visibility.Hidden;
			//this.View.StopButton.Visibility = System.Windows.Visibility.Visible;
			//this.StopCommand.OnCanExecuteChanged();
			#if DEBUG
			DebugConsole.WriteLine("Player.CanPlay():" + this.Player.CanPlay().ToString());
			//DebugConsole.WriteLine("Player.CanStop():" + this.Player.CanStop().ToString());
			#endif
		}
	}
}
