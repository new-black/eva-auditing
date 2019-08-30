using EVA.Auditing;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
  public class Program
  {
    public static string[] Title = new string[]
    {
      "   __    _  _______  _     _    _______  ___      _______  _______  ___   _  ",
      "  |  |  | ||       || | _ | |  |  _    ||   |    |   _   ||       ||   | | | ",
      "  |   |_| ||    ___|| || || |  | |_|   ||   |    |  |_|  ||       ||   |_| | ",
      "  |       ||   |___ |       |  |       ||   |    |       ||       ||      _| ",
      "  |  _    ||    ___||       |  |  _   | |   |___ |       ||      _||     |_  ",
      "  | | |   ||   |___ |   _   |  | |_|   ||       ||   _   ||     |_ |    _  | ",
      "  |_|  |__||_______||__| |__|  |_______||_______||__| |__||_______||___| |_| "
    };

    public static async Task Main()
    {
      Console.WriteLine(string.Join(Environment.NewLine, Title));

      Console.WriteLine();
      Typewriter(@"  NF525 validateur de archive fiscal v0.1, 2019");

      Console.WriteLine();
      Console.Write("  Archive fiscal url: ");

      var auditFile = await Utility.DownloadArchive(Console.ReadLine());
      {
        if (!auditFile.IsSuccessful)
        {
          Console.WriteLine();
          Typewriter($"  Error: {auditFile.Message}", @"V.V");
          Typewriter(@"  Press <enter> to exit.");

          Console.ReadLine();
          return;
        }
      }

      Console.WriteLine();
      Console.Write("  Public key url: ");

      var publicKey = await Utility.DownloadKey(Console.ReadLine());
      {
        if (!publicKey.IsSuccessful)
        {
          Console.WriteLine();
          Typewriter($"  Error: {publicKey.Message}", @"V.V");
          Typewriter(@"  Press <enter> to exit.");

          Console.ReadLine();
          return;
        }
      }

      Console.WriteLine();
      Typewriter("  ## VALIDATION RESULTS ##");

      var validationResult = await Utility.Verify(auditFile.Data, publicKey.Data);
      {
        if (validationResult.IsSuccessful)
        {
          Console.WriteLine();
          Typewriter(@"  All signatures were verified successfully!");
        }
        else
        {
          Console.WriteLine();
          Typewriter(@"  Fiscal archive contains errors:");

          Console.WriteLine();
          foreach (var error in validationResult.Errors) Console.WriteLine($"  - {error}");
        }
      }

      Console.WriteLine();
      Typewriter("  ## EVENT LOG (JET) ##");

      Console.WriteLine();

      foreach (var e in auditFile.Data.Events)
      {
        Console.WriteLine($"  [{e.Timestamp:yyyy-MM-dd HH:mm:ss}] {e.ID} {e.Type} {e.EventCode} '{e.EventDescription}' ({e.Information})");
      }

      Console.WriteLine();

      Typewriter(@"  Completed.");
      Typewriter(@"  Press <enter> to exit.");

      Console.ReadLine();
    }

    public static void Typewriter(string s, string cursor = @"\o/")
    {
      var top = Console.CursorTop;
      var text = s.PadRight(Title[0].Length);

      for (var i = 0; i < text.Length; i++)
      {
        Console.SetCursorPosition(0, top);
        Console.Write(text.Substring(0, i));
        Console.Write(cursor);
        Thread.Sleep(10);
      }

      Console.SetCursorPosition(text.Length - 1, top);
      Console.WriteLine(string.Empty.PadRight(cursor.Length + 1));
    }
  }
}