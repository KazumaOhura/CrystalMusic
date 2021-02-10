using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalMusic.ViewModels
{
	public class MainViewModel : Helpers.Observable
	{
		public Views.MainWindow View { get; private set; } = null;

		public void Initialize(Views.MainWindow mainWindow)
		{
			this.View = mainWindow;
		}
	}
}
