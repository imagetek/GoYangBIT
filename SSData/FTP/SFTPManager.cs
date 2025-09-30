using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSCommonNET;
using Renci.SshNet.Sftp;
using Renci.SshNet;

namespace SSData
{
    public class SFTPManager
    {
        public SFTPManager()
        {
          //  업로드중YN = false;
        }

        SftpClient sftp = null;
        //bool 업로드중YN { get; set; }
        //public List<string> Select파일목록(string CheckDIR = "")
        //{
        //    SftpClient sftp = new SftpClient(IP, PORT, USER_ID, USER_PWD);
        //    try
        //    {
        //        if (sftp == null) return null;

        //        sftp.Connect();
        //        sftp.OperationTimeout = TimeSpan.FromSeconds(3);

        //        var items = sftp.ListDirectory(CheckDIR);
        //        if (items != null)
        //        {

        //        }
        //        sftp.Disconnect();

        //        return null;
        //    }
        //    catch (Exception ee)
        //    {
        //        TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
        //        return null;
        //    }
        //}

        public int 다운로드File(string saveDIR, string saveFileNM, string remotePath, string remoteFileNM)
        {
            var results = 0;
            var config = BITDataManager.BitSystem;

            //20220628 bha 
            var auth = new PasswordAuthenticationMethod(config.FTP_USERID, config.FTP_USERPWD);
            ConnectionInfo conn = new ConnectionInfo(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, auth);
            conn.Encoding = Encoding.GetEncoding(51949);
            //sftp = new SftpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_PASSWD);
            sftp = new SftpClient(conn);
            //sftp.ConnectTimeout = 500;
            sftp.OperationTimeout = TimeSpan.FromMilliseconds(1000);
            
            
            try
            {
                sftp.Connect();
                if (sftp.IsConnected == false) return results;

                string tempDIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "temp_download");
                if (System.IO.Directory.Exists(tempDIR) == false) System.IO.Directory.CreateDirectory(tempDIR);

                Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[SFTP] {0}:{1}에 접속했습니다. {0}", config.FTP_IP, config.FTP_PORT));
                try
                {
                    Console.WriteLine(string.Format("[SFTP] 서버에 접속했습니다."));
                    string tempFile = System.IO.Path.Combine(tempDIR, saveFileNM);
                    //var tasks = new List<Task>();
                    string remoteFile = System.IO.Path.Combine(remotePath, remoteFileNM);
                    using (var saveFile = System.IO.File.OpenWrite(tempFile))
                    {
                        try
                        {
                            sftp.DownloadFile(remoteFile, saveFile);
                        }
                        catch (System.IO.IOException ex)
                        {
                            TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                        }
                    }
                    //}

                    //tasks.Add(Task.Run(() =>
                    //{
                        
                    //}));

                    CommonUtils.WaitTime(50, true);
                    //await Task.WhenAll(tasks);

                    if (System.IO.File.Exists(tempFile) == true)
                    {
                        if (System.IO.Directory.Exists(saveDIR) == false) System.IO.Directory.CreateDirectory(saveDIR);
                        try
                        {
                            System.IO.File.Move(
                                tempFile,
                                System.IO.Path.Combine(saveDIR, saveFileNM));
                            results = 1;
                        }
                        catch (System.IO.IOException ex)
                        {
                            TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                            results = 0;
                        }
                    }
                    else
                    {
                        results = 2;
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
                    if (sftp != null && sftp.IsConnected == true)
                    {
                        sftp.Disconnect();
                        sftp = null;
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

        public async Task<int> 다운로드FileAsync(string saveDIR, string saveFileNM, string remotePath, string remoteFileNM)
        {
            var results = 0;
            var config = BITDataManager.BitSystem;
            //sftp = new SftpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_PASSWD);
            //sftp.ConnectTimeout = 500
            //20220628 bha 
            var auth = new PasswordAuthenticationMethod(config.FTP_USERID, config.FTP_USERPWD);
            ConnectionInfo conn = new ConnectionInfo(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, auth);
            conn.Encoding = Encoding.GetEncoding(51949); // Encoding.GetEncoding(51949);
            //sftp = new SftpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_PASSWD);
            sftp = new SftpClient(conn); ;
            sftp.OperationTimeout = TimeSpan.FromMilliseconds(1000);            
            try
            {
                sftp.Connect();
                if (sftp.IsConnected == false) return results;

                string tempDIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "temp_download");
                if (System.IO.Directory.Exists(tempDIR) == false) System.IO.Directory.CreateDirectory(tempDIR);

                Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[SFTP] {0}:{1}에 접속했습니다. {0}", config.FTP_IP, config.FTP_PORT));
                try
                {
                    Console.WriteLine(string.Format("[SFTP] 서버에 접속했습니다."));
                    string tempFile = System.IO.Path.Combine(tempDIR, saveFileNM);
                    var tasks = new List<Task>();
                    string remoteFile = System.IO.Path.Combine(remotePath, remoteFileNM);
                    if (sftp.Exists(remoteFile) == true)
                    {
                        tasks.Add(Task.Run(() =>
                        {
                            using (var saveFile = System.IO.File.OpenWrite(tempFile))
                            {
                                sftp.DownloadFile(remoteFile, saveFile);
                            }
                        }));
                    }
                    else
					{
                        Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[SFTP] 파일이 존재하지않습니다. {0}", remoteFile));
                    }


                    CommonUtils.WaitTime(50, true);
                    await Task.WhenAll(tasks);

                    if (System.IO.File.Exists(tempFile) == true)
                    {
                        if (System.IO.Directory.Exists(saveDIR) == false) System.IO.Directory.CreateDirectory(saveDIR);
                        try
                        {
                            System.IO.File.Move(
                                tempFile,
                                System.IO.Path.Combine(saveDIR, saveFileNM));
                            results = 1;
                        }
                        catch (System.IO.IOException ex)
                        {
                            TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                            results = 0;
                        }
                    }
                    else
                    {
                        results = 2;
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
                    if (sftp != null && sftp.IsConnected == true)
                    {
                        sftp.Disconnect();
                        sftp = null;
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
            //sftp = new SftpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_PASSWD);
            //sftp.ConnectTimeout = 500;
            //20220628 bha 
            var auth = new PasswordAuthenticationMethod(config.FTP_USERID, config.FTP_USERPWD);
            ConnectionInfo conn = new ConnectionInfo(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, auth);
            conn.Encoding = Encoding.GetEncoding(51949);
            //sftp = new SftpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_PASSWD);
            sftp = new SftpClient(conn);

            sftp.OperationTimeout = TimeSpan.FromMilliseconds(1000*60);
            try
            {
                sftp.Connect();
                if (sftp.IsConnected == false) return results;

                string tempDIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "temp_download");
                if (System.IO.Directory.Exists(tempDIR) == false) System.IO.Directory.CreateDirectory(tempDIR);

                Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[SFTP] {0}:{1}에 접속했습니다. {0}", config.FTP_IP, config.FTP_PORT));
                try
                {
                    List<string> tempFiles = new List<string>();
                    var tasks = new List<Task>();
                    foreach (NoticeData item in items)
                    {
                        string tempFile = System.IO.Path.Combine(tempDIR, item.FILE_NM);
                        string remoteFile = System.IO.Path.Combine(remotePath, item.FILE_NM);
                        bool isErrorYN = false;
                        tasks.Add(Task.Run(() =>
                        {
                            using (var saveFile = System.IO.File.OpenWrite(tempFile))
                            {
                                try
                                {
                                    sftp.DownloadFile(remoteFile, saveFile);
                                }
                                catch (Exception e)
                                {
                                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", e.StackTrace, e.Message));
                                    isErrorYN = true;
                                }
                            }
                        }));

                        if (isErrorYN == false) tempFiles.Add(tempFile);
                        //));
                        //(tempFile, remoteFile));


                    }
                    CommonUtils.WaitTime(50, true);
                    await Task.WhenAll(tasks);
                    CommonUtils.WaitTime(50, true);

                    foreach (NoticeData item in items)
                    {
                        string tempFile = System.IO.Path.Combine(tempDIR, item.FILE_NM);
                        if (System.IO.File.Exists(tempFile) == true)
                        {
                            if (System.IO.Directory.Exists(saveDIR) == false) System.IO.Directory.CreateDirectory(saveDIR);
                            try
                            {
                                System.IO.File.Move(tempFile,System.IO.Path.Combine(saveDIR, item.FILE_NM));
                                results.Add(item);
                            }
                            catch (System.IO.IOException ex)
                            {
                                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                            }
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
                    if (sftp != null && sftp.IsConnected == true)
                    {
                        sftp.Disconnect();
                        sftp = null;
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

        public List<NoticeData> 다운로드Files(string saveDIR, List<NoticeData> items, string remotePath)
        {
            List<NoticeData> results = new List<NoticeData>();
            var config = BITDataManager.BitSystem;
            //sftp = new SftpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_PASSWD);
            //sftp.ConnectTimeout = 500;
            //20220628 bha 
            var auth = new PasswordAuthenticationMethod(config.FTP_USERID, config.FTP_USERPWD);
            ConnectionInfo conn = new ConnectionInfo(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, auth);
            conn.Encoding = Encoding.GetEncoding(51949);
            //sftp = new SftpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_PASSWD);
            sftp = new SftpClient(conn);

            sftp.OperationTimeout = TimeSpan.FromMilliseconds(1000 * 60);
            try
            {
                sftp.Connect();
                if (sftp.IsConnected == false) return results;

                string tempDIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "temp_download");
                if (System.IO.Directory.Exists(tempDIR) == false) System.IO.Directory.CreateDirectory(tempDIR);

                Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[SFTP] {0}:{1}에 접속했습니다. {0}", config.FTP_IP, config.FTP_PORT));
                try
                {
                    List<string> tempFiles = new List<string>();
                    //var tasks = new List<Task>();
                    foreach (NoticeData item in items)
                    {
                        string tempFile = System.IO.Path.Combine(tempDIR, item.FILE_NM);
                        string remoteFile = System.IO.Path.Combine(remotePath, item.FILE_NM);
                        bool isErrorYN = false;
                        using (var saveFile = System.IO.File.OpenWrite(tempFile))
                        {
                            try
                            {
                                sftp.DownloadFile(remoteFile, saveFile);
                            }
                            catch (Exception e)
                            {
                                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", e.StackTrace, e.Message));
                                isErrorYN = true;
                            }
                        }
                        if (isErrorYN == false) tempFiles.Add(tempFile);
                    }
                    CommonUtils.WaitTime(50, true);                    

                    foreach (NoticeData item in items)
                    {
                        string tempFile = System.IO.Path.Combine(tempDIR, item.FILE_NM);
                        if (System.IO.File.Exists(tempFile) == true)
                        {
                            if (System.IO.Directory.Exists(saveDIR) == false) System.IO.Directory.CreateDirectory(saveDIR);
                            try
                            {
                                System.IO.File.Move(tempFile, System.IO.Path.Combine(saveDIR, item.FILE_NM));
                                results.Add(item);
                            }
                            catch (System.IO.IOException ex)
                            {
                                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                            }
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
                    if (sftp != null && sftp.IsConnected == true)
                    {
                        sftp.Disconnect();
                        sftp = null;
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

        //Task DownloadFileAsync(string source, string destination)
        //{
        //    return Task.Run(() =>
        //    {
        //        using (var saveFile = System.IO.File.OpenWrite(destination))
        //        {
        //            sftp.DownloadFile(source, saveFile);
        //        }
        //    });
        //}

        //Task UploadFileAsync(string source, string destination)
        //{
        //    return Task.Run(() =>
        //    {
        //        using (var saveFile = System.IO.File.OpenWrite(source))
        //        {
        //            sftp.UploadFile(saveFile, destination);
        //        }
        //    });
        //}

        public async Task<int> 업로드FileAsync(string localNM, string remotePath, string remoteFileNM)
        {
            var results = 0;
            //var config = BITDataManager.BitBasic.FTP;
            var config = BITDataManager.BitSystem;
            //sftp = new SftpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_PASSWD);
            //20220628 bha 
            var auth = new PasswordAuthenticationMethod(config.FTP_USERID, config.FTP_USERPWD);
            ConnectionInfo conn = new ConnectionInfo(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, auth);
            conn.Encoding = Encoding.GetEncoding(51949);
            //sftp = new SftpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_PASSWD);
            sftp = new SftpClient(conn);

            sftp.OperationTimeout = TimeSpan.FromMilliseconds(1000);
            try
            {
                sftp.Connect();
                if (sftp.IsConnected == false) return results;
                
                Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[SFTP] {0}:{1}에 접속했습니다. {0}", config.FTP_IP, config.FTP_PORT));
                try
                {
                    string remotefilename = System.IO.Path.Combine(remotePath, remoteFileNM);
                    var tasks = new List<Task>();
                    tasks.Add(Task.Run(() =>
                            {
                                using (var saveFile = System.IO.File.OpenWrite(localNM))
                                {
                                    sftp.UploadFile(saveFile, remotefilename);
                                }
                            }));
                    //UploadFileAsync(localNM, System.IO.Path.Combine(remotePath, remoteFileNM)));
                    CommonUtils.WaitTime(50, true);
                    await Task.WhenAll(tasks);

                    return 1;
                }
                catch (Exception ee)
                {
                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    results = -1;
                }
                finally
                {
                    if (sftp != null && sftp.IsConnected == true)
                    {
                        sftp.Disconnect();
                        sftp = null;
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

        public int 업로드File(string localNM, string remotePath, string remoteFileNM)
        {
            var results = 0;
            var config = BITDataManager.BitSystem;
            //sftp = new SftpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_PASSWD);
            //20220628 bha 
            var auth = new PasswordAuthenticationMethod(config.FTP_USERID, config.FTP_USERPWD);
            ConnectionInfo conn = new ConnectionInfo(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, auth);
            conn.Encoding = Encoding.GetEncoding(51949);
            //sftp = new SftpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_PASSWD);
            sftp = new SftpClient(conn);

            sftp.OperationTimeout = TimeSpan.FromMilliseconds(1000);
            try
            {
                sftp.Connect();
                if (sftp.IsConnected == false) return results;
                Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[SFTP] {0}:{1}에 접속했습니다.", config.FTP_IP, config.FTP_PORT));
                if (sftp.Exists(remotePath) == false)
                {
                    sftp.CreateDirectory(remotePath);
                    Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[SFTP] 폴더 {0}를 생성합니다.", remotePath));
                }
                
                try
                {
                    string remotefilename = System.IO.Path.Combine(remotePath, remoteFileNM);
                    using (System.IO.Stream fileStream = new System.IO.FileStream(localNM, System.IO.FileMode.Open))
                    {
                        try
                        {
                            sftp.UploadFile(fileStream, remotefilename);
                        }
                        catch (System.IO.IOException ex)
                        {
                            TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                        }
                    }
                    //sftp.UploadFile
                    //var tasks = new List<Task>();
                    //tasks.Add(Task.Run(() =>
                    //{
                    //    using (var saveFile = System.IO.File.OpenWrite(localNM))
                    //    {
                    //        sftp.UploadFile(saveFile, remotefilename);
                    //    }
                    //}));
                    //UploadFileAsync(localNM, System.IO.Path.Combine(remotePath, remoteFileNM)));
                    //CommonUtils.WaitTime(50, true);
                    //await Task.WhenAll(tasks);

                    return 1;
                }
                catch (Exception ee)
                {
                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    results = -1;
                }
                finally
                {
                    if (sftp != null && sftp.IsConnected == true)
                    {
                        sftp.Disconnect();
                        sftp = null;
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

        public async Task<int> Agent다운로드FileAsync(BIT_SYSTEM config, string saveDIR, string saveFileNM, string remotePath, string remoteFileNM)
        {
            var results = 0;
            var auth = new PasswordAuthenticationMethod(config.FTP_USERID, config.FTP_USERPWD);
            ConnectionInfo conn = new ConnectionInfo(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, auth);
            conn.Encoding = Encoding.GetEncoding(51949); // Encoding.GetEncoding(51949);
            //sftp = new SftpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_PASSWD);
            sftp = new SftpClient(conn); ;
            sftp.OperationTimeout = TimeSpan.FromMilliseconds(1000);
            try
            {
                sftp.Connect();
                if (sftp.IsConnected == false) return results;

                string tempDIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "temp_download");
                if (System.IO.Directory.Exists(tempDIR) == false) System.IO.Directory.CreateDirectory(tempDIR);

                Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[SFTP] {0}:{1}에 접속했습니다. {0}", config.FTP_IP, config.FTP_PORT));
                try
                {
                    Console.WriteLine(string.Format("[SFTP] 서버에 접속했습니다."));
                    string tempFile = System.IO.Path.Combine(tempDIR, saveFileNM);
                    var tasks = new List<Task>();
                    string remoteFile = System.IO.Path.Combine(remotePath, remoteFileNM);
                    if (sftp.Exists(remoteFile) == true)
                    {
                        tasks.Add(Task.Run(() =>
                        {
                            using (var saveFile = System.IO.File.OpenWrite(tempFile))
                            {
                                sftp.DownloadFile(remoteFile, saveFile);
                            }
                        }));
                    }
                    else
                    {
                        Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[SFTP] 파일이 존재하지않습니다. {0}", remoteFile));
                    }


                    CommonUtils.WaitTime(50, true);
                    await Task.WhenAll(tasks);

                    if (System.IO.File.Exists(tempFile) == true)
                    {
                        if (System.IO.Directory.Exists(saveDIR) == false) System.IO.Directory.CreateDirectory(saveDIR);
                        try
                        {
                            System.IO.File.Move(
                                tempFile,
                                System.IO.Path.Combine(saveDIR, saveFileNM));
                            results = 1;
                        }
                        catch (System.IO.IOException ex)
                        {
                            TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                            results = 0;
                        }
                    }
                    else
                    {
                        results = 2;
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
                    if (sftp != null && sftp.IsConnected == true)
                    {
                        sftp.Disconnect();
                        sftp = null;
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

        public List<FileData> Agent업데이트Files(BIT_SYSTEM config, VersionData items)
        {
            List<FileData> results = new List<FileData>();
            
            var auth = new PasswordAuthenticationMethod(config.FTP_USERID, config.FTP_USERPWD);
            ConnectionInfo conn = new ConnectionInfo(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, auth);
            conn.Encoding = Encoding.GetEncoding(51949);
            sftp = new SftpClient(conn);

            sftp.OperationTimeout = TimeSpan.FromMilliseconds(1000 * 60);
            try
            {
                sftp.Connect();
                if (sftp.IsConnected == false) return results;

                string tempDIR = System.IO.Path.Combine(AppConfig.APPSStartPath, "temp_download");
                if (System.IO.Directory.Exists(tempDIR) == false) System.IO.Directory.CreateDirectory(tempDIR);

                Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[SFTP] {0}:{1}에 접속했습니다. {0}", config.FTP_IP, config.FTP_PORT));
                try
                {
                    List<string> tempFiles = new List<string>();
                    //var tasks = new List<Task>();
                    foreach (FileData item in items.FILES)
                    {
                        string tempFile = System.IO.Path.Combine(tempDIR, item.FILE_NM);
                        bool isErrorYN = false;
                        using (var saveFile = System.IO.File.OpenWrite(tempFile))
                        {
                            try
                            {
                                sftp.DownloadFile(item.REMOTE_URL, saveFile);
                            }
                            catch (Exception e)
                            {
                                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", e.StackTrace, e.Message));
                                isErrorYN = true;
                            }
                        }
                        if (isErrorYN == false) tempFiles.Add(tempFile);
                    }
                    CommonUtils.WaitTime(50, true);

                    foreach (FileData item in items.FILES)
                    {
                        if (item.FILE_EXT.Equals(".zip") == true) //압축파일
                        {
                            System.IO.FileInfo fi = new System.IO.FileInfo(System.IO.Path.Combine(tempDIR, item.FILE_NM));
                            if (fi.Length.Equals(item.FILE_SZ) == false)
                            {
                                TraceManager.AddLog("파일용량 미일치 다운로드 오류");
                            }
                            else
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

                                    TraceManager.AddLog("다운받은 파일 압축해제완료");
                                }
                                catch (System.IO.IOException ex)
                                {
                                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                                }
                            }
                            fi = null;
                        }
                        else
                        {
                            System.IO.FileInfo fi = new System.IO.FileInfo(System.IO.Path.Combine(tempDIR, item.FILE_NM));
                            if (fi.Length.Equals(item.FILE_SZ) == false)
							{
                                TraceManager.AddLog("파일용량 미일치 다운로드 오류");
                            }
                            else
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
                    if (sftp != null && sftp.IsConnected == true)
                    {
                        sftp.Disconnect();
                        sftp = null;
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


        public int Agent업로드File(BIT_SYSTEM config, string localNM, string remotePath, string remoteFileNM)
        {
            var results = 0;
            //sftp = new SftpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_PASSWD);
            //20220628 bha 
            var auth = new PasswordAuthenticationMethod(config.FTP_USERID, config.FTP_USERPWD);
            ConnectionInfo conn = new ConnectionInfo(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, auth);
            conn.Encoding = Encoding.GetEncoding(51949);
            //sftp = new SftpClient(config.FTP_IP, config.FTP_PORT, config.FTP_USERID, config.FTP_PASSWD);
            sftp = new SftpClient(conn);

            sftp.OperationTimeout = TimeSpan.FromMilliseconds(1000);
            try
            {
                sftp.Connect();
                if (sftp.IsConnected == false) return results;
                Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[SFTP] {0}:{1}에 접속했습니다.", config.FTP_IP, config.FTP_PORT));
                if (sftp.Exists(remotePath) == false)
                {
                    sftp.CreateDirectory(remotePath);
                    Log4Manager.WriteFTPLog(Log4Level.Info, string.Format("[SFTP] 폴더 {0}를 생성합니다.", remotePath));
                }

                try
                {
                    string remotefilename = System.IO.Path.Combine(remotePath, remoteFileNM);
                    using (System.IO.Stream fileStream = new System.IO.FileStream(localNM, System.IO.FileMode.Open))
                    {
                        try
                        {
                            sftp.UploadFile(fileStream, remotefilename);
                        }
                        catch (System.IO.IOException ex)
                        {
                            TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ex.StackTrace, ex.Message));
                        }
                    }
                    //sftp.UploadFile
                    //var tasks = new List<Task>();
                    //tasks.Add(Task.Run(() =>
                    //{
                    //    using (var saveFile = System.IO.File.OpenWrite(localNM))
                    //    {
                    //        sftp.UploadFile(saveFile, remotefilename);
                    //    }
                    //}));
                    //UploadFileAsync(localNM, System.IO.Path.Combine(remotePath, remoteFileNM)));
                    //CommonUtils.WaitTime(50, true);
                    //await Task.WhenAll(tasks);

                    return 1;
                }
                catch (Exception ee)
                {
                    TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                    results = -1;
                }
                finally
                {
                    if (sftp != null && sftp.IsConnected == true)
                    {
                        sftp.Disconnect();
                        sftp = null;
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
    }
}
