using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using SSCommonNET;

namespace SSData
{
    public class FTPManager
    {
        public FTPManager()
        {

        }

        public bool FTP업로드(string local파일, string remote파일, string UploadDIR = "")
        {
            try
            {
                var config = BITDataManager.BitSystem;
                FluentFTP.FtpConfig ftpConfig = new FluentFTP.FtpConfig();
                ftpConfig.ConnectTimeout = 5000;
                FluentFTP.FtpClient ftp = new FluentFTP.FtpClient(config.FTP_IP, config.FTP_USERID, config.FTP_USERPWD, config.FTP_PORT, ftpConfig);
                try
                {
                    ftp.Connect();
                    //ftp.ConnectTimeout = 5000;                    
                    
                    Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[FTP] {0}:{1}에 접속했습니다.", config.FTP_IP, config.FTP_PORT));
                    if (UploadDIR.Equals("") == false)
                    {
                        if (ftp.DirectoryExists(UploadDIR) == false)
                        {
                            ftp.CreateDirectory(UploadDIR);
                            Console.WriteLine(string.Format("[FTP] 업로드폴더 ({0})를 생성했습니다. ", UploadDIR));
                            CommonUtils.WaitTime(50, true);
                        }
                        ftp.SetWorkingDirectory(UploadDIR);
                        Console.WriteLine(string.Format("[FTP] 기본폴더를 ({0})로 설정합니다.", UploadDIR));
                    }
                    FluentFTP.FtpStatus uploadS = ftp.UploadFile(local파일, remote파일, FluentFTP.FtpRemoteExists.Skip, true, FluentFTP.FtpVerify.Delete);
                    Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[FTP] 파일을 업로드했습니다. {0}", remote파일));
                    if (uploadS == FluentFTP.FtpStatus.Failed)
                    {
                        //중단??
                        return false;
                    }
                    CommonUtils.WaitTime(50, true);
                }
                catch (Exception ee)
                {
                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    return false;
                }
                finally
                {
                    if (ftp.IsConnected == true) ftp.Disconnect();
                }
                return true;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public bool FTP다운로드(string local파일, string remote파일, string UploadDIR = "")
        {
            try
            {
                var config = BITDataManager.BitSystem;
				
                FluentFTP.FtpConfig ftpConfig = new FluentFTP.FtpConfig();
				ftpConfig.ConnectTimeout = 1000;
				//FluentFTP.FtpClient ftp = new FluentFTP.FtpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_USERPWD);
				FluentFTP.FtpClient ftp = new FluentFTP.FtpClient(config.FTP_IP, config.FTP_USERID, config.FTP_USERPWD, config.FTP_PORT, ftpConfig);
				try
                {
                    ftp.Connect();
                    //ftp.ConnectTimeout = 1000;
                    Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[FTP] {0}:{1}에 접속했습니다.", config.FTP_IP, config.FTP_PORT));
                    if (UploadDIR.Equals("") == false)
                    {
                        if (ftp.DirectoryExists(UploadDIR) == false)
                        {
                            ftp.CreateDirectory(UploadDIR);
                            Console.WriteLine(string.Format("[FTP] 업로드폴더 ({0})를 생성했습니다. ", UploadDIR));
                            CommonUtils.WaitTime(50, true);
                        }
                        ftp.SetWorkingDirectory(UploadDIR);
                        Console.WriteLine(string.Format("[FTP] 기본폴더를 ({0})로 설정합니다.", UploadDIR));
                    }

                    FluentFTP.FtpStatus down상태 = ftp.DownloadFile(local파일, remote파일, FluentFTP.FtpLocalExists.Overwrite, FluentFTP.FtpVerify.None);
                    Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[FTP] 파일을 다운로드했습니다. {0}", remote파일));
                    if (down상태 == FluentFTP.FtpStatus.Failed)
                    {
                        //중단??
                        return false;
                    }
                    CommonUtils.WaitTime(50, true);
                }
                catch (Exception ee)
                {
                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    return false;
                }
                finally
                {
                    if (ftp.IsConnected == true) ftp.Disconnect();
                }
                return true;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

   
        //저장DIR, 저장FILE, "/NOTICE/NEWS/NEWS.txt");
        public async Task<int> 다운로드FileAsync(string saveDIR, string saveFileNM, string remotePath, string remoteFileNM)
        {
            var results = 0;
            var config = BITDataManager.BitSystem;
			FluentFTP.FtpConfig ftpConfig = new FluentFTP.FtpConfig();
			ftpConfig.ConnectTimeout = 1000;
			//FluentFTP.FtpClient ftp = new FluentFTP.FtpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_USERPWD);
			FluentFTP.AsyncFtpClient ftp = new FluentFTP.AsyncFtpClient(config.FTP_IP, config.FTP_USERID, config.FTP_USERPWD, config.FTP_PORT, ftpConfig);
			try
			{
				await ftp.AutoConnect();
				//ftp.ConnectTimeout = 1000;
				if (ftp.IsConnected == false) return results;

                string tempDIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "temp_download");
                if (System.IO.Directory.Exists(tempDIR) == false) System.IO.Directory.CreateDirectory(tempDIR);

                Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[FTP] {0}:{1}에 접속했습니다. {0}", config.FTP_IP, config.FTP_PORT));
                try
                {
					await ftp.SetWorkingDirectory(remotePath);
                    //System.IO.FileInfo fi = new System.IO.FileInfo(orgname);

                    FluentFTP.FtpStatus status = await ftp.DownloadFile(System.IO.Path.Combine(tempDIR, saveFileNM),
                        System.IO.Path.Combine(remotePath, remoteFileNM));

                    CommonUtils.WaitTime(50, true);
                    switch (status)
                    {
                        case FluentFTP.FtpStatus.Failed:
                            try
                            {
                                System.IO.File.Delete(
                                    System.IO.Path.Combine(tempDIR, saveFileNM));
                            }
                            catch (System.IO.IOException ex)
                            {
                                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                            }
                            results = 0;
                            break;

                        case FluentFTP.FtpStatus.Success:
                            if (System.IO.Directory.Exists(saveDIR) == false) System.IO.Directory.CreateDirectory(saveDIR);
                            try
                            {
                                System.IO.File.Move(
                                    System.IO.Path.Combine(tempDIR, saveFileNM),
                                    System.IO.Path.Combine(saveDIR, saveFileNM));
                            }
                            catch (System.IO.IOException ex)
                            {
                                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                            }
                            results = 1;
                            break;

                        case FluentFTP.FtpStatus.Skipped:
                            try
                            {
                                System.IO.File.Delete(
                                    System.IO.Path.Combine(tempDIR, saveFileNM));
                            }
                            catch (System.IO.IOException ex)
                            {
                                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                            }

                            results = 2;
                            break;
                    }
                }
                catch (Exception ee)
                {
                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    results = -1;
                }
                finally
                {
                    if (ftp != null && ftp.IsConnected == true)
                    {
                        await ftp.Disconnect();
                    }
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                results = -9;
            }
            return results;
        }

        public async Task<List<NoticeData>> 다운로드FilesAsync(string saveDIR, List<NoticeData> items, string remotePath)
        {
            List<NoticeData> results = new List<NoticeData>();
            var config = BITDataManager.BitSystem;
			
            FluentFTP.FtpConfig ftpConfig = new FluentFTP.FtpConfig();
			ftpConfig.ConnectTimeout = 1000;

			//FluentFTP.AsyncFtpClient ftp = new FluentFTP.AsyncFtpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_USERPWD);
			FluentFTP.AsyncFtpClient ftp = new FluentFTP.AsyncFtpClient(config.FTP_IP, config.FTP_USERID, config.FTP_USERPWD, config.FTP_PORT, ftpConfig);
			try
            {
                await ftp.Connect();
                if (ftp.IsConnected == false) return results;

                string tempDIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "temp_download");
                if (System.IO.Directory.Exists(tempDIR) == false) System.IO.Directory.CreateDirectory(tempDIR);

                Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[FTP] {0}:{1}에 접속했습니다. {0}", config.FTP_IP, config.FTP_PORT));
                try
                {
                    await ftp.SetWorkingDirectory(remotePath);
                    //System.IO.FileInfo fi = new System.IO.FileInfo(orgname);
                    foreach (NoticeData item in items)
                    {
                        FluentFTP.FtpStatus status = await ftp.DownloadFile(
                            System.IO.Path.Combine(tempDIR, item.FILE_NM),
                            System.IO.Path.Combine(remotePath, item.FILE_NM));

                        CommonUtils.WaitTime(50, true);
                        switch (status)
                        {
                            case FluentFTP.FtpStatus.Failed:
                                try
                                {
                                    System.IO.File.Delete(
                                        System.IO.Path.Combine(tempDIR, item.FILE_NM));
                                }
                                catch (System.IO.IOException ex)
                                {
                                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                                }
                                break;

                            case FluentFTP.FtpStatus.Success:
                                if (System.IO.Directory.Exists(saveDIR) == false) System.IO.Directory.CreateDirectory(saveDIR);
                                try
                                {
                                    System.IO.File.Move(
                                        System.IO.Path.Combine(tempDIR, item.FILE_NM),
                                        System.IO.Path.Combine(saveDIR, item.FILE_NM));
                                }
                                catch (System.IO.IOException ex)
                                {
                                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                                }
                                results.Add(item);
                                break;

                            case FluentFTP.FtpStatus.Skipped:
                                try
                                {
                                    System.IO.File.Delete(
                                        System.IO.Path.Combine(tempDIR, item.FILE_NM));
                                }
                                catch (System.IO.IOException ex)
                                {
                                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                                }
                                break;
                        }
                    }
                }
                catch (Exception ee)
                {
                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    results = null;
                }
                finally
                {
                    if (ftp != null && ftp.IsConnected == true)
                    {
						await ftp.Disconnect();
                    }
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                results = null;
            }
            return results;
        }

        public async Task<int> 업로드FileAsync(string localNM, string remotePath, string remoteFileNM)
        {
            var results = 0;
            var config = BITDataManager.BitSystem;

			FluentFTP.FtpConfig ftpConfig = new FluentFTP.FtpConfig();
			ftpConfig.ConnectTimeout = 1000;

			//FluentFTP.AsyncFtpClient ftp = new FluentFTP.AsyncFtpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_USERPWD);
			FluentFTP.AsyncFtpClient ftp = new FluentFTP.AsyncFtpClient(config.FTP_IP, config.FTP_USERID, config.FTP_USERPWD, config.FTP_PORT, ftpConfig);

			//FluentFTP.FtpClient ftp = new FluentFTP.FtpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_USERPWD);
   //         ftp.ConnectTimeout = 500;
            try
            {
				await ftp.Connect();
                if (ftp.IsConnected == false) return results;

                Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[FTP] {0}:{1}에 접속했습니다. {0}", config.FTP_IP, config.FTP_PORT));
                try
                {
                    Console.WriteLine(string.Format("[FTP] 서버에 접속했습니다."));

                    bool resCreate = await ftp.DirectoryExists(remotePath);

					if (resCreate == false)
                    {
                        await ftp.CreateDirectory(remotePath);
                        await ftp.SetWorkingDirectory(remotePath);
                    }
                    else
                    {
                        await ftp.SetWorkingDirectory(remotePath);
                    }
                    
                    //System.IO.FileInfo fi = new System.IO.FileInfo(orgname);

                    FluentFTP.FtpStatus status = await ftp.UploadFile(
                        localNM,
                        System.IO.Path.Combine(remotePath, remoteFileNM) ,
                        FluentFTP.FtpRemoteExists.Overwrite ,
                        false ,
                        FluentFTP.FtpVerify.None);

                    CommonUtils.WaitTime(50, true);
                    switch (status)
                    {
                        case FluentFTP.FtpStatus.Failed:                            
                            results = 0;
                            break;

                        case FluentFTP.FtpStatus.Success:                            
                            results = 1;
                            break;

                        case FluentFTP.FtpStatus.Skipped:
                            results = 2;
                            break;
                    }
                }
                catch (Exception ee)
                {
                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    results = -1;
                }
                finally
                {
                    if (ftp != null && ftp.IsConnected == true)
                    {
						await ftp.Disconnect();
                    }
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                results = -9;
            }
            return results;
        }

        public bool AgentFTP다운로드(BIT_SYSTEM config, string local파일, string remote파일, string UploadDIR = "")
        {
            try
            {
				FluentFTP.FtpConfig ftpConfig = new FluentFTP.FtpConfig();
				ftpConfig.ConnectTimeout = 1000;
				FluentFTP.FtpClient ftp = new FluentFTP.FtpClient(config.FTP_IP, config.FTP_USERID, config.FTP_USERPWD, config.FTP_PORT, ftpConfig);
				//FluentFTP.FtpClient ftp = new FluentFTP.FtpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_USERPWD);
                try
                {
                    ftp.Connect();
                    //ftp.ConnectTimeout = 5000;
                    Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[FTP] {0}:{1}에 접속했습니다.", config.FTP_IP, config.FTP_PORT));
                    if (UploadDIR.Equals("") == false)
                    {
                        if (ftp.DirectoryExists(UploadDIR) == false)
                        {
                            ftp.CreateDirectory(UploadDIR);
                            Console.WriteLine(string.Format("[FTP] 업로드폴더 ({0})를 생성했습니다. ", UploadDIR));
                            CommonUtils.WaitTime(50, true);
                        }
                        ftp.SetWorkingDirectory(UploadDIR);
                        Console.WriteLine(string.Format("[FTP] 기본폴더를 ({0})로 설정합니다.", UploadDIR));
                    }

                    FluentFTP.FtpStatus down상태 = ftp.DownloadFile(local파일, remote파일, FluentFTP.FtpLocalExists.Overwrite, FluentFTP.FtpVerify.None);
                    Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[FTP] 파일을 다운로드했습니다. {0}", remote파일));
                    if (down상태 == FluentFTP.FtpStatus.Failed)
                    {
                        //중단??
                        return false;
                    }
                    CommonUtils.WaitTime(50, true);
                }
                catch (Exception ee)
                {
                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    return false;
                }
                finally
                {
                    if (ftp.IsConnected == true) ftp.Disconnect();
                }
                return true;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                return false;
            }
        }

        public async Task<int> Agent다운로드FileAsync(BIT_SYSTEM config, string saveDIR, string saveFileNM, string remotePath, string remoteFileNM)
        {
            var results = 0;
			//FluentFTP.FtpClient ftp = new FluentFTP.FtpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_USERPWD);
			//ftp.ConnectTimeout = 500;
			FluentFTP.FtpConfig ftpConfig = new FluentFTP.FtpConfig();
			ftpConfig.ConnectTimeout = 1000;

			//FluentFTP.AsyncFtpClient ftp = new FluentFTP.AsyncFtpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_USERPWD);
			FluentFTP.AsyncFtpClient ftp = new FluentFTP.AsyncFtpClient(config.FTP_IP, config.FTP_USERID, config.FTP_USERPWD, config.FTP_PORT, ftpConfig);

			try
			{
				await ftp.Connect();
                if (ftp.IsConnected == false) return results;

                string tempDIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "temp_download");
                if (System.IO.Directory.Exists(tempDIR) == false) System.IO.Directory.CreateDirectory(tempDIR);

                Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[FTP] {0}:{1}에 접속했습니다. {0}", config.FTP_IP, config.FTP_PORT));
                try
                {
                    await ftp.SetWorkingDirectory(remotePath);
                    //System.IO.FileInfo fi = new System.IO.FileInfo(orgname);

                    FluentFTP.FtpStatus status = await ftp.DownloadFile(
                        System.IO.Path.Combine(tempDIR, saveFileNM),
                        System.IO.Path.Combine(remotePath, remoteFileNM));

                    CommonUtils.WaitTime(50, true);
                    switch (status)
                    {
                        case FluentFTP.FtpStatus.Failed:
                            try
                            {
                                System.IO.File.Delete(
                                    System.IO.Path.Combine(tempDIR, saveFileNM));
                            }
                            catch (System.IO.IOException ex)
                            {
                                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                            }
                            results = 0;
                            break;

                        case FluentFTP.FtpStatus.Success:
                            if (System.IO.Directory.Exists(saveDIR) == false) System.IO.Directory.CreateDirectory(saveDIR);
                            try
                            {
                                System.IO.File.Move(
                                    System.IO.Path.Combine(tempDIR, saveFileNM),
                                    System.IO.Path.Combine(saveDIR, saveFileNM));
                            }
                            catch (System.IO.IOException ex)
                            {
                                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                            }
                            results = 1;
                            break;

                        case FluentFTP.FtpStatus.Skipped:
                            try
                            {
                                System.IO.File.Delete(
                                    System.IO.Path.Combine(tempDIR, saveFileNM));
                            }
                            catch (System.IO.IOException ex)
                            {
                                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                            }

                            results = 2;
                            break;
                    }
                }
                catch (Exception ee)
                {
                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    results = -1;
                }
                finally
                {
                    if (ftp != null && ftp.IsConnected == true)
                    {
						await ftp.Disconnect();
                    }
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                results = -9;
            }
            return results;
        }
        public async Task<int> Agent업로드FileAsync(BIT_SYSTEM config, string localNM, string remotePath, string remoteFileNM)
        {
            var results = 0;

			FluentFTP.FtpConfig ftpConfig = new FluentFTP.FtpConfig();
			ftpConfig.ConnectTimeout = 1000;

			//FluentFTP.AsyncFtpClient ftp = new FluentFTP.AsyncFtpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_USERPWD);
			FluentFTP.AsyncFtpClient ftp = new FluentFTP.AsyncFtpClient(config.FTP_IP, config.FTP_USERID, config.FTP_USERPWD, config.FTP_PORT, ftpConfig);

			///FluentFTP.FtpClient ftp = new FluentFTP.FtpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_USERPWD);
   //         ftp.ConnectTimeout = 500;
            try
            {
				await ftp.Connect();
                if (ftp.IsConnected == false) return results;

                Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[FTP] {0}:{1}에 접속했습니다. {0}", config.FTP_IP, config.FTP_PORT));
                try
                {
                    Console.WriteLine(string.Format("[FTP] 서버에 접속했습니다."));

                    bool resCreate= await ftp.DirectoryExists(remotePath);
                    if (resCreate == false)
                    {
                        await ftp.CreateDirectory(remotePath);
                        await ftp.SetWorkingDirectory(remotePath);
                    }
                    else
                    {
                        await ftp.SetWorkingDirectory(remotePath);
                    }

                    //System.IO.FileInfo fi = new System.IO.FileInfo(orgname);

                    FluentFTP.FtpStatus status = await ftp.UploadFile(
                        localNM,
                        System.IO.Path.Combine(remotePath, remoteFileNM),
                        FluentFTP.FtpRemoteExists.Overwrite,
                        false,
                        FluentFTP.FtpVerify.None);

                    CommonUtils.WaitTime(50, true);
                    switch (status)
                    {
                        case FluentFTP.FtpStatus.Failed:
                            results = 0;
                            break;

                        case FluentFTP.FtpStatus.Success:
                            results = 1;
                            break;

                        case FluentFTP.FtpStatus.Skipped:
                            results = 2;
                            break;
                    }
                }
                catch (Exception ee)
                {
                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    results = -1;
                }
                finally
                {
                    if (ftp != null && ftp.IsConnected == true)
                    {
						await ftp.Disconnect();
                    }
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                results = -9;
            }
            return results;
        }

        public async Task<List<FileData>> Agent업데이트FileAsync(BIT_SYSTEM config, VersionData items)
        {
            List<FileData> results = new List<FileData>();
            //FluentFTP.FtpClient ftp = new FluentFTP.FtpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_USERPWD);
			FluentFTP.FtpConfig ftpConfig = new FluentFTP.FtpConfig();
			ftpConfig.ConnectTimeout = 1000;

			//FluentFTP.AsyncFtpClient ftp = new FluentFTP.AsyncFtpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_USERPWD);
			FluentFTP.AsyncFtpClient ftp = new FluentFTP.AsyncFtpClient(config.FTP_IP, config.FTP_USERID, config.FTP_USERPWD, config.FTP_PORT, ftpConfig);

			//ftp.ConnectTimeout = 500;
            try
            {
				await ftp.Connect();
                if (ftp.IsConnected == false) return results;

                string tempDIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "temp_download");
                if (System.IO.Directory.Exists(tempDIR) == false) System.IO.Directory.CreateDirectory(tempDIR);

                Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[FTP] {0}:{1}에 접속했습니다. {0}", config.FTP_IP, config.FTP_PORT));
                try
                {
                    //await ftp.SetWorkingDirectoryAsync(remotePath);
                    //System.IO.FileInfo fi = new System.IO.FileInfo(orgname);
                    foreach (FileData item in items.FILES)
                    {
                        FluentFTP.FtpStatus status = await ftp.DownloadFile(
                            System.IO.Path.Combine(tempDIR, item.FILE_NM),
                            item.REMOTE_URL);

                        CommonUtils.WaitTime(50, true);
                        switch (status)
                        {
                            case FluentFTP.FtpStatus.Failed:
                                try
                                {
                                    System.IO.File.Delete(
                                        System.IO.Path.Combine(tempDIR, item.FILE_NM));
                                }
                                catch (System.IO.IOException ex)
                                {
                                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                                }
                                break;

                            case FluentFTP.FtpStatus.Success:
                                if (item.FILE_EXT.Equals(".zip") == true)
                                {
                                    System.IO.FileInfo fi = new System.IO.FileInfo(System.IO.Path.Combine(tempDIR, item.FILE_NM));
                                    if (item.FILE_SZ.Equals(fi.Length) == true)
                                    {
                                        try
                                        {
                                            string 저장DIR = System.IO.Path.Combine(AppConfig.APPSStartPath, item.LOCAL_URL);
                                            if (System.IO.Directory.Exists(저장DIR) == false) System.IO.Directory.CreateDirectory(저장DIR);

                                            Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(fi.FullName);
                                            zip.ExtractAll(저장DIR, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
                                            CommonUtils.WaitTime(500, true);
                                            zip.Dispose();
                                            zip = null;

                                            results.Add(item);
                                        }
                                        catch (System.IO.IOException ex)
                                        {
                                            TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                                        }
                                    }
                                    else
                                    {
                                        TraceManager.AddLog(string.Format("파일용량이 미일치합니다."));
                                    }
                                    fi = null;
                                }
                                else
                                {
                                    System.IO.FileInfo fi = new System.IO.FileInfo(System.IO.Path.Combine(tempDIR, item.FILE_NM));
                                    if (item.FILE_SZ.Equals(fi.Length) == true)
                                    {
                                        string 저장DIR = System.IO.Path.Combine(AppConfig.APPSStartPath, item.LOCAL_URL);
                                        if (System.IO.Directory.Exists(저장DIR) == false) System.IO.Directory.CreateDirectory(저장DIR);
                                        try
                                        {
                                            System.IO.File.Move(
                                                System.IO.Path.Combine(tempDIR, item.FILE_NM),
                                                System.IO.Path.Combine(저장DIR, item.FILE_NM));

                                            results.Add(item);
                                        }
                                        catch (System.IO.IOException ex)
                                        {
                                            TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                                        }
                                    }
                                   else
									{
                                        TraceManager.AddLog(string.Format("파일용량이 미일치합니다."));
                                    }
                                    fi = null;
                                }
                                break;

                            case FluentFTP.FtpStatus.Skipped:
                                try
                                {
                                    System.IO.File.Delete(
                                        System.IO.Path.Combine(tempDIR, item.FILE_NM));
                                }
                                catch (System.IO.IOException ex)
                                {
                                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                                }
                                break;
                        }
                    }
                }
                catch (Exception ee)
                {
                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    results = null;
                }
                finally
                {
                    if (ftp != null && ftp.IsConnected == true)
                    {
                        await ftp.Disconnect();
                    }
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                results = null;
            }
            return results;
        }

    }
}
