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
