using System;
using System.Collections.Generic;

namespace EVA.Auditing.NF525
{
    public class ArchiveDto : BaseSignatureDto
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string TerminalID { get; set; }
        public DateTime Timestamp { get; set; }
        public IDictionary<decimal, decimal> TotalsPerVAT { get; set; }
        public decimal TotalAmount { get; set; }

        public List<TicketDto> Tickets { get; set; } = new List<TicketDto>();
        public List<InvoiceDto> Invoices { get; set; } = new List<InvoiceDto>();
        public List<AddressDto> LegalEntities { get; set; } = new List<AddressDto>();
        public List<AddressDto> NaturalPersons { get; set; } = new List<AddressDto>();
        public List<EventDto> Events { get; set; } = new List<EventDto>();
        public List<TotalDto> Totals { get; set; } = new List<TotalDto>();

        public override string ToString() => $"Archive {ID} {Type}";
    }

    public class TicketDto : BaseSignatureDto
    {
        public string ID { get; set; }
        public string DocumentNumber { get; set; }
        public string SoftwareVersion { get; set; }
        public int PrintNumber { get; set; }
        public string SellerID { get; set; }
        public string SellerName { get; set; }
        public string OperatorID { get; set; }
        public string OperatorName { get; set; }
        public string TerminalID { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime? DocumentDate { get; set; }
        public OperationTypes OperationType { get; set; }
        public DocumentTypes DocumentType { get; set; }
        public int NumberOfLines { get; set; }
        public int NumberOfCustomers { get; set; }
        public string TicketStatus { get; set; }
        public string SignatureReturn { get; set; }
        public decimal TotalAmountExTax { get; set; }
        public decimal TotalAmountInTax { get; set; }

        public AddressDto Company { get; set; }
        public List<TicketLineDto> Lines { get; set; }
        public List<VATTotalDto> TotalsPerVAT { get; set; }
        public List<PaymentDto> Payments { get; set; }
        public List<DuplicateDto> Duplicates { get; set; }

        public override string ToString() => $"Ticket {ID} {DocumentNumber}";
    }

    public class TicketLineDto
    {
        public string LineNumber { get; set; }
        public string ProductID { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string VATCode { get; set; }
        public decimal VATRate { get; set; }
        public decimal UnitPriceExTax { get; set; }
        public decimal UnitPriceInTax { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal TotalAmountExTax { get; set; }
        public decimal TotalAmountInTax { get; set; }
        public DateTime Date { get; set; }
    }

    public class VATTotalDto
    {
        public string VATCode { get; set; }
        public decimal VATRate { get; set; }
        public decimal VATAmount { get; set; }
        public decimal TotalAmountExTax { get; set; }
        public decimal TotalAmountInTax { get; set; }
    }

    public class PaymentDto
    {
        public string PaymentMethod { get; set; }
        public string PaymentType { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyID { get; set; }
        public decimal ExchangeRate { get; set; }
        public OperationTypes OperationType { get; set; }
        public string UserID { get; set; }
        public DateTime Date { get; set; }
    }

    public class DuplicateDto : BaseSignatureDto
    {
        public int ID { get; set; }
        public long ReprintNumber { get; set; }
        public string Status { get; set; }
        public int DocumentType { get; set; }
        public string DocumentID { get; set; }
        public string OperatorID { get; set; }
        public DateTime Timestamp { get; set; }

        public override string ToString() => $"Duplicate {ID} {ReprintNumber} of {DocumentType} {DocumentID}";
    }

    public class InvoiceDto : BaseSignatureDto
    {
        public string ID { get; set; }
        public string DocumentNumber { get; set; }
        public DocumentTypes DocumentType { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime Timestamp { get; set; }
        public string CustomerID { get; set; }
        public string OperatorID { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public decimal TotalDiscountAmountExTax { get; set; }
        public decimal TotalShippingAmount { get; set; }
        public decimal TotalAmountExTax { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal TotalAmountInTax { get; set; }
        public string SignatureReturn { get; set; }

        public AddressDto Company { get; set; }
        public List<InvoiceLineDto> Lines { get; set; }
        public List<PaymentDto> Payments { get; set; }
        public List<DuplicateDto> Duplicates { get; set; }
        public List<VATTotalDto> TotalsPerVAT { get; set; }

        public override string ToString() => $"Invoice {ID} {DocumentNumber}";
    }

    public class InvoiceLineDto
    {
        public string LineNumber { get; set; }
        public string ProductID { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPriceExTax { get; set; }
        public decimal UnitPriceInTax { get; set; }
        public string VATCode { get; set; }
        public decimal VATRate { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal TotalAmountExTax { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal TotalAmountInTax { get; set; }
    }

    public class AddressDto
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        /// <summary>
        /// <see cref="OrganizationUnit.VatNumber"/>
        /// </summary>
        public string VatNumber { get; set; }

        /// <summary>
        /// <see cref="OrganizationUnit.RegistrationNumber"/>
        /// </summary>
        public string SIRET { get; set; }

        /// <summary>
        /// Every business in France is classified under an activity code entitled APE - Activite Principale de
        /// l’Entreprise - or NAF code. When registering your company, you will need to indicate which is your main
        /// activity using an APE code. Here is the APE list in English up-dated by the national Institute of statistics INSEE.
        /// For example, if you sell online your APE code would be 97.91B - vente a distance sur catalogue specialise.
        /// Another example, if you plan to become an IT consultant specialised in hardware and software your APE code would
        /// be 62.02 - conseils en systemes informatiques.
        /// https://www.startbusinessinfrance.com/blog/post/tip-what-is-your-ape-code
        /// <see cref="OrganizationUnit.BranchNumber"/>
        /// </summary>
        public string NAF { get; set; }

        public AddressDto ShippingAddress { get; set; }
    }

    public class EventDto : BaseSignatureDto
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string EventCode { get; set; }
        public string EventDescription { get; set; }
        public string OperatorID { get; set; }
        public string TerminalID { get; set; }
        public DateTime Timestamp { get; set; }
        public string Information { get; set; }

        public override string ToString() => $"Event {ID} {Type}";
    }

    public class TotalDto : BaseSignatureDto
    {
        public string ID { get; set; }
        public TotalTypes Type { get; set; }
        public string DocumentID { get; set; }
        public DocumentTypes DocumentType { get; set; }
        public List<VATTotalDto> TotalsPerVAT { get; set; }
        public decimal CumulativeGrandTotal { get; set; }
        public decimal CumulativePerpetualGrandTotalRealValue { get; set; }
        public decimal CumulativePerpetualGrandTotalAbsoluteValue { get; set; }
        public DateTime Timestamp { get; set; }

        public override string ToString() => $"Total {ID} {Type} {DocumentType}";
    }

    public class BaseSignatureDto : ISignatureDto
    {
        public string Signature { get; set; }
        public string PreviousSignature { get; set; }
    }

    public interface ISignatureDto
    {
        string Signature { get; set; }
        string PreviousSignature { get; set; }
    }

    public enum OperationTypes
    {
        Sales,
        Return,
        Archiving
    }

    public enum DocumentTypes
    {
        Ticket,
        Invoice,
        CreditNote,
        Totals
    }

    public enum TotalTypes
    {
        GrandTotal,
        PeriodGrandTotal,
        MonthlyGrandTotal,
        FiscalYearGrandTotal
    }
}
