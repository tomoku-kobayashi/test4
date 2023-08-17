using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ApiIsolated
{
    public class Insert
    {
        [Function("Insert")]
        public static async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "register-employee")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger<Insert>();
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse();

            try
            {
                using var reader = new StreamReader(req.Body);
                var requestBody = await reader.ReadToEndAsync();
                var employeeData = JsonSerializer.Deserialize<EmployeeData>(requestBody);

                // ÉfÅ[É^ÉxÅ[ÉXê⁄ë±ï∂éöóÒ
                var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "Insertè]ã∆àı001";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@è]ã∆àıCD", employeeData.è]ã∆àıÇbÇc);
                        command.Parameters.AddWithValue("@è]ã∆àıñº", employeeData.è]ã∆àıñº);

                        await command.ExecuteNonQueryAsync();
                    }
                }

                response.WriteString("Employee registered successfully.");
                response.Headers.Add("Content-Type", "text/plain");
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred");
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }
    }

    public class EmployeeData
    {
        public int è]ã∆àıÇbÇc { get; set; }
        public string è]ã∆àıñº { get; set; }
    }
}
