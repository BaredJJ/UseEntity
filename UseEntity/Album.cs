namespace UseEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Album
    {
        [Key]
        public int AlbumsId { get; set; }

        public int ArtistId { get; set; }

        [Required]
        [StringLength(400)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateRelease { get; set; }

        public virtual Artist Artist { get; set; }

        public override string ToString() => Name + " " + DateRelease;
    }
}
