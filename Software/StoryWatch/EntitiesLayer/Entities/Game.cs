namespace EntitiesLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Game : Media
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Game()
        {
            GameListItems = new HashSet<GameListItem>();
            GameListCategories = new HashSet<GameListCategory>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(900)]
        public string Summary { get; set; }

        [StringLength(100)]
        public string Company { get; set; }
        
        [StringLength(100)]
        public string Release_Date { get; set; }

        [StringLength(200)]
        public string Genres { get; set; }

        [StringLength(100)]
        public string IGDB_Id { get; set; }

        public virtual Genre Genre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GameListItem> GameListItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GameListCategory> GameListCategories { get; set; }
    }
}
