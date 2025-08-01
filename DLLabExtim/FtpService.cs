using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

using CMLabExtim;


namespace DLLabExtim
{

    public class FTPFileData
    {
        public DateTime? DateCreated { get; set; }
        public Int64 Size { get; set; }
        public string Name { get; set; }
    }

    public class FtpService
    {

        private string m_ftpSite;
        private string m_username;
        private string m_password;
        private string m_ftpFolderIn;
        private string m_ftpFolderOut;

        public string InputDir { get { return m_ftpFolderIn; } }

        public string OutputDir { get { return m_ftpFolderOut; } }

        public FtpService(SharedConfiguration sharedConfig)
        {
            m_ftpSite = sharedConfig.FtpBaseUrl;
            m_username = sharedConfig.FtpUsername;
            m_password = sharedConfig.FtpPassword;
            m_ftpFolderIn = sharedConfig.FtpInputPath;
            m_ftpFolderOut = sharedConfig.FtpOutputPath;
        }

        private List<string> FtpGetList(string ftpDirectory)
        {
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(m_ftpSite + @"/" + ftpDirectory);
            request.UseBinary = true;
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(m_username, m_password);

            // aggiunti
            request.Timeout = 20000;
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLeaseTimeout = 20000;
            request.ServicePoint.MaxIdleTime = 20000;


            //FtpWebResponse response = BIWMISFtpWebRequest.GetResponse(request);

            //Stream responseStream = response.GetResponseStream();
            //StreamReader reader = new StreamReader(responseStream);

            //List<string> tmp = new List<string>();
            //try
            //{
            //    string Data = reader.ReadToEnd();
            //    foreach (string item in Regex.Split(Data, "\r\n"))
            //    {
            //        if (!string.IsNullOrEmpty(item))
            //        {
            //            tmp.Add(item);
            //        }
            //    }
            //}
            //catch
            //{
            //}
            //finally
            //{
            //    reader.Close();
            //    response.Close();
            //}

            // parte sostituita
            List<string> tmp = new List<string>();
            try
            {
                //using (var response = (FtpWebResponse)request.GetResponse())
                using (var response = BIWMISFtpWebRequest.GetResponse(request))
                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream))
                {
                    string Data = reader.ReadToEnd();
                    foreach (string item in Regex.Split(Data, "\r\n"))
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            tmp.Add(item);
                        }
                    }
                }
            }
            catch (Exception genericException)
            {
                //throw genericException;
            }
            finally
            {
                request.Abort();
            }

            return tmp;
        }


        private List<FTPFileData> FtpGetTypedList(string ftpDirectory)
        {
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(m_ftpSite + @"/" + ftpDirectory);
            request.UseBinary = true;
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            request.Credentials = new NetworkCredential(m_username, m_password);

            // aggiunti
            request.Timeout = 20000;
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLeaseTimeout = 20000;
            request.ServicePoint.MaxIdleTime = 20000;

            //FtpWebResponse response = BIWMISFtpWebRequest.GetResponse(request);

            //Stream responseStream = response.GetResponseStream();
            //StreamReader reader = new StreamReader(responseStream);

            List<FTPFileData> tmp = new List<FTPFileData>();
            //try
            //{
            //    string Data = reader.ReadToEnd();
            //    foreach (string item in Regex.Split(Data, "\r\n"))
            //    {
            //        if (!string.IsNullOrEmpty(item))
            //        {
            //            var tokens = item.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //            if (tokens.Length > 3)
            //            {
            //                var fileData = new FTPFileData
            //                {
            //                    DateCreated = DateTime.ParseExact(tokens[0] + tokens[1], "MM-dd-yyhh:mmtt", CultureInfo.InvariantCulture),
            //                    Size = int.Parse(tokens[2]),
            //                    Name = tokens[3]
            //                };
            //                tmp.Add(fileData);
            //            }

            //        }
            //    }
            //}
            //catch
            //{
            //}
            //finally
            //{
            //    reader.Close();
            //    response.Close();
            //}

            try
            {
                //using (var response = (FtpWebResponse)request.GetResponse())
                using (var response = BIWMISFtpWebRequest.GetResponse(request))
                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream))
                {
                    string Data = reader.ReadToEnd();
                    foreach (string item in Regex.Split(Data, "\r\n"))
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            var tokens = item.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (tokens.Length > 3)
                            {
                                var fileData = new FTPFileData
                                {
                                    DateCreated = DateTime.ParseExact(tokens[0] + tokens[1], "MM-dd-yyhh:mmtt", CultureInfo.InvariantCulture),
                                    Size = int.Parse(tokens[2]),
                                    Name = tokens[3]
                                };
                                tmp.Add(fileData);
                            }

                        }
                    }
                }
            }
            catch (Exception genericException)
            {
                //throw genericException;
            }
            finally
            {
                request.Abort();
            }

            return tmp;
        }



        private string CreateDirectoryOnFTP(String ftpRelativePath, String newFtpDirectory)
        {
            // Step 1 - Open a request using the full URI, ftp://ftp.server.tld/path/file.ext
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(m_ftpSite + @"/" + ftpRelativePath + @"/" + newFtpDirectory);

            // Step 2 - Configure the connection request
            request.Credentials = new NetworkCredential(m_username, m_password);
            request.UseBinary = true;
            request.Method = WebRequestMethods.Ftp.MakeDirectory;

            // Step 3 - Call GetResponse() method to actually attempt to create the directory
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            BIWMISFtpWebRequest.GetResponse(request).Close();
            return newFtpDirectory;

        }

        private bool FtpDirectoryExists(string ftpRelativePath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(m_ftpSite + @"/" + ftpRelativePath); // + @"/"); 
                request.Credentials = new NetworkCredential(m_username, m_password);
                request.UseBinary = true;
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                BIWMISFtpWebRequest.GetResponse(request).Close();
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.Undefined)
                {
                    return false;
                }
            }
            return true;
        }

        private bool FtpFileExists(string ftpFilePathAndName)
        {
            FtpWebRequest _FtpRequest = (FtpWebRequest)WebRequest.Create(m_ftpSite + @"/" + ftpFilePathAndName);
            _FtpRequest.UseBinary = true;
            _FtpRequest.Credentials = new NetworkCredential(m_username, m_password);
            _FtpRequest.Method = WebRequestMethods.Ftp.GetFileSize;

            try
            {
                FtpWebResponse response = (FtpWebResponse)_FtpRequest.GetResponse();
                BIWMISFtpWebRequest.GetResponse(_FtpRequest).Close();
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode ==
                    FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    return false;
                }
            }

            return true;
        }

        private bool FtpRenameFile(string ftpFilePathAndName, string newName)
        {
            try
            {
                string newPathFileName = ftpFilePathAndName.RemoveUriFileName() + newName;
                if (FtpFileExists(newPathFileName))
                {
                    FtpWebRequest _FtpRequest0 = (FtpWebRequest)WebRequest.Create(m_ftpSite + @"/" + newPathFileName);
                    _FtpRequest0.Credentials = new NetworkCredential(m_username, m_password);
                    _FtpRequest0.Method = WebRequestMethods.Ftp.Rename;

                    _FtpRequest0.RenameTo = string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), newName);
                    _FtpRequest0.GetResponse().Close();
                }

                FtpWebRequest _FtpRequest = (FtpWebRequest)WebRequest.Create(m_ftpSite + @"/" + ftpFilePathAndName);
                _FtpRequest.Credentials = new NetworkCredential(m_username, m_password);
                _FtpRequest.Method = WebRequestMethods.Ftp.Rename;

                _FtpRequest.RenameTo = newName;
                //_FtpRequest.GetResponse().Close();
                BIWMISFtpWebRequest.GetResponse(_FtpRequest).Close();
            }
            catch
            {
                System.Diagnostics.Debug.Flush();
                System.Diagnostics.Debug.Close();
                return false;
            }

            return true;
        }

        private bool FtpDeleteFile(string ftpFilePathAndName)
        {
            try
            {
                FtpWebRequest _FtpRequest = (FtpWebRequest)WebRequest.Create(m_ftpSite + @"/" + ftpFilePathAndName);
                _FtpRequest.Credentials = new NetworkCredential(m_username, m_password);
                _FtpRequest.Method = WebRequestMethods.Ftp.DeleteFile;

                //_FtpRequest.GetResponse().Close();
                BIWMISFtpWebRequest.GetResponse(_FtpRequest).Close();
            }
            catch
            {
                return false;
            }

            return true;
        }



        private void FtpCopyStream(Stream destination, Stream source)
        {
            int count;
            byte[] buffer = new byte[4096];
            while ((count = source.Read(buffer, 0, buffer.Length)) > 0)
                destination.Write(buffer, 0, count);
        }

        private string FtpGetFile(string ftpFilePathAndName, string destinationPathAndName)
        {

            try
            {
                FtpWebRequest _FtpRequest = (FtpWebRequest)WebRequest.Create(m_ftpSite + @"/" + @ftpFilePathAndName);
                _FtpRequest.UseBinary = true;
                _FtpRequest.Credentials = new NetworkCredential(m_username, m_password);
                _FtpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                //FtpWebResponse response = (FtpWebResponse)_FtpRequest.GetResponse();
                FtpWebResponse response = BIWMISFtpWebRequest.GetResponse(_FtpRequest);

                Stream responseStream = response.GetResponseStream();

                string _destDir = Path.GetDirectoryName(destinationPathAndName);
                if (!Directory.Exists(_destDir))
                {
                    Directory.CreateDirectory(_destDir);
                }

                using (FileStream sw = new FileStream(destinationPathAndName, FileMode.Create))
                {
                    FtpCopyStream(sw, responseStream);
                }

                BIWMISFtpWebRequest.GetResponse(_FtpRequest).Close();
                return destinationPathAndName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private bool FtpPutFile(string ftpFilePathAndName, string originFilePathAndName)
        {
            try
            {
                FtpWebRequest _FtpRequest = (FtpWebRequest)WebRequest.Create(m_ftpSite + @"/" + ftpFilePathAndName);
                _FtpRequest.UseBinary = true;
                _FtpRequest.Credentials = new NetworkCredential(m_username, m_password);
                _FtpRequest.Method = WebRequestMethods.Ftp.UploadFile;

                FileInfo _File = new FileInfo(originFilePathAndName);

                byte[] _fileContents = new byte[_File.Length];
                FileStream fr = _File.OpenRead();
                fr.Read(_fileContents, 0, Convert.ToInt32(_File.Length));
                fr.Close();

                //Stream writer = _FtpRequest.GetRequestStream();
                Stream writer = BIWMISFtpWebRequest.GetRequestStream(_FtpRequest);

                writer.Write(_fileContents, 0, _fileContents.Length);
                writer.Close();

                BIWMISFtpWebRequest.GetResponse(_FtpRequest).Close();
            }
            catch
            {
                return false;
            }
            return true;
        }


        //private void TestDepMainFTPDirectory(string depdes)
        //{
        //    if (!FtpDirectoryExists(@depdes))
        //        this.CreateDirectoryOnFTP("", @depdes);
        //}

        public string TestDepSubFTPDirectory(string depdes, string directoryType)
        {
            //if (!FtpDirectoryExists( @depdes))
            //    this.CreateDirectoryOnFTP("", @depdes);
            if (!FtpDirectoryExists(directoryType))
            {
                return this.CreateDirectoryOnFTP("", directoryType);
            }
            return directoryType;

        }

        //public bool CreatePathAndUpload(string depdes, string filePathAndName)
        //{
        //    string _ftpPath = TestDepSubFTPDirectory(depdes, "out");
        //    bool result = this.FtpPutFile(_ftpPath + @"/" + Path.GetFileName(filePathAndName), filePathAndName);
        //    Log.WriteGeneralEntry("UPLOAD: " + _ftpPath + @"/" + Path.GetFileName(filePathAndName) + "    ****************");
        //    return result;
        //}

        public bool Upload(string localPath, string fileName)
        {
            bool result = this.FtpPutFile(Path.Combine(OutputDir, fileName), Path.Combine(localPath, fileName));
            //Log.WriteGeneralEntry("UPLOAD: " + OutputDir + @"/" + Path.GetFileName(Path.Combine(localPath, fileName)) + "    ****************");
            return result;
        }

        public string Download(string fileName, string localPath)
        {
            string result = this.FtpGetFile(Path.Combine(InputDir, fileName), Path.Combine(localPath, fileName));
            //Log.WriteGeneralEntry("DOWNLOAD: " + InputDir + @"/" + Path.GetFileName(Path.Combine(localPath, fileName)) + "    ****************");
            return result;
        }

        //public bool CreatePathAndUploadToInputForTest(string depdes, string filePathAndName)
        //{
        //    string _ftpPath = TestDepSubFTPDirectory(depdes, "in");
        //    bool result = this.FtpPutFile(_ftpPath + @"/" + Path.GetFileName(filePathAndName), filePathAndName);
        //    Log.WriteGeneralEntry("UPLOAD: " + _ftpPath + @"/" + Path.GetFileName(filePathAndName) + "    ****************");
        //    return result;
        //}

        public List<string> GetListOfFiles(string ftpFolder)
        {
            return this.FtpGetList(ftpFolder);

        }

        public bool OutputDirContains(string fileExtension)
        {

            foreach (string _file in FtpGetList(OutputDir))
            {
                if (_file.Substring(_file.LastIndexOf('.') + 1).ToLowerInvariant() == fileExtension.ToLowerInvariant())
                    return true;
            }
            return false;
        }

        public bool OutputDirFileExists(string fileName)
        {

            foreach (string _file in FtpGetList(OutputDir))
            {
                if (_file.ToLowerInvariant() == fileName.ToLowerInvariant())
                    return true;
            }
            return false;
        }

        public bool InputDirContains(string fileExtension)
        {

            foreach (string _file in FtpGetList(InputDir))
            {
                if (_file.Substring(_file.LastIndexOf('.') + 1).ToLowerInvariant() == fileExtension.ToLowerInvariant())
                    return true;
            }
            return false;
        }
        public string InputDirGetOldest(string fileExtension)
        {
            string _result = string.Empty;
            foreach (string _file in FtpGetList(InputDir).OrderBy(f => f))
            {
                if (_file.Substring(_file.LastIndexOf('.') + 1).ToLowerInvariant() == fileExtension.ToLowerInvariant())
                {
                    _result = _file;
                    break;
                }
            }
            return _result;
        }


        public FTPFileData FTPDirGetOldest(string direction, string fileExtension)
        {
            FTPFileData _result = null;

            List<FTPFileData> files = FtpGetTypedList(direction == "I" ? InputDir : OutputDir);
            if (files != null)
            {
                _result = files.OrderBy(f => f.DateCreated).FirstOrDefault();
            }
            return _result;
        }


        public bool InputDirFileExists(string fileName)
        {

            foreach (string _file in FtpGetList(InputDir))
            {
                if (_file.ToLowerInvariant() == fileName.ToLowerInvariant())
                    return true;
            }
            return false;
        }


        public void Delete(string ftpFilePath)
        {
            this.FtpDeleteFile(ftpFilePath);
            //Log.WriteGeneralEntry("DELETED: " + ftpFilePath + "    ****************");

        }

        

        public bool Rename(string ftpFilePath, string newName)
        {
            bool result = this.FtpRenameFile(ftpFilePath, newName);
            //Log.WriteGeneralEntry("RENAME: " + ftpFilePath + "==>" + newName + "    ****************");
            return result;
        }


        public bool RenameFileExtension(string ftpPath, string fileName, string newExtension)
        {
            bool result = true;
            if (Path.GetFileNameWithoutExtension(fileName) == "*")
            {
                foreach (string _file in this.FtpGetList(ftpPath))
                {
                    if (Path.GetExtension(_file) == Path.GetExtension(fileName))
                    {
                        bool inResult = this.FtpRenameFile(Path.Combine(ftpPath, _file), string.Format("{0}.{1}", Path.GetFileNameWithoutExtension(_file), newExtension));
                        if (inResult == false)
                            result = false;
                    }
                }
            }
            else
            {
                result = this.FtpRenameFile(Path.Combine(ftpPath, fileName), string.Format("{0}.{1}", fileName.Substring(0, fileName.LastIndexOf('.')), newExtension));
            }
            //Log.WriteGeneralEntry("RENAME EXTENSION: " + Path.Combine(ftpPath, fileName) + "==>" + newExtension + "    ****************");
            return result;
        }


    }
}
