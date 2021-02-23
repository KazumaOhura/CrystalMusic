using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Debug;

namespace CrystalMusic.Models
{
	static public class Config
	{
		static private readonly string _fileName = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CrystalMusic\user.config";
		static private readonly string _dirName = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CrystalMusic";
		static public void Save(Setting setting)
		{
			try
			{
				if (!System.IO.Directory.Exists(_dirName)) System.IO.Directory.CreateDirectory(_dirName);
				System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Setting));
				System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(_fileName, false, new UTF8Encoding(false));
				serializer.Serialize(streamWriter, setting);
				streamWriter.Close();
			}catch(Exception e){
				DebugConsole.WriteLine("Config(Save):" + e.Message);
				throw;
			}
		}
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
				}else return new Setting();
			}catch (Exception e)
			{
				DebugConsole.WriteLine("Config(read):" + e.Message);
				throw;
			}
		}
	}
}
