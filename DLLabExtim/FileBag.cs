using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLLabExtim
{
    public class FileBag
    {
        public string FileName { get; internal set; }
        public string UniqueFilename { get; internal set; }

        public FileBag(string fileName, string uniqueFileName)
        {
            FileName = fileName;
            UniqueFilename = uniqueFileName;
        }


    }
}
