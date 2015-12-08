using IOC.FW.Core.Abstraction.FTP;
using IOC.FW.Core.Model.FTP;
using IOC.FW.FTP.Handlers;
using IOC.FW.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading;

namespace IOC.FW.FTP
{
    public class FTP : IFTP
    {
        private string _host;
        private string _userName;
        private string _password;
        private int _port;
        private const int BufferSize = 1024;
        private const string SearchPattern = "*";
        private readonly int ChunkSize = (5 * 1024);

        public delegate void ErrorHandler(ErrorHandlerArgs args);
        public event ErrorHandler OnError;

        public FTP()
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

            Check.If<ArgumentException, int>(
                () => port,
                p => p > 0
            );

            _host = !host.Contains("ftp://")
                ? string.Format("ftp://{0}", host)
                : host;
            _port = port;
            _userName = userName;
            _password = password;
        }

        public void Download(
            string sourcePath,
            string destPath
        )
        {
            Check.IfNullOrEmpty(
                new Expression<Func<string>>[] {
                    () => sourcePath,
                    () => destPath
                }
            );

            if (!Path.HasExtension(destPath))
            {
                var fileName = Path.GetFileName(sourcePath);
                destPath = string.Concat(
                    destPath,
                    Path.DirectorySeparatorChar,
                    fileName
                );
            }

            var request = Connect(sourcePath, WebRequestMethods.Ftp.DownloadFile);
            request.Timeout = 5000;

            using (var response = (FtpWebResponse)request.GetResponse())
            using (var streamRead = response.GetResponseStream())
            {
                var streamWrite = new FileStream(
                    destPath,
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,
                    FileShare.ReadWrite
                );

                WriteFile(streamRead, streamWrite);
                response.Close();
            }
        }

        public void Upload(
            string sourcePath,
            string destPath
        )
        {
            Check.IfNullOrEmpty(
                new Expression<Func<string>>[] {
                    () => sourcePath,
                    () => destPath
                }
            );

            Upload(sourcePath, destPath, true);
        }

        public void Upload(
            string sourcePath,
            string destPath,
            bool overwrite
        )
        {
            Check.IfNullOrEmpty(
                new Expression<Func<string>>[] {
                    () => sourcePath,
                    () => destPath
                }
            );

            var exists = File.Exists(sourcePath);
            if (exists)
            {
                if (!Path.HasExtension(destPath))
                {
                    var fileName = Path.GetFileName(sourcePath);
                    destPath = string.Concat(
                        destPath,
                        Path.AltDirectorySeparatorChar,
                        fileName
                    );
                }

                var method = overwrite
                    ? WebRequestMethods.Ftp.UploadFile
                    : WebRequestMethods.Ftp.UploadFileWithUniqueName;

                var request = Connect(destPath, method);
                request.Timeout = 50000;
                request.ContentLength = (new FileInfo(sourcePath)).Length;
                request.UsePassive = true;

                using (var streamRead = new FileStream(
                    sourcePath,
                    FileMode.Open,
                    FileAccess.ReadWrite,
                    FileShare.ReadWrite
                ))
                {
                    var streamWrite = request.GetRequestStream();
                    WriteFile(streamRead, streamWrite);

                    using (var response = (FtpWebResponse)request.GetResponse())
                    {
                        var responseStream = response.GetResponseStream();
                        responseStream.Close();
                        response.Close();
                    }
                }

                Thread.Sleep(200);
            }
        }

        public void Rename(
            string filePath,
            string newFileName
        )
        {
            Check.IfNullOrEmpty(
                new Expression<Func<string>>[] {
                    () => filePath,
                    () => newFileName
                }
            );

            var exists = FileExists(filePath);
            if (exists)
            {
                var request = Connect(filePath, WebRequestMethods.Ftp.Rename);
                request.RenameTo = newFileName;

                using (var response = (FtpWebResponse)request.GetResponse())
                {
                    ExecuteCommand(
                        response.GetResponseStream()
                    );
                    response.Close();
                }
            }
        }

        public void DeleteFile(
            string path
        )
        {
            Check.IfNullOrEmpty(() => path);

            var request = Connect(path, WebRequestMethods.Ftp.DeleteFile);
            using (var response = (FtpWebResponse)request.GetResponse())
            {
                ExecuteCommand(
                    response.GetResponseStream()
                );
                response.Close();
            }
        }

        public void CreateDirectory(
            string path
        )
        {
            Check.IfNullOrEmpty(() => path);

            var parents = path
                .TrimStart(Path.AltDirectorySeparatorChar)
                .Split(Path.AltDirectorySeparatorChar);

            var parentPath = string.Empty;
            foreach (var parent in parents)
            {
                parentPath = string.Concat(
                    parentPath,
                    Path.AltDirectorySeparatorChar,
                    parent
                );

                var exists = DirectoryExists(parentPath);
                if (!exists)
                {
                    var request = Connect(parentPath, WebRequestMethods.Ftp.MakeDirectory);
                    using (var response = (FtpWebResponse)request.GetResponse())
                    {
                        ExecuteCommand(
                            response.GetResponseStream()
                        );
                        response.Close();
                    }
                }
            }
        }

        public void RemoveDirectory(
            string path
        )
        {
            Check.IfNullOrEmpty(() => path);

            var exists = DirectoryExists(path);
            if (exists)
            {
                var request = Connect(path, WebRequestMethods.Ftp.RemoveDirectory);
                using (var response = (FtpWebResponse)request.GetResponse())
                {
                    ExecuteCommand(
                        response.GetResponseStream()
                    );
                    response.Close();
                }
            }
        }

        public IEnumerable<FtpFileInfo> GetFiles(
            string path,
            string searchPattern,
            SearchOption searchOption
        )
        {


            Check.IfNullOrEmpty(
                new Expression<Func<string>>[] {
                    () => path,
                    () => searchPattern
                }
            );

            var request = Connect(path, WebRequestMethods.Ftp.ListDirectory);
            var files = new List<FtpFileInfo>();
            var subDirectories = new List<string>();

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                if (searchPattern != SearchPattern)
                {
                    searchPattern = searchPattern.Replace(SearchPattern, string.Empty);
                }

                ExecuteCommand(
                    response.GetResponseStream(),
                    (line) =>
                    {
                        var fileOrDirectory = line;
                        bool isFile = Path.HasExtension(fileOrDirectory);

                        if (!string.IsNullOrWhiteSpace(fileOrDirectory))
                        {
                            if (isFile)
                            {
                                if (
                                    searchPattern.Equals(SearchPattern)
                                    || fileOrDirectory.Contains(searchPattern)
                                )
                                {
                                    var filePath = string.Concat(
                                        path,
                                        Path.AltDirectorySeparatorChar,
                                        fileOrDirectory
                                    );

                                    var indexLastBackSlash = filePath.LastIndexOf('/');
                                    var dirName = filePath.Substring(0, indexLastBackSlash);
                                    var fileName = filePath.Substring(
                                        indexLastBackSlash > 0
                                            ? indexLastBackSlash + 1
                                            : indexLastBackSlash,
                                        filePath.Length - indexLastBackSlash - 1
                                    );

                                    var fileInfo = new FtpFileInfo
                                    {
                                        FullName = string.Concat(_host, filePath),
                                        RelativePath = filePath,
                                        DirectoryName = dirName,
                                        Name = fileName,

                                    };

                                    Console.WriteLine(fileInfo.FullName);
                                    files.Add(fileInfo);
                                }
                            }
                            else
                            {
                                if (searchOption.Equals(SearchOption.AllDirectories))
                                {
                                    var newPath = string.Concat(
                                        path,
                                        Path.AltDirectorySeparatorChar,
                                        fileOrDirectory
                                    );
                                    subDirectories.Add(newPath);
                                }
                            }
                        }
                    }
                );

                response.Close();
            }

            if (subDirectories.Count > 0)
            {
                foreach (var subDirectory in subDirectories)
                {
                    var moreFiles = GetFiles(subDirectory, searchPattern, searchOption);
                    files.AddRange(moreFiles);
                }
            }

            return files;
        }

        private FtpWebRequest Connect(
            string requestUri,
            string requestMethod = WebRequestMethods.Ftp.ListDirectory
        )
        {
            Check.IfNullOrEmpty(
                new Expression<Func<string>>[] {
                    () => requestUri,
                    () => requestMethod
                }
            );

            if (requestUri.IndexOf(_host) == -1)
            {
                requestUri = string.Format(
                    @"{0}/{1}",
                    _host.TrimEnd('/'),
                    requestUri.TrimStart('/')
                );
            }


            var request = (FtpWebRequest)WebRequest.Create(requestUri);
            request.Credentials = new NetworkCredential(_userName, _password);
            request.UseBinary = true;
            request.UsePassive = false;
            request.KeepAlive = false;
            request.Method = requestMethod;

            return request;
        }

        public bool FileExists(
            string path
        )
        {
            Check.IfNullOrEmpty(() => path);

            var fileSize = GetFileSize(path);
            return (fileSize > 0);
        }

        public bool DirectoryExists(
            string path
        )
        {
            Check.IfNullOrEmpty(() => path);

            bool exists = false;

            try
            {
                if (!path.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
                {
                    path = string.Format("{0}/", path);
                }

                var request = Connect(path, WebRequestMethods.Ftp.ListDirectory);
                using (var response = (FtpWebResponse)request.GetResponse())
                {
                    exists = true;
                    response.Close();
                }
            }
            catch (WebException ex)
            {
                if (OnError != null)
                {
                    OnError(new ErrorHandlerArgs
                    {
                        ErrorMessage = "Exception on (bool DirectoryExists (string path))",
                        Exception = ex
                    });
                }

                var response = (FtpWebResponse)ex.Response;
                if (response != null
                    && response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable
                )
                {
                    response.Close();
                    exists = false;
                }
            }

            return exists;
        }

        public long GetFileSize(
            string path
        )
        {
            Check.IfNullOrEmpty(() => path);

            long fileSize = 0;

            try
            {
                var request = Connect(path, WebRequestMethods.Ftp.GetFileSize);
                using (var response = (FtpWebResponse)request.GetResponse())
                {
                    fileSize = response.ContentLength;
                    response.Close();
                }
            }
            catch (WebException ex)
            {
                if (OnError != null)
                {
                    OnError(new ErrorHandlerArgs
                    {
                        ErrorMessage = "Exception on (long GetFileSize(string path))",
                        Exception = ex
                    });
                }

                var response = (FtpWebResponse)ex.Response;
                if (response != null
                    && response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable
                )
                {
                    response.Close();
                    fileSize = 0;
                }
            }

            return fileSize;
        }

        private void WriteFile(
            Stream streamRead,
            Stream streamWrite
        )
        {
            Check.IfNull(
                new Expression<Func<object>>[] {
                    () => streamRead,
                    () => streamWrite
                }
            );

            using (streamRead)
            {
                using (streamWrite)
                {
                    byte[] buffer = new byte[BufferSize];
                    int bytesRead = 0;
                    int totalBytes = 0;

                    while ((bytesRead = streamRead.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        streamWrite.Write(buffer, 0, bytesRead);
                        totalBytes += bytesRead;

                        if (totalBytes >= ChunkSize)
                        {
                            streamWrite.Flush();
                            totalBytes = 0;
                        }
                    }

                    streamWrite.Close();
                }

                streamRead.Close();
            }
        }

        private void ExecuteCommand(
            Stream stream
        )
        {
            Check.IfNull(() => stream);

            ExecuteCommand(
                stream,
                null
            );
        }

        private void ExecuteCommand(
            Stream stream,
            Action<string> action
        )
        {
            Check.IfNull(() => stream);

            using (stream)
            {
                using (var streamReader = new StreamReader(stream))
                {
                    var listBuilder = new StringBuilder();
                    while (!streamReader.EndOfStream)
                    {
                        listBuilder.AppendLine(
                            streamReader.ReadLine()
                        );
                    }

                    if (action != null)
                    {
                        var allLines = listBuilder
                        .ToString()
                        .Split(
                            new string[] { "\r\n" },
                            StringSplitOptions.None
                        )
                        .ToList()
                        .Select(
                            s => s.Substring(
                                s.LastIndexOf("/") > 0
                                    ? s.LastIndexOf("/") + 1
                                    : 0
                            )
                        );

                        foreach (var line in allLines)
                        {
                            action(line);
                        }
                    }
                }

                stream.Close();
            }
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
            Check.IfNullOrEmpty(() => fileWithPath);

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