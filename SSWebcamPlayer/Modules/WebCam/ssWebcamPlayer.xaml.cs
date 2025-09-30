using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using OpenCvSharp;
using SSCommon;

namespace SSWebcamPlayer
{
    /// <summary>
    /// CtrProgress.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ssWebcamPlayer : UserControl
    {
        public ssWebcamPlayer()
        {
            InitializeComponent();
        }

        bool _isLoaded = false;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_isLoaded == false)
                {
                   // InitProc();
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
            finally
            {
                _isLoaded = true;
            }
        }
                

        System.ComponentModel.BackgroundWorker _bwApps = null;
        int RotateGBN= 0;
        public void InitProc(int camIdx, int nRotate)
        {
            try
            {
                InitWebCam(camIdx);

                RotateGBN = nRotate;

                if (_bwApps == null)
                {
                    _bwApps = new System.ComponentModel.BackgroundWorker();
                    _bwApps.RunWorkerCompleted += _bwApps_RunWorkerCompleted;
                    _bwApps.WorkerReportsProgress = false; //진행도 보고
                    _bwApps.WorkerSupportsCancellation = true; //취소가능여부
                    _bwApps.ProgressChanged += _bwApps_ProgressChanged;
                    _bwApps.DoWork += _bwApps_DoWork;
                }

                Run();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        public bool Run()
        {
            try
            {
                if (_bwApps != null && IsPlaying == false) _bwApps.RunWorkerAsync(1000);

                return true;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public bool IsPlay()
        {
            try
            {
                return IsPlaying;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }
        public void Stop()
        {
            try
            {
                if (_bwApps != null) _bwApps.CancelAsync();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }



        bool IsPlaying = false;
        /// <summary>
        /// 작업 쓰레드
        /// </summary>
        private void _bwApps_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;
                if (worker == null) return;
                //e.Argument 매개변수

                while (true)
                {
                    //CancelAsync 호출시 정지
                    if (worker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        IsPlaying = false;
                        break;
                    }

                    IsPlaying = true;

                    OpenCvSharp.Mat frame = new OpenCvSharp.Mat();
                    OpenCvSharp.Mat cutfreme = new OpenCvSharp.Mat();
                    if (recodeYN == true)
                    {
                        if (_vcap.Read(frame) == true)
                        {
                            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate ()
                            {
                                if (cvVWriter != null) cvVWriter.Write(frame);

                                FlipMode mode = FlipMode.Y;
                                switch (RotateGBN)
                                {
                                    case 1: mode = FlipMode.XY; break;
                                    case 2: mode = FlipMode.X; break;
                                    case 3: mode = FlipMode.XY; break;
                                }
                                System.Drawing.Bitmap bm = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame.Flip(mode));
                                if (bm != null)
                                {
                                    img.Source = ImageUtils.ConvertBitmapToBitmapSource(bm);
                                }

                                //if (배경모드YN == false)
                                //{
                                //    System.Drawing.Bitmap bm = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame.Flip(OpenCvSharp.FlipMode.Y));
                                //    if (bm != null)
                                //    {
                                //        //        cvVWriter.Write(frame);
                                //        img.Source = ImageUtils.ConvertBitmapToBitmapSource(bm);
                                //    }
                                //}
                            });

                        }
                    }
                    else
                    {
                        if (_vcap.Read(frame) == true)
                        {
                            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate ()
                            {
                                FlipMode mode = FlipMode.Y;
                                switch (RotateGBN)
                                {
                                    case 1: mode = FlipMode.XY; break;
                                    case 2: mode = FlipMode.X; break;
                                    case 3: mode = FlipMode.XY; break;
                                }
                                System.Drawing.Bitmap bm = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame.Flip(mode));
                                if (bm != null) img.Source = ImageUtils.ConvertBitmapToBitmapSource(bm);


                                //System.Drawing.Bitmap bm = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame.Flip(OpenCvSharp.FlipMode.Y));
                                //if (bm != null)
                                //{
                                //    BitmapSource bs = ImageUtils.ConvertBitmapToBitmapSource(bm);
                                //    if (RotateGBN > 0)
                                //    {
                                //        int nRotate = 0;
                                //        switch (RotateGBN)
                                //        {
                                //            case 1: nRotate = 90; break;
                                //            case 2: nRotate = 180; break;
                                //            case 3: nRotate = 270; break;
                                //        }

                                //        TransformedBitmap tb = new TransformedBitmap();
                                //        tb.BeginInit();
                                //        tb.Source = bs;

                                //        RotateTransform ts = new RotateTransform(nRotate);
                                //        tb.Transform = ts;
                                //        tb.EndInit();

                                //        img.Source = tb;
                                //    }
                                //    else
                                //    {
                                //        img.Source = bs;
                                //    }
                                //}

                                //if (bm != null) img.Source = ImageUtils.ConvertBitmapToBitmapSource(bm);
                                //}
                            });

                        }
                        //worker.ReportProgress()
                        //e.Result = 
                        // this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate ()
                        //{
                        //    img.Source = OpenCvSharp.Extensions.BitmapSourceConverter.ToBitmapSource(frame);
                        //});
                    }
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        //private void _bwApps_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;
        //        if (worker == null) return;
        //        //e.Argument 매개변수

        //        while (true)
        //        {
        //            //CancelAsync 호출시 정지
        //            if (worker.CancellationPending == true)
        //            {
        //                e.Cancel = true;
        //                IsPlaying = false;
        //                break;
        //            }

        //            IsPlaying = true;

        //            OpenCvSharp.Mat frame = new OpenCvSharp.Mat();
        //            if (_vcap.Read(frame) == true)
        //            {
        //                if (UseFaceDetect == true && faceCascade != null)
        //                {
        //                    OpenCvSharp.Rect[] faces = faceCascade.DetectMultiScale(frame);
        //                    if (faces.Length > 0)
        //                    {
        //                        foreach (var item in faces)
        //                        {
        //                            OpenCvSharp.Cv2.Rectangle(frame, item, OpenCvSharp.Scalar.Yellow, 2); // add rectangle to the image

        //                            //Console.WriteLine("faces : " + item);
        //                        }
        //                    }
        //                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate ()
        //                    {
        //                        System.Drawing.Bitmap bm = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame.Flip(OpenCvSharp.FlipMode.Y));
        //                        if (bm != null) img.Source = ImageUtils.GetBitmapSourceByBitmap(bm);
        //                    });
        //                }
        //                else
        //                {
        //                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate ()
        //                    {
        //                        System.Drawing.Bitmap bm = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame.Flip(OpenCvSharp.FlipMode.Y));
        //                        if (bm != null) img.Source = ImageUtils.GetBitmapSourceByBitmap(bm);
        //                    });
        //                }

        //                //worker.ReportProgress()
        //                //e.Result = 
        //                // this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate ()
        //                //{
        //                //    img.Source = OpenCvSharp.Extensions.BitmapSourceConverter.ToBitmapSource(frame);
        //                //});
        //            }
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //    }
        //}

        /// <summary>
        /// 작업 진행도
        /// </summary>
        private void _bwApps_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            try
            {
                Console.WriteLine("BackgroundWorkers 진행도 {0}", e.ProgressPercentage);
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        /// <summary>
        /// 작업 완료
        /// </summary>
        private void _bwApps_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    Console.WriteLine("### 오류 : {0} ###", e.Error.Message);
                    return;
                }
                //lblMsg.Text = "성공적으로 완료되었습니다";
                //_bwApps.RunWorkerAsync();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        OpenCvSharp.VideoCapture _vcap;
        WriteableBitmap _wb;        

        private void InitWebCam(int camaraIdx)
        {
            try
            {
                _vcap = OpenCvSharp.VideoCapture.FromCamera(camaraIdx, OpenCvSharp.VideoCaptureAPIs.DSHOW);
                _vcap.FrameWidth = 640;
                _vcap.FrameHeight = 480;
                _wb = new WriteableBitmap(_vcap.FrameWidth, _vcap.FrameHeight, 96, 96, PixelFormats.Rgb24, null);
                img.Source = _wb;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        public void DoFinal()
        {
            try
            {
                if (_bwApps != null)
                {
                    _bwApps.CancelAsync();
                    _bwApps.Dispose();
                    _bwApps = null;
                }

                if (_wb != null)
                {
                    _wb.Freeze();
                    _wb = null;
                }

                if (_vcap != null)
                {
                    _vcap.Dispose();
                    _vcap = null;
                }

                GC.Collect();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DoFinal();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        public ImageSource GetSnapshot()
        {
            try
            {
                try
                {
                    CommonUtils.WaitTime(500, true);
                }
                catch (Exception ex)
                {
                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                }
                //if (UseFaceDetect == true) UseFaceDetect = false;
                if (img.Source != null)
                {
                    return img.Source.Clone();
                }
                //if (배경모드YN == true)
                //{
                //    RenderTargetBitmap bitmap =
                //        new RenderTargetBitmap(Convert.ToInt32(mainGrid.ActualWidth), Convert.ToInt32(mainGrid.ActualHeight),
                //        96, 96, System.Windows.Media.PixelFormats.Pbgra32);
                //    bitmap.Render(mainGrid);

                //    return bitmap;

                //}
                //else
                //{
                //    if (img.Source != null)
                //    {
                //        return img.Source.Clone();
                //    }
                //}
                return null;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return null;
            }
        }

        //bool 배경모드YN = false;
        //public void Set배경화면(BitmapImage bi, int x, int y, int sz_w, int sz_h)
        //{
        //    try
        //    {
        //        if (bi == null) return;

        //        배경모드YN = true;

        //        img.HorizontalAlignment = HorizontalAlignment.Left;
        //        img.VerticalAlignment = VerticalAlignment.Top;
        //        img.Margin = new Thickness(x, y, 0, 0);
        //        img.Width = sz_w;
        //        img.Height = sz_h;
        //        img.Stretch = Stretch.Fill;

        //        img오버랩.Source = bi.Clone();
        //        img오버랩.Stretch = Stretch.Fill;
        //        img오버랩.Width = this.ActualWidth;
        //        img오버랩.Height = this.ActualHeight;

        //        Console.WriteLine("img오버랩 = {0}x{1}", img오버랩.Width, img오버랩.Height);
        //        Console.WriteLine("img = {0}x{1}", img.Width, img.Height);
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //    }
        //}
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {

            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        bool recodeYN = false;
        VideoWriter cvVWriter = null;
        public void StartRecod(string fileName)
		{
            try
            {
                if (recodeYN == true) return;

                recodeYN = true;
                
                //cvVWriter = new VideoWriter(fileName, FourCC.FromString("X264"), 15, new OpenCvSharp.Size(640, 480));
                cvVWriter = new VideoWriter(fileName, FourCC.FromString("X264"), 20, new OpenCvSharp.Size(640, 480));
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
		}

        public void StopRecod()
        {
            try
			{
                recodeYN = false;
                if (cvVWriter != null)
                {
                    cvVWriter.Release();
                    cvVWriter.Dispose();
                    cvVWriter = null;
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }
    }

    //public System.Drawing.Image CutFaceDetect(System.Drawing.Image img사진)
    //{
    //    try
    //    {
    //        if (faceCascade == null)
    //        {
    //            Console.WriteLine("faceCascade IS NULL");
    //            if (InitFaceDetect() == false)
    //            {
    //                Console.WriteLine("InitFaceDetect IS NULL");
    //                return null;
    //            }
    //        }

    //        if (img사진 == null)
    //        {
    //            Console.WriteLine("img사진 IS NULL {0}x{1}", img사진.Width, img사진.Height);
    //        }


    //        OpenCvSharp.Mat frame = new OpenCvSharp.Mat();
    //        System.Drawing.Bitmap bit = new System.Drawing.Bitmap(img사진, img사진.Width, img사진.Height);
    //        frame = OpenCvSharp.Extensions.BitmapConverter.ToMat(bit);
    //        OpenCvSharp.Rect[] faces = faceCascade.DetectMultiScale(frame);
    //        Console.WriteLine("faces Count {0}", faces.Length);
    //        switch (faces.Length)
    //        {
    //            case 0:
    //                break;

    //            case 1:
    //                OpenCvSharp.Mat cutfreme = frame.SubMat(faces[0]);
    //                if (cutfreme != null)
    //                {
    //                    return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(cutfreme);
    //                }
    //                break;

    //            default:
    //                List<OpenCvSharp.Rect> items = faces.OrderByDescending(data => data.Width * data.Height).ToList();
    //                if (items != null && items.Count > 0)
    //                {
    //                    OpenCvSharp.Mat fremeCut = frame.SubMat(items.First());
    //                    if (fremeCut != null)
    //                    {
    //                        return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(fremeCut);
    //                    }
    //                }
    //                break;
    //        }
    //        return null;
    //    }
    //    catch (Exception ee)
    //    {
    //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
    //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
    //        return null;
    //    }
    //}
}