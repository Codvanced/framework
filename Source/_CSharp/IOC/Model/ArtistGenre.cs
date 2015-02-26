using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IOC.Model
{
    [Table("ArtistGenre")]
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
