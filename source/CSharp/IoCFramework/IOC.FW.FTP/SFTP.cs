﻿using IOC.FW.Core.Abstraction.FTP;
using IOC.FW.Core.Model.FTP;
using IOC.FW.FTP.Handlers;
using IOC.FW.Validation;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace IOC.FW.FTP
{
    public class SFTP : IFTP
    {
        private string _host;
        private string _userName;
        private string _password;
        private int _port;
        private const int BufferSize = 1024;
        private const string SearchPattern = "*";

        public delegate void ErrorHandler(ErrorHandlerArgs args);
        public event ErrorHandler OnError;

        public SFTP()
        {
        }

        public void Setup(string host, int port, string userName, string password)
        {
            Check.IfNullOrEmpty(
                new Expression<Func<string>>[] {
                    () => host,
                    () => userName,
                    () => password
                }
            );

            _host = host;
            _port = port;
            _userName = userName;
            _password = password;
        }

        private void ExecuteCommand(Action<SftpClient> action)
        {
            Check.IfNull(() => action);
            using (var sftp = new SftpClient(_host, _port, _userName, _password))
            {
                sftp.Connect();
                action(sftp);
                sftp.Disconnect();
            }
        }

        private TOut ExecuteCommand<TOut>(Func<SftpClient, TOut> action)
        {
            Check.IfNull(() => action);

            TOut result = default(TOut);
            using (var sftp = new SftpClient(
                _host,
                _port,
                _userName,
                _password
            ))
            {
                sftp.Connect();
                result = action(sftp);
                sftp.Disconnect();
            }
            return result;
        }

        public void CreateDirectory(string path)
        {
            Check.IfNullOrEmpty(() => path);

            ExecuteCommand(
                sftp => sftp.CreateDirectory(path)
            );
        }

        public void DeleteFile(string path)
        {
            Check.IfNullOrEmpty(() => path);

            ExecuteCommand(
                sftp => sftp.DeleteFile(path)
            );
        }

        public bool DirectoryExists(string path)
        {
            Check.IfNullOrEmpty(() => path);

            return ExecuteCommand(
                sftp => sftp.Exists(path)
            );
        }

        public void Download(string sourcePath, string destPath)
        {
            Check.IfNullOrEmpty(
                new Expression<Func<string>>[] {
                    () => sourcePath,
                    () => destPath
                }
            );

            ExecuteCommand(
                sftp =>
                {
                    using (var fileStream = new FileStream(
                        destPath,
                        FileMode.OpenOrCreate,
                        FileAccess.ReadWrite
                    ))
                    {
                        sftp.DownloadFile(sourcePath, fileStream);
                    }
                }
            );
        }

        public bool FileExists(string path)
        {
            Check.IfNullOrEmpty(() => path);

            return ExecuteCommand(
                sftp => sftp.Exists(path)
            );
        }

        public long GetFileSize(string path)
        {
            Check.IfNullOrEmpty(() => path);

            return ExecuteCommand(
                sftp => sftp.Get(path).Length
            );
        }

        public void RemoveDirectory(string path)
        {
            Check.IfNullOrEmpty(() => path);

            ExecuteCommand(
                sftp => sftp.DeleteDirectory(path)
            );
        }

        public void Rename(string filePath, string newFileName)
        {
            Check.IfNullOrEmpty(
                new Expression<Func<string>>[] {
                    () => filePath,
                    () => newFileName
                }
            );

            ExecuteCommand(
                sftp => sftp.RenameFile(filePath, newFileName)
            );
        }

        public void Upload(string sourcePath, string destPath)
        {
            Check.IfNullOrEmpty(
                new Expression<Func<string>>[] {
                    () => sourcePath,
                    () => destPath
                }
            );

            Upload(sourcePath, destPath, false);
        }

        public void Upload(string sourcePath, string destPath, bool overwrite)
        {
            Check.IfNullOrEmpty(
                new Expression<Func<string>>[] {
                    () => sourcePath,
                    () => destPath
                }
            );

            ExecuteCommand(
                sftp =>
                {
                    using (var fileStream = new FileStream(
                            sourcePath,
                            FileMode.Open,
                            FileAccess.Read
                        ))
                    {
                        sftp.UploadFile(fileStream, destPath, overwrite);
                    }
                }
            );
        }

        public IEnumerable<FtpFileInfo> GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            Check.IfNullOrEmpty(
                new Expression<Func<string>>[] {
                    () => path,
                    () => searchPattern
                }
            );

            var filesFound = new List<FtpFileInfo>();

            ExecuteCommand(
               sftp =>
               {
                   var fileList = sftp.ListDirectory(path);

                   if (fileList != null && fileList.GetEnumerator().MoveNext())
                   {
                       foreach (var file in fileList)
                       {
                           if (file.IsDirectory
                                && searchOption == SearchOption.AllDirectories
                                && (
                                    !file.Name.Equals("..")
                                    && !file.Name.Equals(".")
                                )
                           )
                           {
                               var recursiveFilesFound = GetFiles(
                                   file.FullName,
                                   searchPattern,
                                   searchOption
                                );

                               filesFound.AddRange(recursiveFilesFound);
                           }
                           else if (file.IsRegularFile)
                           {
                               var lastSlash = file.FullName.LastIndexOf("/");

                               string directoryPath = file.FullName.Remove(
                                  lastSlash
                               );

                               filesFound.Add(new FtpFileInfo
                               {
                                   Name = file.Name,
                                   FullName = file.FullName,
                                   DirectoryName = directoryPath,
                                   RelativePath = file.FullName,
                                   Exists = true,
                                   IsReadOnly = !file.OwnerCanWrite,
                                   Length = file.Length
                               });
                           }
                           else
                           {
                               if (OnError != null)
                               {
                                   OnError(new ErrorHandlerArgs
                                   {
                                       ErrorMessage = "Unknown file type!",
                                       Exception = null
                                   });
                               }
                           }
                       }
                   }
               }
            );

            return filesFound;
        }

        public IEnumerable<FtpFileInfo> GetFiles(string path, string searchPattern)
        {
            Check.IfNullOrEmpty(
                new Expression<Func<string>>[] {
                    () => path,
                    () => searchPattern
                }
            );

            return GetFiles(path, searchPattern, SearchOption.AllDirectories);
        }

        public FtpFileInfo GetFileInfo(string fileWithPath)
        {
            Check.IfNullOrEmpty(
                new Expression<Func<string>>[] {
                    () => fileWithPath
                }
            );

            var indexLastBackSlash = fileWithPath.LastIndexOf('/');
            var dirName = fileWithPath.Substring(0, indexLastBackSlash);
            var fileName = fileWithPath.Substring(
                indexLastBackSlash > 0
                    ? indexLastBackSlash + 1
                    : indexLastBackSlash,
                fileWithPath.Length - indexLastBackSlash - 1
            );

            return new FtpFileInfo
            {
                FullName = string.Concat(_host, fileWithPath),
                RelativePath = fileWithPath,
                DirectoryName = dirName,
                Name = fileName,
            };
        }
    }
}