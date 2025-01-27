using HealthManager.WebApp.BS.Entities.Models;
using NJsonSchema;
using NJsonSchema.Validation;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;


namespace Health.WebApp.BS.Shared.Schema
{
    public static class ValidateJSON
    {
        public static async Task<ICollection<ValidationError>> Validate(string jsonData, string schemaJson, JsonSchemaValidatorSettings? settings = null)
        {
            // Step 1: Normalize the JSON keys to match the schema
            var normalizedJson = NormalizeJsonKeysToSchema(jsonData);

            var schema = await JsonSchema.FromJsonAsync(schemaJson);
            var validator = new JsonSchemaValidator();
            var result = validator.Validate(normalizedJson, schema);
            // Step 2: Validate the normalized JSON against the schema
            var validationResults = schema.Validate(normalizedJson, settings);

            return result;
        }

        private static string NormalizeJsonKeysToSchema(string jsonData)
        {
            using var document = JsonDocument.Parse(jsonData);
            var normalizedJson = NormalizeElementKeys(document.RootElement);
            return JsonSerializer.Serialize(normalizedJson);
        }

        private static Dictionary<string, object?> NormalizeElementKeys(JsonElement element)
        {
            var normalizedDict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

            foreach (var property in element.EnumerateObject())
            {
                var normalizedKey = property.Name.ToLowerInvariant(); // Normalize to lowercase
                if (property.Value.ValueKind == JsonValueKind.Object)
                {
                    // Recursively normalize nested objects
                    normalizedDict[normalizedKey] = NormalizeElementKeys(property.Value);
                }
                else if (property.Value.ValueKind == JsonValueKind.Array)
                {
                    // Handle arrays
                    var normalizedArray = new List<object?>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        if (item.ValueKind == JsonValueKind.Object)
                        {
                            normalizedArray.Add(NormalizeElementKeys(item));
                        }
                        else
                        {
                            normalizedArray.Add(JsonSerializer.Deserialize<object>(item.GetRawText()));
                        }
                    }
                    normalizedDict[normalizedKey] = normalizedArray;
                }
                else
                {
                    normalizedDict[normalizedKey] = JsonSerializer.Deserialize<object>(property.Value.GetRawText());
                }
            }

            return normalizedDict;
        }
    }
}
