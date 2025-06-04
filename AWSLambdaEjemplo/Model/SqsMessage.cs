using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSLambdaEjemplo.Model
{
  public record SqsMessage
  {
    public string TypeProcess { get; init; }
    public string BucketName { get; init; }
    public string key { get; init; }
    public string Text { get; init; }
  }
}
