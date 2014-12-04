using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOC.FW.Core
{
    /// <summary>
    /// Classe utilizada para armazenar enumeradores
    /// </summary>
    public class Enumerators
    {
        //public enum RepositoryType : byte
        //{
        //    SqlServer = 0,
        //    Oracle = 1,
        //    MySql = 2,
        //    TextFileStorage = 3,
        //    XmlFileStorage = 4
        //}

        //public enum CompressionType : byte
        //{
        //    Zip = 0,
        //    BZip2 = 1,
        //    GZip = 2,
        //    Tar = 3,
        //    Rar = 4,
        //    SevenZip = 5
        //}

        //public enum DebugType : byte
        //{
        //    Info = 0,
        //    Warn = 1,
        //    Error = 2
        //}

        public enum ReferencePoint : byte
        {
            TopLeft = 1,
            TopCenter = 2,
            TopRight = 3,
            MiddleLeft = 4,
            MiddleCenter = 5,
            MiddleRight = 6,
            BottomLeft = 7,
            BottomCenter = 8,
            BottomRight = 9
        }
    }
}
