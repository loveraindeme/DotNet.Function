using System.ComponentModel.DataAnnotations;

namespace DotNet.Internationalization.Dtos
{
    public class DataAnnotationsTestDto
    {
        [StringLength(64, MinimumLength = 2, ErrorMessage = "{0}长度只可在{2}到{1}个字符之间")]
        public string Name { get; set; } = null!;
    }
}
