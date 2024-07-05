using Coding.Challenge.Dependencies.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Coding.Challenge.API.Models
{
    [Table("content_input")]
    public class ContentInput
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int? Duration { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        [NotMapped]
        [JsonIgnore]
        public ICollection<Genrer>? Genrers { get; set; }

        public ContentDto ToDto()
        {
            return new ContentDto(
                Title,
                SubTitle,
                Description,
                ImageUrl,
                Duration,
                StartTime,
                EndTime,
                new List<string>()
            );
        }
    }
}
