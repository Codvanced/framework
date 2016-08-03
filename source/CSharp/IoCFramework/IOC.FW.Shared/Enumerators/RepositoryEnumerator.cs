namespace IOC.FW.Shared.Enumerators
{
    /// <summary>
    /// Classe utilizada para armazenar enumeradores de repositórios
    /// </summary>
    public class RepositoryEnumerator
    {
        /// <summary>
        /// Enum responsável por pontos de referência para configuração de qual ORM a DAO utilizará.
        /// </summary>
        public enum RepositoryType : byte
        {
            EF6 = 0,
            NHabernate = 1,
            ADO = 2,
            TextFile = 3,
            XmlFile = 4
        }
    }
}