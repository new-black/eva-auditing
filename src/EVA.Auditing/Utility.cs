using EVA.Core.Auditing.Compliancies.NF525;
using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EVA.Auditing
{
  public static class Utility
  {
    public static async Task<Result<ArchiveDto>> DownloadArchive(string url)
    {
      var result = new Result<ArchiveDto>();

      try
      {
        using (var sr = new StreamReader(await url.GetStreamAsync(), Encoding.Unicode))
        using (var j = new JsonTextReader(sr))
        {
          var serializer = new JsonSerializer();
          return result.Success(serializer.Deserialize<ArchiveDto>(j));
        }
      }
      catch (UriFormatException)
      {
        return result.Error("The url is not properly formatted.");
      }
      catch (FlurlHttpException)
      {
        return result.Error("Could not download file.");
      }
      catch (JsonReaderException)
      {
        return result.Error("File is not in expected JSON format.");
      }
      catch (Exception)
      {
        return result.Error("Something went wrong.");
      }
    }

    public static async Task<Result<string>> DownloadKey(string url)
    {
      var result = new Result<string>();

      try
      {
        return result.Success(await url.GetStringAsync());
      }
      catch (UriFormatException)
      {
        return result.Error("The url is not properly formatted.");
      }
      catch (FlurlHttpException)
      {
        return result.Error("Could not download file.");
      }
      catch (Exception)
      {
        return result.Error("Something went wrong.");
      }
    }

    public static async Task<Result> Verify(ArchiveDto archive, string publicKey)
    {
      var result = new Result(true);

      try
      {
        await NF525Signature.Verify(archive, publicKey, (dto) => result.Error(dto.ToString()).AsTask());
      }
      catch
      {
        result.Error("Could not verify signatures.");
      }

      return result;
    }
  }
}