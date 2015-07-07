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
        public enum RepositoryType : byte
        {
            EntityFramework = 0,
            NHabernate = 1,
            ADO = 2,
            TextFile = 3,
            XmlFile = 4
        }

        /// <summary>
        /// Enum responsável por pontos de referência para geração de thumbnails (Core\Images\Thumbnail.cs)
        /// </summary>
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
