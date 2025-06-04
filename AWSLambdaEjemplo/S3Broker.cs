using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace AWSLambdaEjemplo
{
  public class S3Broker
  {
    private readonly AmazonS3Client client = new();

    public async ValueTask<string> GetObjecAsync(string bucketName, string key)
    {
      string contenidoTxt = string.Empty;
      if (string.IsNullOrEmpty(bucketName)) throw new ArgumentNullException(nameof(bucketName));
      if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
      var request = new GetObjectRequest
      {
        BucketName = bucketName,
        Key = key
      };

      using (GetObjectResponse response = await client.GetObjectAsync(request))
      using (var reader = new StreamReader(response.ResponseStream, Encoding.UTF8))
      {
        return contenidoTxt = reader.ReadToEndAsync().Result;
      }
    }

    public async ValueTask<string> UploadTextObjectAsync(string bucketName, string key, string text)
    {
      if (string.IsNullOrEmpty(bucketName)) throw new ArgumentNullException(nameof(bucketName));
      if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
      if (text == null) throw new ArgumentNullException(nameof(text));
      using var stream = new MemoryStream(Encoding.UTF8.GetBytes(text));
     return  await PutObjectAsync(bucketName, key, stream);
    }

    public async ValueTask<string> PutObjectAsync(string bucketName, string key, Stream stream)
    {
      if (string.IsNullOrEmpty(bucketName)) throw new ArgumentNullException(nameof(bucketName));
      if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
      if (stream == null) throw new ArgumentNullException(nameof(stream));
      var response = await client.PutObjectAsync(new PutObjectRequest { BucketName = bucketName, Key = key, InputStream = stream });

      if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
      {
        return $"Object {key} uploaded successfully to bucket {bucketName}.";
      }
      else
      {
        throw new Exception($"Failed to upload object {key} to bucket {bucketName}. Status code: {response.HttpStatusCode}");
      }
    }
  }
}