using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace PSJsonSchema
{
    [Cmdlet(VerbsDiagnostic.Test, "Json")]
    [OutputType(typeof(bool))]
    public class TestJson : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string [] Document { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string [] Schema { get; set; }


        protected override void EndProcessing()
        {
            string schemaDocument = string.Concat(Schema);
            JSchema schema = JSchema.Parse(schemaDocument);

            string jsonDocument = string.Concat(Document);
            object obj = JsonConvert.DeserializeObject(jsonDocument);

            if (obj is JObject)
            {
                WriteVerbose("Input document deserialized as json dictionary.");
                var jobj = obj as JObject;
                WriteObject(jobj.IsValid(schema));
            }

            else if (obj is JArray)
            {
                WriteVerbose("Input document deserialized as json array.");
                var jlist = obj as JArray;
                WriteObject(jlist.IsValid(schema));
            }
        }
    }
}
