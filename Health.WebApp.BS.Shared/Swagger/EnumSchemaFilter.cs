using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthManager.WebApp.BS.Shared.Swagger
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            OpenApiSchema model2 = model;
            if (context.Type.IsEnum)
            {
                model2.Enum.Clear();
                Enum.GetNames(context.Type).ToList().ForEach(delegate (string n)
                {
                    model2.Enum.Add(new OpenApiString(n));
                    model2.Type = "string";
                    model2.Format = null;
                });
            }
        }
    }
}
