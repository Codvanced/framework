namespace IOC.FW.ImageTransformation
{
    /// <summary>
    /// Classe utilizada para armazenar enumeradores de image transforme
    /// </summary>
    public class Enumerators
    {
        /// <summary>
        /// Enum responsável por pontos de referência para geração de thumbnails
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