namespace IOC.FW.Core.Model.FTP
{
    public class FtpFileInfo
    {
        public string DirectoryName { get; set; }
        public bool Exists { get; set; }
        public bool IsReadOnly { get; set; }
        public long Length { get; set; }
        public string Name { get; set; }
        public string RelativePath { get; set; }
        public string FullName { get; set; }
        public bool DirectoryExists { get; set; }
    }
}