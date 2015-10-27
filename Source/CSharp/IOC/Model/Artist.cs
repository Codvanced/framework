using System.Collections.Generic;
using IOC.FW.Core.Abstraction.Model;
using System.Data.Linq.Mapping;
using System.ComponentModel.DataAnnotations;

namespace IOC.Model
{
    [Table(Name = "Artist")]
    public class Artist
        : IModelCreating
    {
        [Key]
        public int IdArtist { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Genre> Genres { get; set; }

        public void OnCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Artist>()
                .HasMany(a => a.Genres)
                .WithMany(g => g.Artists)
                .Map(mp => {
                        mp.ToTable("ArtistGenre");
                        mp.MapLeftKey("IdArtist");
                        mp.MapRightKey("IdGenre");
                    }
                );
        }
    }
}
