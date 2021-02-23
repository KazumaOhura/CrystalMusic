using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalMusic.Models
{
	public class Setting
	{
		private string folder = @"C:\\Music";
		public string Folder { get => this.folder; set=>this.folder = value; }
		private List<string> musics = new List<string>();
		public List<string> Musics { get => this.musics; set=>this.musics = value; }
	}
}
