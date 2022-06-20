using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using EVA.Auditing;
using EVA.Auditing.NF525;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AuditingApp
{
    public partial class Form1 : Form
    {
        private List<EventDto> _fiscalArchiveEvents;

        public Form1()
        {
            InitializeComponent();
        }

        public void btnSaveAndClose_Click(object sender, EventArgs e) => HandleSaveAndClose(true, true);

        private void btnSave_Click(object sender, EventArgs e) => HandleSaveAndClose(true, false);

        private void btnClose_Click(object sender, EventArgs e) => HandleSaveAndClose(false, true);

        private void HandleSaveAndClose(bool save, bool close)
        {
            if (save)
            {

                // Create dialog
                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
                    Filter = "Excel|*.xlsx",
                    Title = "Save file"
                };

                // Open dialog
                saveFileDialog1.ShowDialog();

                // Save data when filename is present
                if (saveFileDialog1.FileName != "")
                {
                    if (File.Exists(saveFileDialog1.FileName))
                    {
                        File.Delete(saveFileDialog1.FileName);
                    }


                    using (FileStream fileStream = (FileStream)saveFileDialog1.OpenFile())
                    using (var spreadsheetDocument = SpreadsheetDocument.Create(fileStream, SpreadsheetDocumentType.Workbook))
                    {
                        WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
                        Workbook workbook = new Workbook();
                        workbookPart.Workbook = workbook;
                        Sheets sheets = new Sheets();
                        workbook.Append(sheets);
                        Sheet sheet = new Sheet() { SheetId = 1, Id = "rId1", Name = "Events", };
                        sheets.Append(sheet);
                        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>("rId1");
                        Worksheet worksheet = new Worksheet();
                        SheetData sheetData = new SheetData();
                        worksheet.Append(sheetData);
                        worksheetPart.Worksheet = worksheet;

                        UInt32Value rowIndex = 1;
                        Row row = new Row() { RowIndex = rowIndex };
                        row.Append(new Cell() { CellReference = $"A{rowIndex}", DataType = CellValues.String, CellValue = new CellValue($"Timestamp") });
                        row.Append(new Cell() { CellReference = $"B{rowIndex}", DataType = CellValues.String, CellValue = new CellValue($"ID") });
                        row.Append(new Cell() { CellReference = $"C{rowIndex}", DataType = CellValues.String, CellValue = new CellValue($"Type") });
                        row.Append(new Cell() { CellReference = $"D{rowIndex}", DataType = CellValues.String, CellValue = new CellValue($"EventCode") });
                        row.Append(new Cell() { CellReference = $"E{rowIndex}", DataType = CellValues.String, CellValue = new CellValue($"EventDescription") });
                        row.Append(new Cell() { CellReference = $"F{rowIndex}", DataType = CellValues.String, CellValue = new CellValue($"Information") });

                        sheetData.Append(row);

                        foreach (var fiscalArchiveEvent in _fiscalArchiveEvents)
                        {
                            rowIndex++;

                            row = new Row() { RowIndex = rowIndex };
                            row.Append(new Cell() { CellReference = $"A{rowIndex}", DataType = CellValues.String, CellValue = new CellValue($"{fiscalArchiveEvent.Timestamp:yyyy-MM-dd HH:mm:ss}") });
                            row.Append(new Cell() { CellReference = $"B{rowIndex}", DataType = CellValues.String, CellValue = new CellValue($"{fiscalArchiveEvent.ID}") });
                            row.Append(new Cell() { CellReference = $"C{rowIndex}", DataType = CellValues.String, CellValue = new CellValue($"{fiscalArchiveEvent.Type}") });
                            row.Append(new Cell() { CellReference = $"D{rowIndex}", DataType = CellValues.String, CellValue = new CellValue($"{fiscalArchiveEvent.EventCode}") });
                            row.Append(new Cell() { CellReference = $"E{rowIndex}", DataType = CellValues.String, CellValue = new CellValue($"{fiscalArchiveEvent.EventDescription}") });
                            row.Append(new Cell() { CellReference = $"F{rowIndex}", DataType = CellValues.String, CellValue = new CellValue($"{fiscalArchiveEvent.Information}") });

                            sheetData.Append(row);
                        }


                        workbookPart.Workbook.Save();
                        spreadsheetDocument.Close();
                    }
                }
            }

            if (close)
            {
                this.Close();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var loadSuccessfull = true;
            ArchiveDto fiscalArchiveData = default;
            string publicKeyData = default;

            btnLoad.Enabled = false;
            btnSave.Enabled = false;
            btnSaveAndClose.Enabled = false;
            btnClose.Enabled = false;

            tbProgress.Clear();
            tbProgress.Text += $"Starting process ({DateTime.Now:yyyy-MM-dd HH:mm:ss})." + Environment.NewLine + Environment.NewLine;

            try 
            { 
                if(string.IsNullOrWhiteSpace(tbFicalArchiveURL.Text))
                {
                    tbProgress.Text += "The archive fiscal URL is required." + Environment.NewLine;
                    loadSuccessfull = false;
                    return;
                }

                if (Uri.TryCreate(tbFicalArchiveURL.Text, UriKind.Absolute, out var fiscalArchiveURL) && (fiscalArchiveURL.Scheme == Uri.UriSchemeHttp || fiscalArchiveURL.Scheme == Uri.UriSchemeHttp))
                {
                    tbProgress.Text += "The archive fiscal URL is invalid. Process requires a valid http(s) URL." + Environment.NewLine;
                    loadSuccessfull = false;
                    return;
                }

                if (string.IsNullOrWhiteSpace(tbPublicKeyURL.Text))
                {
                    tbProgress.Text += "The public key URL is required." + Environment.NewLine;
                    loadSuccessfull = false;
                    return;
                }

                if (Uri.TryCreate(tbPublicKeyURL.Text, UriKind.Absolute, out var publicKeyURL) && (publicKeyURL.Scheme == Uri.UriSchemeHttp || publicKeyURL.Scheme == Uri.UriSchemeHttp))
                {
                    tbProgress.Text += "The public key URL is invalid. Process requires a valid http(s) URL." + Environment.NewLine;
                    loadSuccessfull = false;
                    return;
                }

                // Get fiscal archive
                await Task.WhenAll(
                    Utility.DownloadArchive(fiscalArchiveURL.AbsoluteUri).ContinueWith(t =>
                    {
                        if (!t.Result.IsSuccessful)
                        {
                            tbProgress.Text += "The download of the fiscal archive failed with the following error:" + Environment.NewLine;
                            tbProgress.Text += t.Result.Message + Environment.NewLine;
                            loadSuccessfull = false;
                        }

                        fiscalArchiveData = t.Result.Data;
                    }),
                    Utility.DownloadKey(publicKeyURL.AbsoluteUri).ContinueWith(t =>
                    {
                        if (!t.Result.IsSuccessful)
                        {
                            tbProgress.Text += "The download of the public key failed with the following error:" + Environment.NewLine;
                            tbProgress.Text += t.Result.Message + Environment.NewLine;
                            loadSuccessfull = false;
                            return;
                        }

                        publicKeyData = t.Result.Data;
                    })
                );

                if (loadSuccessfull)
                {
                    // Validate
                    var validationResult = Utility.Verify(fiscalArchiveData, publicKeyData).Result;
                    if (!validationResult.IsSuccessful)
                    {
                        tbProgress.Text += "Fiscal archive contains errors:" + Environment.NewLine;
                        foreach (var error in validationResult.Errors)
                        {
                            tbProgress.Text += $"- {error}" + Environment.NewLine;
                        }
                    }

                    this._fiscalArchiveEvents = fiscalArchiveData.Events;
                    loadSuccessfull = true;
                }
            }
            catch(Exception excep)
            {
                var exceptionId = DateTime.Now.ToString("yyyyMMddhhmmss") + Guid.NewGuid().ToString().Split('-')[0];

                File.AppendAllLines("ErrorLog.log", new List<string>() { $"{DateTime.Now:yyyy-MM-dd hh:mm:ss};UnexpectedError;{excep.Message}" });
                tbProgress.Text += $"An unexpected error occured. See error log for details (exceptionId '{exceptionId}')";
            }
            finally
            {
                tbProgress.Text += Environment.NewLine + $"Process completed ({DateTime.Now:yyyy-MM-dd HH:mm:ss}).";
                btnLoad.Enabled = true;
                btnClose.Enabled = true;

                if (loadSuccessfull)
                {
                    btnSave.Enabled = true;
                    btnSaveAndClose.Enabled = true;
                }
            }
        }
    }
}
