using CafeErez.Shared.Model.Identity;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System.Data;
using System.Diagnostics;
using System.Drawing;

namespace BusinessService.Reports
{
    public class PdfService
    {
        public MemoryStream CreatePdf(List<UserResponse> users)
        {
            using (PdfDocument document = new PdfDocument())
            {
                //Add a page
                PdfPage page = document.Pages.Add();

                //Create a PdfGrid
                PdfGrid pdfGrid = new PdfGrid();

                //Create a DataTable
                DataTable dataTable = new DataTable();

                //Add columns to the DataTable
                dataTable.Columns.Add("First Name");
                dataTable.Columns.Add("Last Name");
                dataTable.Columns.Add("User Name");
                dataTable.Columns.Add("Email");
                dataTable.Columns.Add("Phone Number");
                dataTable.Columns.Add("Email Confirmation");

                foreach (UserResponse user in users)
                {
                    dataTable.Rows.Add(new object[] { user.FirstName, user.LastName, user.UserName, user.Email, user.PhoneNumber, user.EmailConfirmed.ToString()});
                }

                //Assign data source
                pdfGrid.DataSource = dataTable;
                //Create string format for PdfGrid
                PdfStringFormat format = new PdfStringFormat();
                format.Alignment = PdfTextAlignment.Center;
                format.LineAlignment = PdfVerticalAlignment.Bottom;
                //Assign string format for each column in PdfGrid
                foreach (PdfGridColumn column in pdfGrid.Columns)
                    column.Format = format;
                //Apply a built-in style
                pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent6);
                //Set properties to paginate the grid
                PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
                layoutFormat.Break = PdfLayoutBreakType.FitPage;
                layoutFormat.Layout = PdfLayoutType.Paginate;
                //Draw grid to the page of PDF document
                pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10,10), layoutFormat);

                MemoryStream ms = new MemoryStream();
                ms.Position = 0;

                document.Save(ms);
                return ms;


                //pdfGrid.DataSource = dataTable;

                ////Draw grid to the page of PDF document
                //pdfGrid.Draw(page, new PointF(10, 10));

                ////Save the document
                //document.Save("Output.pdf");

                //MemoryStream ms = new MemoryStream();
                //ms.Position = 0;

                //document.Save(ms);
                //return ms;
            }
        }
    }
}
