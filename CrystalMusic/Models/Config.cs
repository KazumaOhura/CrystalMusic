using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Debug;

namespace CrystalMusic.Models
{
	static public class Config
	{
		static private readonly string _fileName = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CrystalMusic\user.config";
		static private readonly string _dirName = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CrystalMusic";
		/// <summary>
		/// Configを保存する
		/// </summary>
		/// <param name="setting">保存する設定</param>
		static public void Save(Setting setting)
		{
			try
			{
				if (!System.IO.Directory.Exists(_dirName)) System.IO.Directory.CreateDirectory(_dirName);
				System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Setting));
				System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(_fileName, false, new UTF8Encoding(false));
				serializer.Serialize(streamWriter, setting);
				streamWriter.Close();
			}
			catch (Exception e)
			{
				DebugConsole.WriteLine("Config(Save):" + e.Message);
				throw;
			}
		}
		/// <summary>
		/// Configを読み込む
		/// </summary>
		/// <returns>読み込まれた設定</returns>
		static public Setting Read()
		{
			try
			{
				if (System.IO.Directory.Exists(_dirName))
				{
					System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Setting));
					System.IO.StreamReader streamReader = new System.IO.StreamReader(_fileName, new UTF8Encoding(false));
					Setting retVal = (Setting)serializer.Deserialize(streamReader);
					streamReader.Close();
					return retVal;
				}
				else return new Setting();
			}
			catch (Exception e)
			{
				DebugConsole.WriteLine("Config(read):" + e.Message);
				throw;
			}
		}
	}
}
