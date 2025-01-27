using System.Reflection;


namespace Health.WebApp.BS.Shared.Schema
{
    public static class JsonSchemaHelper
    {
        public static string GetJsonSchema()
        {
            // Get the embedded resource stream
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Health.WebApp.BS.Shared.metadata-schema.json";

            var allResourceNames = assembly.GetManifestResourceNames();
            //Selecting first one. 
            var resourceNam = allResourceNames[0];
            var pathToFile = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) +
                              resourceName;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd(); 
            }
        }
    }
}
