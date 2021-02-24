using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;

namespace CrystalMusic.Models
{
	[DataContract]
	public class Setting
	{
		[DataMember]
		private string _folder;
		public string Folder { get => this._folder; set => this._folder = value; }
		[DataMember]
		private Dictionary<string, string> _musics;
		public Dictionary<string, string> Musics { get => this._musics; set => this._musics = value; }

		public Setting()
		{
			Folder = @"C:\Music";
			Musics = new Dictionary<string, string>();
		}

		public Setting(string folder, Dictionary<string, string> musics)
		{
			Folder = folder;
			Musics = musics;
		}
	}
}
