using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace IOC.Model
{
    [Table(Name = "ArtistGenre")]
    public class ArtistGenre
    {
        [Key]
        public int IdArtistGenre { get; set; }

        public int IdArtist { get; set; }
        public virtual Artist Artist { get; set; }

        public int IdGenre { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
