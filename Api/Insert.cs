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

                // �f�[�^�x�[�X�ڑ�������
                var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "Insert�]�ƈ�001";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@�]�ƈ�CD", employeeData.�]�ƈ��b�c);
                        command.Parameters.AddWithValue("@�]�ƈ���", employeeData.�]�ƈ���);

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
        public int �]�ƈ��b�c { get; set; }
        public string �]�ƈ��� { get; set; }
    }
}
