using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalMusic.Models
{
	public class Setting : Helpers.Observable
	{
		private string _folder = @"C:\Music";
		public string Folder { get => this._folder; set => Set(ref this._folder, value); }
		private Dictionary<string, string> _musics = new Dictionary<string, string>();
		public Dictionary<string, string> Musics { get => this._musics; set => Set(ref this._musics, value); }
	}
}
