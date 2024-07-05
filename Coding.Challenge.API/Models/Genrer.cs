using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coding.Challenge.API.Models
{
    [Table("Genrer")]
    public class Genrer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [Column("content_input_id")]
        public Guid ContentInputId { get; set; }

        [ForeignKey("ContentInputId")]
        public ContentInput? ContentInput { get; set; }
    }
}
