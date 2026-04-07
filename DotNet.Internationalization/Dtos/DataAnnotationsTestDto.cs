using System.ComponentModel.DataAnnotations;

namespace DotNet.Internationalization.Dtos
{
    public class DataAnnotationsTestDto
    {
        [Length(2, 64, ErrorMessage = "name-length-limit")]
        public string Name { get; set; } = null!;
    }
}
