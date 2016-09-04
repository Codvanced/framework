using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOC.FW.FTP
{
    public class Enumerators
    {
        public enum StructureType
            : byte
        {
            LocalEnvironment = 0,
            FTP = 1,
            SFTP = 2
        }
    }
}