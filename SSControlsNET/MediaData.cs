using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SSControlsNET
{
	public class MediaData
	{
		public int SEQ_NO { get; set; }

		public int USE_YN { get; set; }

		public string USE_YN_DISP
		{
			get
			{
				string useYnDisp = "미사용";
				if (this.USE_YN == 1)
					useYnDisp = "사용";
				return useYnDisp;
			}
		}

		public int SCREEN_NO { get; set; }

		public int STRETCH_GBN { get; set; }

		public Stretch MediaStretch => (Stretch)this.STRETCH_GBN;

		public int IPC_NO { get; set; }

		public string FILE_NM { get; set; }

		public string FILE_EXT { get; set; }

		public 컨텐츠Type CONT_GBN
		{
			get
			{
				컨텐츠Type contGbn = 컨텐츠Type.NONE;
				switch (this.FILE_EXT.ToLower())
				{
					case ".avi":
					case ".mp4":
					case ".mpg":
						contGbn = 컨텐츠Type.MOVIE;
						break;
					case ".bmp":
					case ".jpeg":
					case ".jpg":
					case ".png":
						contGbn = 컨텐츠Type.IMAGE;
						break;
					case ".gif":
						contGbn = 컨텐츠Type.GIF;
						break;
				}
				return contGbn;
			}
		}

		public BitmapImage bIMG { get; set; }

		public string LOCAL_URL { get; set; }

		public string REMOTE_URL { get; set; }

		public DateTime S_DATE { get; set; }

		public string S_DATE_DISP => this.S_DATE.ToString("yyyy-MM-dd HH:mm:ss");

		public DateTime E_DATE { get; set; }

		public string E_DATE_DISP => this.E_DATE.ToString("yyyy-MM-dd HH:mm:ss");

		public int Volume { get; set; }

		public int Speed { get; set; }

		public int Duration { get; set; }

		public double POS_X { get; set; }

		public double POS_Y { get; set; }

		public double SZ_W { get; set; }

		public double SZ_H { get; set; }
	}
}
