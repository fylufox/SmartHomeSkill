using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using SmartHomeSkill.JSON.discovery;

#if DEBUG 
using SmartHomeSkill.env;
#endif

[assembly:LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace SmartHomeSkill
{
    public class Function
    {
        public JObject Handler(JObject input, ILambdaContext context)
        {
#if DEBUG
            Env env = new Env();
#endif
            string API_URL = Environment.GetEnvironmentVariable("API_URL");
            string USER = Environment.GetEnvironmentVariable("USER");
            string PASSWD = Environment.GetEnvironmentVariable("PASSWD");
            string ENDPOINT_ID = Environment.GetEnvironmentVariable("ENDPOINT_ID");
            string ENDPOINT_DESCRIPTION = Environment.GetEnvironmentVariable("ENDPOINT_DESCRIPTION");
            string ENDPOINT_FRIENDLY_NAME = Environment.GetEnvironmentVariable("ENDPOINT_FRIENDLY_NAME");
            string ENDPOINT_MANUFACTURER_NAME = Environment.GetEnvironmentVariable("ENDPOINT_MANUFACTURER_NAME");

            var url = Environment.GetEnvironmentVariable("URL");
            var user = Environment.GetEnvironmentVariable("USER");
            var passwd = Environment.GetEnvironmentVariable("PASSWD");
            Console.Out.WriteLine("Request:");
            Console.Out.WriteLine(input.ToString());

            AlexaResponse ar;

            var jsonRequest = input;
            string nameSpace = jsonRequest["directive"]["header"]["namespace"].Value<string>();
            switch (nameSpace)
            {

                case "Alexa.Authorization":
                    if (context != null)
                        Console.Out.WriteLine("Alexa.Authorization Request");
                    ar = new AlexaResponse("Alexa.Authorization", "AcceptGrant.Response");
                    break;

                case "Alexa.Discovery":
                    if (context != null)
                        Console.Out.WriteLine("Alexa.Discovery Request");

                    ar = new AlexaResponse("Alexa.Discovery", "Discover.Response", "endpoint-001");

                    // Create Discover.Response Payload
                    JObject payload = new JObject();
                    endpoint[] endpoints = new endpoint[1];
                    // --- endpoin-001 ---
                    endpoint endpoint1 = new endpoint();
                    endpoint1.endpointId = ENDPOINT_ID;
                    endpoint1.description = ENDPOINT_DESCRIPTION;
                    endpoint1.friendlyName = ENDPOINT_FRIENDLY_NAME;
                    endpoint1.manufacturerName = ENDPOINT_MANUFACTURER_NAME; // ÉÅÅ[ÉJÅ[ñº
                    string[] displayCategories = { "COMPUTER" };
                    endpoint1.displayCategories = displayCategories;

                    capabilities[] capabilities = new capabilities[2];
                    capabilities capabilitiesAlexa = new capabilities();
                    capabilitiesAlexa.type = "AlexaInterface";
                    capabilitiesAlexa.@interface = "Alexa";
                    capabilitiesAlexa.version = "3";

                    capabilities capabilitiesPowerController = new capabilities();
                    capabilitiesPowerController.type = "AlexaInterface";
                    capabilitiesPowerController.@interface = "Alexa.PowerController";
                    capabilitiesPowerController.version = "3";
                    Properties properties = new Properties();
                    properties.proactivelyReported = false;
                    properties.retrievable = false;
                    Supported[] supported = new Supported[1];
                    Supported supported1 = new Supported();
                    supported1.name = "powerState";
                    supported[0] = supported1;
                    properties.supported = supported;
                    capabilitiesPowerController.properties = properties;

                    capabilities[0] = capabilitiesAlexa;
                    capabilities[1] = capabilitiesPowerController;
                    endpoint1.capabilities = capabilities;
                    // --- END (endpoint-001) ---
                    endpoints[0] = endpoint1;
                    payload.Add("endpoints", JArray.FromObject(endpoints));

                    ar.Response["event"]["payload"] = payload;
                    break;

                case "Alexa.PowerController":
                    if (context != null)
                        Console.Out.WriteLine("Alexa.PowerController Request");
                    string correlationToken = jsonRequest["directive"]["header"]["correlationToken"].Value<string>();
                    string endpointId = jsonRequest["directive"]["endpoint"]["endpointId"].Value<string>();
                    string name = jsonRequest["directive"]["header"]["name"].Value<string>();

                    string state = (name == "TurnOff") ? "OFF" : "ON";

                    bool result = true; // check result TurnOff or TurnON.
                    if (result)
                    {
                        ar = new AlexaResponse("Alexa", "Response", endpointId, "INVALID", correlationToken);
                        ar.AddContextProperty("Alexa.PowerController", "powerState", state, 200);
                    }
                    else
                    {
                        JObject payloadError = new JObject();
                        payloadError.Add("type", "ENDPOINT_UNREACHABLE");
                        payloadError.Add("message", "There wa an error setting the device state.");
                        ar = new AlexaResponse("Alexa", "ErrorResponse");
                        ar.SetPayload(payloadError.ToString());
                    }

                    break;

                default:
                    if (context != null)
                        Console.Out.WriteLine("INVALID Namespace");
                    ar = new AlexaResponse();
                    break;
            }

            string response = ar.ToString();

            if (context != null)
            {
                Console.Out.WriteLine("Response:");
                Console.Out.WriteLine(response);
            }

            return JObject.Parse(response);
        }

    }
}
