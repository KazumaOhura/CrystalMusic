using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

using NAudio.Wave;

using Debug;
using System.Windows.Media.Imaging;

namespace CrystalMusic.Models
{
	public class Player : Helpers.Observable, IDisposable
	{
		private AudioFileReader audioFileReader;
		public AudioFileReader AudioFileReader { get => this.audioFileReader; set => this.audioFileReader = value; }
		private WaveOutEvent outputDevice;
		public WaveOutEvent OutputDevice { get => this.outputDevice; set => this.outputDevice = value; }
		private string playFilePath = null;
		public string PlayFilePath { get => this.playFilePath; set => Set(ref playFilePath, value); }
		private float soundVolume = 0.5f;
		public float SoundVolume
		{
			get
			{
				return this.soundVolume;
			}
			set
			{
				if (this.AudioFileReader != null) this.AudioFileReader.Volume = soundVolume;
				this.soundVolume = value;
			}
		}
		private bool disposedValue;
		private TagLib.File audioFile;
		public TagLib.File AudioFile { get => this.audioFile; set => Set(ref this.audioFile, value); }

		public Player()
		{
			if (this.OutputDevice == null)
			{
				this.OutputDevice = new WaveOutEvent();
				this.OutputDevice.PlaybackStopped += OnPlaybackStopped;
			}
		}
		public void Play()
		{
			try
			{
				if (this.CanPlay())
				{
					this.OutputDevice.Init(this.AudioFileReader);
					this.OutputDevice.Play();
				}
			}
			catch (Exception e)
			{
				DebugConsole.WriteLine("Play():" + e.Message);
				throw;
			}
		}
		public bool CanPlay()
		{
			if (this.OutputDevice.PlaybackState != PlaybackState.Playing && this.AudioFileReader != null) return true;
			else return false;
		}
		public void Stop()
		{
			try
			{
				if (this.CanStop()) this.OutputDevice?.Stop();
			}
			catch (NullReferenceException e)
			{
				DebugConsole.WriteLine("Stop():" + e.Message);
				throw;
			}
		}
		public bool CanStop()
		{
			if (this.OutputDevice.PlaybackState == PlaybackState.Playing) return true;
			return false;
		}
		public void Rewind()
		{
			try
			{
				this.AudioFileReader.Position = 0;
			}
			catch (NullReferenceException)
			{
				DebugConsole.WriteLine("Please select the audio file.");
			}
		}
		public void CreateFileReader()
		{
			try
			{
				if (this.AudioFileReader == null && this.PlayFilePath != null) this.AudioFileReader = new AudioFileReader(this.PlayFilePath);
				GetAudioFile();
				this.SoundVolume = this.SoundVolume;
			}
			catch (Exception e)
			{
				DebugConsole.WriteLine("CreateFileReader():" + e.Message);
				throw;
			}
		}
		public Dictionary<int, string> GetDevices()
		{
			Dictionary<int, string> keyValues = new Dictionary<int, string>();
			for(int i = -1; i < WaveOut.DeviceCount; i++)
			{
				var caps = WaveOut.GetCapabilities(i);
				if (i == -1) keyValues.Add(i, "Default Device");
				else { keyValues.Add(i, caps.ProductName); }
			}
			return keyValues;
		}
		public void SetDevice(int num)
		{
			try
			{
				if (num < -1 || WaveOut.DeviceCount < num) throw new ArgumentOutOfRangeException("Invalid value","無効な値が指定されました");
				this.OutputDevice.DeviceNumber = num;
			}catch(ArgumentOutOfRangeException e){
				DebugConsole.WriteLine("SetDevice():" + e.ParamName);
			}
		}
		public void GetAudioFile()
		{
			this.AudioFile = TagLib.File.Create(this.PlayFilePath);
		}
		public void OnPlaybackStopped(object sender, EventArgs args)
		{
			
		}

		private void Destroy()
		{
			this.audioFileReader?.Dispose();
			this.OutputDevice?.Dispose();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: マネージド状態を破棄します (マネージド オブジェクト)
					this.Destroy();
				}

				// TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
				// TODO: 大きなフィールドを null に設定します
				disposedValue = true;
			}
		}

		// // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
		// ~Player()
		// {
		//     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
		//     Dispose(disposing: false);
		// }

		public void Dispose()
		{
			// このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
