using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DotNet.VersionControl
{
    /// <summary>
    /// 从Swagger文档中移除版本参数
    /// </summary>
    public sealed class RemoveVersionOperationFilter : IOperationFilter
    {
        private static readonly HashSet<string> VersionParameterNames =
            new(StringComparer.OrdinalIgnoreCase)
            {
                "api-version",
                "version",
                "ApiVersion"
            };

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters is null || operation.Parameters.Count == 0)
            {
                return;
            }

            var versionParameters = operation.Parameters
                .Where(p => p.Name is not null && VersionParameterNames.Contains(p.Name))
                .ToList();

            foreach (var versionParameter in versionParameters)
            {
                operation.Parameters.Remove(versionParameter);
            }
        }
    }
}
