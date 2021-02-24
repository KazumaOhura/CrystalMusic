using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;

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
				DataContractSerializer serializer = new DataContractSerializer(typeof(Setting));
				XmlWriterSettings xmlSettings = new XmlWriterSettings
				{
					Encoding = new UTF8Encoding(false)
				};
				XmlWriter xmlWariter = XmlWriter.Create(_fileName, xmlSettings);
				serializer.WriteObject(xmlWariter, setting);
				xmlWariter.Close();
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
				if (Directory.Exists(_dirName) && File.Exists(_fileName))
				{
					DataContractSerializer serializer = new DataContractSerializer(typeof(Setting));
					XmlReader xmlReader = XmlReader.Create(_fileName);
					Setting retVal = (Setting)serializer.ReadObject(xmlReader);
					xmlReader.Close();
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
