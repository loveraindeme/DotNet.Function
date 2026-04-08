using System.ComponentModel.DataAnnotations;

namespace DotNet.Internationalization.Dtos
{
    public class DataAnnotationsTestDto
    {
        [StringLength(64, MinimumLength = 2, ErrorMessage = "name-length-limit")]
        public string Name { get; set; } = null!;
    }
}
