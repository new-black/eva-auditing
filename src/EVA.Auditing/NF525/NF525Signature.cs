using EVA.Auditing.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EVA.Auditing.NF525
{
    public static class NF525Signature
    {
        public static NF525SignatureBuilder Create(string currencyID, InvoiceDto dto, AddressDto legalEntity)
        {
            return new NF525SignatureBuilder()
              .AddTaxes(currencyID, dto.TotalsPerVAT.Select(l => (l.VATRate, l.TotalAmountInTax)))
              .AddAmount(currencyID, dto.TotalAmountInTax)
              .Add(dto.Timestamp)
              .Add(dto.DocumentNumber)
              .Add((int)dto.DocumentType)
              .Add(legalEntity?.Name ?? string.Empty)
              .Add(legalEntity?.ZipCode ?? "N/A")
              .Add(legalEntity?.VatNumber ?? string.Empty)
              .AddSignature(dto.PreviousSignature);
        }

        public static NF525SignatureBuilder Create(string currencyID, TicketDto dto)
        {
            return new NF525SignatureBuilder()
              .AddTaxes(currencyID, dto.TotalsPerVAT.Select(l => (l.VATRate, l.TotalAmountInTax)))
              .AddAmount(currencyID, dto.TotalAmountInTax)
              .Add(dto.Timestamp)
              .Add(dto.DocumentNumber)
              .Add((int)dto.OperationType)
              .AddSignature(dto.PreviousSignature);
        }

        public static NF525SignatureBuilder Create(string currencyID, ArchiveDto dto)
        {
            return new NF525SignatureBuilder()
              .AddTaxes(currencyID, dto.TotalsPerVAT.Select(x => (x.Key, x.Value)))
              .AddAmount(currencyID, dto.TotalAmount)
              .Add(dto.Timestamp)
              .Add(dto.TerminalID)
              .Add(dto.Type)
              .AddSignature(dto.PreviousSignature);
        }

        public static NF525SignatureBuilder Create(EventDto dto)
        {
            return new NF525SignatureBuilder()
              .Add(dto.ID.ToString())
              .Add(dto.EventCode)
              .Add(dto.Information.DefaultIfNullOrEmpty(dto.EventDescription))
              .Add(dto.Timestamp)
              .Add(dto.OperatorID)
              .Add(dto.TerminalID)
              .AddSignature(dto.PreviousSignature);
        }

        public static NF525SignatureBuilder Create(string currencyID, TotalDto dto)
        {
            return new NF525SignatureBuilder()
              .AddTaxes(currencyID, dto.TotalsPerVAT.GroupBy(x => x.VATRate).Select(x => (x.Key, x.Sum(t => t.TotalAmountInTax))))
              .AddAmount(currencyID, dto.CumulativeGrandTotal)
              .AddAmount(currencyID, dto.CumulativePerpetualGrandTotalRealValue)
              .Add(dto.Timestamp)
              .Add(dto.DocumentID)
              .AddSignature(dto.PreviousSignature);
        }

        public static NF525SignatureBuilder Create(DuplicateDto dto)
        {
            return new NF525SignatureBuilder()
              .Add(dto.ID)
              .Add(dto.DocumentType)
              .Add(dto.ReprintNumber)
              .Add(dto.OperatorID)
              .Add(dto.Timestamp)
              .Add(dto.DocumentID)
              .AddSignature(dto.PreviousSignature);
        }

        public static async Task Verify(string currencyID, ArchiveDto archive, string publicKey, Func<ISignatureDto, Task> failureFunc)
        {
            Task Verify(ISignatureDto dto, NF525SignatureBuilder b) => !b.Verify(dto.Signature, publicKey) ? failureFunc(dto) : Task.CompletedTask;
            AddressDto Customer(InvoiceDto invoice) => archive.LegalEntities.FirstOrDefault(x => x.ID == invoice.CustomerID) ?? archive.NaturalPersons.FirstOrDefault(x => x.ID == invoice.CustomerID);

            await Verify(archive, Create(currencyID, archive));

            // foreach (var x in archive.Tickets) await Verify(x, Create(currencyID, x));
            // foreach (var x in archive.Invoices) await Verify(x, Create(currencyID, x, Customer(x)));
            // foreach (var x in archive.Tickets.SelectMany(x => x.Duplicates)) await Verify(x, Create(x));
            // foreach (var x in archive.Invoices.SelectMany(x => x.Duplicates)) await Verify(x, Create(x));
            // foreach (var x in archive.Events) await Verify(x, Create(x));
            // foreach (var x in archive.Totals) await Verify(x, Create(currencyID, x));
        }
    }

    public class RSAParametersJson
    {
        public string Modulus { get; set; }
        public string Exponent { get; set; }
        public string P { get; set; }
        public string Q { get; set; }
        public string DP { get; set; }
        public string DQ { get; set; }
        public string InverseQ { get; set; }
        public string D { get; set; }
    }

    public class NF525SignatureBuilder
    {
        public List<string> Segments { get; set; }

        public NF525SignatureBuilder()
        {
            Segments = new List<string>();
        }

        public NF525SignatureBuilder Add(string str) => this.With(x => x.Segments.Add(str.Replace(",", ";")));
        public NF525SignatureBuilder Add(DateTime dateTime) => this.With(x => x.Segments.Add(dateTime.ToString("yyyyMMddHHmmss")));
        public NF525SignatureBuilder Add(int i) => this.With(x => x.Segments.Add(i.ToString()));
        public NF525SignatureBuilder Add(long i) => this.With(x => x.Segments.Add(i.ToString()));
        public NF525SignatureBuilder AddTaxes(string currencyID, IEnumerable<(decimal TaxRate, decimal Total)> amounts) => this.With(x => x.Segments.Add(amounts.OrderByDescending(a => a.TaxRate).Select(g => $"{TaxRate(g.TaxRate)}:{AmountToCents(currencyID, g.Total)}").JoinBy("|")));
        public NF525SignatureBuilder AddAmount(string currencyID, decimal d) => this.With(x => x.Segments.Add(AmountToCents(currencyID, d)));
        public NF525SignatureBuilder AddSignature(string s) => this.With(x => x.Segments.Add(s != null ? "1" : "0"), x => x.Segments.Add(s ?? string.Empty));

        public static byte[] Get(string s) => s != null ? Convert.FromBase64String(s) : null;
        public static string Get(byte[] b) => b != null ? Convert.ToBase64String(b) : null;
        public static string AmountToCents(string currencyID, decimal amount) => ((int)(amount.RoundFor(currencyID) * 100)).ToString(CultureInfo.InvariantCulture);
        public static string TaxRate(decimal taxRate) => ((taxRate - 1) * 10_000).ToString("0000");

        public string Sign(string privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                var json = Encoding.UTF8.GetString(Convert.FromBase64String(privateKey));
                var obj = JsonConvert.DeserializeObject<RSAParametersJson>(json);

                rsa.ImportParameters(new RSAParameters
                {
                    Modulus = Get(obj.Modulus),
                    Exponent = Get(obj.Exponent),
                    P = Get(obj.P),
                    Q = Get(obj.Q),
                    DP = Get(obj.DP),
                    DQ = Get(obj.DQ),
                    InverseQ = Get(obj.InverseQ),
                    D = Get(obj.D),
                });

                var data = Segments.Select(s => s.Replace(",", ";")).JoinBy(",");
                var bytes = Encoding.UTF8.GetBytes(data);
                var result = rsa.SignData(bytes, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);

                return Convert.ToBase64String(result).AsUrlSafe();
            }
        }

        public bool Verify(string signature, string publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                var json = Encoding.UTF8.GetString(Convert.FromBase64String(publicKey));
                var obj = JsonConvert.DeserializeObject<RSAParametersJson>(json);

                rsa.ImportParameters(new RSAParameters
                {
                    Modulus = Get(obj.Modulus),
                    Exponent = Get(obj.Exponent),
                    P = Get(obj.P),
                    Q = Get(obj.Q),
                    DP = Get(obj.DP),
                    DQ = Get(obj.DQ),
                    InverseQ = Get(obj.InverseQ),
                    D = Get(obj.D),
                });

                var data = Segments.Select(s => s.Replace(",", ";")).JoinBy(",");
                var bytes = Encoding.UTF8.GetBytes(data);
                var signatureBytes = Convert.FromBase64String(signature.FromUrlSafe());

                return rsa.VerifyData(bytes, signatureBytes, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            }
        }

        // unused, but in case we want to generate a new set - this is handy
        public static (string publicKey, string privateKey) GenerateKeys(int keySize = 1024)
        {
            string Export(RSAParameters obj) => Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new RSAParametersJson
            {
                Modulus = Get(obj.Modulus),
                Exponent = Get(obj.Exponent),
                P = Get(obj.P),
                Q = Get(obj.Q),
                DP = Get(obj.DP),
                DQ = Get(obj.DQ),
                InverseQ = Get(obj.InverseQ),
                D = Get(obj.D),
            })));

            using (var rsa = new RSACryptoServiceProvider(keySize))
            {
                return (publicKey: Export(rsa.ExportParameters(false)), privateKey: Export(rsa.ExportParameters(true)));
            }
        }
    }
}
