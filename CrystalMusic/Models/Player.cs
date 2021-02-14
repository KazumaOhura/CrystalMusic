using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NAudio.Wave;

using Debug;

namespace CrystalMusic.Models
{
	public class Player : IDisposable
	{
		private AudioFileReader audioFileReader;
		public AudioFileReader AudioFileReader { get => this.audioFileReader; set => this.audioFileReader = value; }
		private WaveOutEvent outputDevice;
		public WaveOutEvent OutputDevice { get => this.outputDevice; set => this.outputDevice = value; }
		private string playFileName = null;
		public string PlayFileName { get => this.playFileName; set => this.playFileName = value; }
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

		private bool isClosing = false;
		private bool disposedValue;

		public bool IsClosing { get => this.isClosing; set => this.isClosing = value; }

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
				if (this.AudioFileReader == null && this.PlayFileName != null) this.AudioFileReader = new AudioFileReader(this.PlayFileName);
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
