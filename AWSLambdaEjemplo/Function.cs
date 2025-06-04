using System.Text;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using AWSLambdaEjemplo.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambdaEjemplo;

public class Function
{
 private readonly UTF8Encoding defaultEncoding =new(false);
  public async ValueTask<string> FunctionHandler(SQSEvent  evnt, ILambdaContext context)
    {
    S3Broker s3Broker = new S3Broker();
    string response = string.Empty;
    try
    {
      foreach (SQSEvent.SQSMessage record in evnt.Records)
      {
        SqsMessage sqsMessage = System.Text.Json.JsonSerializer.Deserialize<SqsMessage>(record.Body);
        context.Logger.LogLine($"Processing message: {record.Body}");
        // Here you can process each message as needed
        // For example, you could call another method to handle the message
        switch (sqsMessage.TypeProcess)
        {
          case "Upload":

           return await s3Broker.UploadTextObjectAsync(sqsMessage.BucketName, sqsMessage.key,sqsMessage.Text);
          case "Get":
          return await s3Broker.GetObjecAsync(sqsMessage.BucketName, sqsMessage.key);
          default:
            return await s3Broker.PutObjectAsync(sqsMessage.BucketName, sqsMessage.key, new MemoryStream(defaultEncoding.GetBytes(sqsMessage.Text)));
            
        }
      }
      return response;
    }
    catch (Exception ex)
    {
      context.Logger.LogLine($"Error message: {ex.Message}");
      throw;
    }
   
    }
}
