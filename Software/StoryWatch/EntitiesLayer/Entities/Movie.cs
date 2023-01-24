namespace EntitiesLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Movie : Media
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Movie()
        {
            MovieListItems = new HashSet<MovieListItem>();
            MovieListCategories = new HashSet<MovieListCategory>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(900)]
        public string Description { get; set; }

        [StringLength(100)]
        public string Trailer_URL { get; set; }

        [StringLength(100)]
        public string TMDB_ID { get; set; }

        [StringLength(100)]
        public string Countries { get; set; }

        [StringLength(100)]
        public string ReleaseDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MovieListItem> MovieListItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MovieListCategory> MovieListCategories { get; set; }
    }
}
