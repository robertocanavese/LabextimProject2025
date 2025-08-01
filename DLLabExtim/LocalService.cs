using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using CMLabExtim;


namespace DLLabExtim
{
    public class LocalService
    {

        private string m_localFolderIn;
        private string m_localFolderOut;
        private string m_localFolderError;
        private string m_localFolderArchIn;
        private string m_localFolderArchOut;

        public string InputDir { get { return m_localFolderIn; } }
        public string OutputDir { get { return m_localFolderOut; } }
        public string ErrorDir { get { return m_localFolderError; } }
        public string ArchInDir { get { return m_localFolderArchIn; } }
        public string ArchOutDir { get { return m_localFolderArchOut; } }

        public LocalService(SharedConfiguration sharedConfig)
        {
            m_localFolderIn = string.Format(@"{0}\{1}", sharedConfig.LocRootPath, sharedConfig.LocInputPath);
            m_localFolderOut = string.Format(@"{0}\{1}", sharedConfig.LocRootPath, sharedConfig.LocOutputPath);
            m_localFolderError = string.Format(@"{0}\{1}", sharedConfig.LocRootPath, sharedConfig.LocErrorPath);
            m_localFolderArchIn = string.Format(@"{0}\{1}", sharedConfig.LocRootPath, sharedConfig.LocArchInPath);
            m_localFolderArchOut = string.Format(@"{0}\{1}", sharedConfig.LocRootPath, sharedConfig.LocArchOutPath);
        }


        // Area TX
        public bool OutputDirContains(string fileExtension)
        {
            return (new DirectoryInfo(OutputDir).GetFiles(string.Format("*.{0}", fileExtension)).Count() > 0);
        }
        public bool OutputDirFileExists(string fileName)
        {
            return (new DirectoryInfo(OutputDir).GetFiles(fileName).FirstOrDefault(f => f.Name.ToLowerInvariant() == fileName.ToLowerInvariant()) != null);
        }

        public bool OutputDirContainsFile(string fileName)
        {
            return (new DirectoryInfo(OutputDir).GetFiles().FirstOrDefault(f => f.Name.ToLowerInvariant() == fileName.ToLowerInvariant()) != null);
        }

        public IEnumerable<FileInfo> OutputDirGetFiles(string fileExtension)
        {
            return new DirectoryInfo(OutputDir).GetFiles(string.Format("*.{0}", fileExtension));
        }

        public IEnumerable<FileBag> OutputDirGetFileBags(string fileExtension)
        {
            return new DirectoryInfo(OutputDir).GetFiles(string.Format("*.{0}", fileExtension)).Select(f => new FileBag(f.Name, f.Name));
        }


        public void OutputDirDeleteFiles(string fileExt)
        {
            foreach (FileInfo f in new DirectoryInfo(OutputDir).GetFiles(string.Format("*.{0}", fileExt)))
            {
                f.Delete();
            }
        }

        public void OutputDirRenameFileExtension(string oldExtension, string newExtension)
        {
            DirectoryInfo _di = new DirectoryInfo(OutputDir);
            foreach (FileInfo f in _di.GetFiles(string.Format("*.{0}", oldExtension)))
            {
               if (_di.GetFiles(f.Name).Count() > 0)
                    if (_di.GetFiles(string.Format("{0}.{1}", f.Name.Substring(0, f.Name.LastIndexOf('.')), newExtension)).Count() == 0)
                    {
                        f.MoveTo(Path.Combine(OutputDir, string.Format("{0}.{1}", f.Name.Substring(0, f.Name.LastIndexOf('.')), newExtension)));
                    }
                    else
                    {
                        f.MoveTo(Path.Combine(OutputDir, string.Format("Z{0}.{1}", string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), string.Format("{0}.{1}", f.Name.Substring(0, f.Name.LastIndexOf('.')), newExtension)))));
                        f.MoveTo(Path.Combine(OutputDir, string.Format("{0}.{1}", f.Name.Substring(0, f.Name.LastIndexOf('.')), newExtension)));
                    }
            }
        }


        //area RX
        public bool InputDirContains(string fileExtension)
        {
            return (new DirectoryInfo(InputDir).GetFiles(string.Format("*.{0}", fileExtension)).Count() > 0);
        }

        public bool InputDirFileExists(string fileName)
        {
            return (new DirectoryInfo(InputDir).GetFiles(fileName).FirstOrDefault(f => f.Name.ToLowerInvariant() == fileName.ToLowerInvariant()) != null);
        }

        public bool ErrorDirFileExists(string fileName)
        {
            return (new DirectoryInfo(ErrorDir).GetFiles(fileName).FirstOrDefault(f => f.Name.ToLowerInvariant() == fileName.ToLowerInvariant()) != null);
        }

        public bool InputDirFileWithOtherExtensionExists(string fileName, string fileExtension)
        {
            return (new DirectoryInfo(InputDir).GetFiles().FirstOrDefault(f => f.Name.ToLowerInvariant() == fileName.Replace(f.Extension, fileExtension).ToLowerInvariant()) != null);
        }

        public IEnumerable<FileInfo> InputDirGetFiles(string fileExtension)
        {
            return new DirectoryInfo(InputDir).GetFiles(string.Format("*.{0}", fileExtension));
        }

        public string InputDirGetOldest(string fileExtension)
        {
            string _result = string.Empty;
            FileInfo _found = new DirectoryInfo(InputDir).GetFiles(string.Format("*.{0}", fileExtension)).OrderBy(f => f.Name).FirstOrDefault();
            if (_found != null)
                _result = _found.Name;
            return _result;
        }


        public IEnumerable<FileBag> InputDirGetFileBags(string fileExtension)
        {
            return new DirectoryInfo(InputDir).GetFiles(string.Format("*.{0}", fileExtension)).Select(f => new FileBag(f.Name, f.Name));
        }

        public void InputDirDeleteFiles(string fileExt)
        {
            foreach (FileInfo f in new DirectoryInfo(InputDir).GetFiles(string.Format("*.{0}", fileExt)))
            {
                f.Delete();
            }
        }

        public void InputDirDeleteFile(string fileNameWithExtension)
        {
            FileInfo f = new FileInfo(Path.Combine(InputDir, fileNameWithExtension));
            {
                f.Delete();
            }
        }

        public void InputDirRenameFileExtension(string oldExtension, string newExtension)
        {
            foreach (FileInfo f in new DirectoryInfo(InputDir).GetFiles(string.Format("*.{0}", oldExtension)))
            {
                if (File.Exists(Path.Combine(InputDir, string.Format("{0}.{1}", f.Name.Substring(0, f.Name.LastIndexOf('.')), newExtension))))
                {
                    FileInfo found = new FileInfo((Path.Combine(InputDir, string.Format("{0}.{1}", f.Name.Substring(0, f.Name.LastIndexOf('.')), newExtension))));
                    string uniqueName = string.Format("Z{0}_{1}.{2}", DateTime.Now.ToString("yyyyMMddHHmmss"), f.Name.Substring(0, f.Name.LastIndexOf('.')), newExtension);
                    found.MoveTo(Path.Combine(InputDir, uniqueName));
                }
                f.MoveTo(Path.Combine(InputDir, string.Format("{0}.{1}", f.Name.Substring(0, f.Name.LastIndexOf('.')), newExtension)));
            }
        }


        // Comuni
        public void RenameFileExtension(string path, string fileName, string newExtension)
        {
            FileInfo f = new FileInfo(Path.Combine(path, fileName));
            f.MoveTo(Path.Combine(path, string.Format("{0}.{1}", fileName.Substring(0, fileName.LastIndexOf('.')), newExtension)));
        }

        public void MoveFileToDir(string fileName, string originDir, string destinationDir)
        {
            FileInfo f = new FileInfo(Path.Combine(originDir, fileName));
            f.MoveTo(Path.Combine(Path.Combine(destinationDir, fileName)));
        }

        public void RenameAndMoveFileToDir(string oldFileName, string newFileName, string originDir, string destinationDir)
        {
            FileInfo f = new FileInfo(Path.Combine(originDir, oldFileName));
            f.MoveTo(Path.Combine(Path.Combine(destinationDir, newFileName)));
        }


        public string ArchivedInputDirGetLastFile(string fileExtension)
        {
            FileInfo file = new DirectoryInfo(ArchInDir).GetFiles(string.Format("*.{0}", fileExtension)).OrderByDescending(f => f.CreationTime).FirstOrDefault();
            if (file != null)
                return file.FullName;
            else
                return null;
        }

        public string ArchivedOutputDirGetLastFile(string fileExtension)
        {
            FileInfo file = new DirectoryInfo(ArchOutDir).GetFiles(string.Format("*.{0}", fileExtension)).OrderByDescending(f => f.CreationTime).FirstOrDefault();
            if (file != null)
                return file.FullName;
            else
                return null;
        }

        public bool FileCompare(string file1, string file2)
        {
            int file1byte;
            int file2byte;
            FileStream fs1;
            FileStream fs2;

            if (file1 == file2)
            {
                return true;
            }

            fs1 = new FileStream(file1, FileMode.Open);
            fs2 = new FileStream(file2, FileMode.Open);
            if (fs1.Length != fs2.Length)
            {
                fs1.Close();
                fs2.Close();
                return false;
            }

            do
            {
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            }
            while ((file1byte == file2byte) && (file1byte != -1));
            fs1.Close();
            fs2.Close();

            return ((file1byte - file2byte) == 0);
        }

    }
}
