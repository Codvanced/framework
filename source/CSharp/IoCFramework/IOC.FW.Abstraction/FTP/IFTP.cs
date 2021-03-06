﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IOC.FW.Abstraction.FTP
{
    public interface IFTP
    {
        void Setup(string host, int port, string userName, string password);
        void CreateDirectory(string path);
        void DeleteFile(string path);
        bool DirectoryExists(string path);
        void Download(string sourcePath, string destPath);
        bool FileExists(string path);
        long GetFileSize(string path);
        void RemoveDirectory(string path);
        void Rename(string filePath, string newFileName);
        void Upload(string sourcePath, string destPath);
        void Upload(string sourcePath, string destPath, bool overwrite);
        IEnumerable<string> GetFiles(
            string path,
            string searchPattern,
            SearchOption searchOption
        );
        IEnumerable<string> GetFiles(
            string path,
            string searchPattern
        );
    }
}