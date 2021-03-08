using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaDATAPARSER
{
    public class ReportGenerator
    {
        public MemoryStream CreatePdf(string imageURL)
        {
            var workStream = new MemoryStream();
            var doc = new Document(PageSize.A4, 10f, 10f, 20f, 0f);

            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            Image jpg = Image.GetInstance(imageURL);
            //Resize image depend upon your need
            jpg.ScaleToFit(220, 200);

            jpg.Alignment = Element.ALIGN_LEFT;
            doc.Add(jpg);

            var tableLayout1 = new PdfPTable(15);
            tableLayout1.AddCell(new PdfPCell(new Phrase("hollow text",
                new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(255, 255, 255))))
            { Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, Colspan = 100 });
            doc.Add(tableLayout1);

            Paragraph paragraph = new Paragraph("Reporte Diario de Activos. Fecha: " + DateTime.Today.ToShortDateString());
            doc.Add(paragraph);

            //Add Content to PDF
            //Create PDF Table with 15 columns  
            var tableLayout2 = new PdfPTable(15);
            doc.Add(AddContent(tableLayout2));

            // Closing the document  
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return workStream;
        }

        private PdfPTable AddContent(PdfPTable tableLayout)
        {
            float[] headers = { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;

            List<Tuple<int, string, string, string, DateTime>> employees = this.GetEmployees();

            tableLayout.AddCell(new PdfPCell(new Phrase("hollow text",
                new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(255, 255, 255))))
            { Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT, Colspan = 100 });

            ////Add header  
            AddCellToHeader(tableLayout, "Activo");
            AddCellToHeader(tableLayout, "EMA 20");
            AddCellToHeader(tableLayout, "RSI 14");
            AddCellToHeader(tableLayout, "Boolinger 20");
            AddCellToHeader(tableLayout, "MACD");
            AddCellToHeader(tableLayout, "SO FAST");
            AddCellToHeader(tableLayout, "Momentum 12");
            AddCellToHeader(tableLayout, "MA 20");
            AddCellToHeader(tableLayout, "SO SLOW");
            AddCellToHeader(tableLayout, "ROC 12");
            AddCellToHeader(tableLayout, "WilliamsR 14");
            AddCellToHeader(tableLayout, "OBV");
            AddCellToHeader(tableLayout, "VO 14-28");
            AddCellToHeader(tableLayout, "CM 50");
            AddCellToHeader(tableLayout, "CM 200");

            //Add body  
            foreach (var emp in employees)
            {
                AddCellToBody(tableLayout, "ALUA");
                AddCellToBody(tableLayout, "123.3");
                AddCellToBody(tableLayout, "123.3");
                AddCellToBody(tableLayout, "123.3");
                AddCellToBody(tableLayout, "123.3");
                AddCellToBody(tableLayout, "123.3");
                AddCellToBody(tableLayout, "123.3");
                AddCellToBody(tableLayout, "123.3");
                AddCellToBody(tableLayout, "123.3");
                AddCellToBody(tableLayout, "123.3");
                AddCellToBody(tableLayout, "123.3");
                AddCellToBody(tableLayout, "123.3");
                AddCellToBody(tableLayout, "123.3");
                AddCellToBody(tableLayout, "123.3");
                AddCellToBody(tableLayout, "123.3");
            }

            return tableLayout;
        }

        // Method to add single cell to the Header  
        private void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell( new PdfPCell(new Phrase(cellText, 
                new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.WHITE)))
            { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(20, 112, 98) });
        }

        // Method to add single cell to the body  
        private void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, 
                new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
             { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255) });
        }

        private List<Tuple<int, string, string, string, DateTime>> GetEmployees()
        {
            return new List<Tuple<int, string, string, string, DateTime>>()
            {
                new Tuple<int, string, string, string, DateTime>(1, "Robert Fdez", "Male", "Alamar City", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(2, "Ory Rdguez", "Female", "New Vedado", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(3, "Ale Torres", "Male", "New Vedado", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(4, "Karmin Diez", "Female", "Melena City", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(5, "Ricardo Franklin", "Male", "Diez de Octubre", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(1, "Robert Fdez", "Male", "Alamar City", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(2, "Ory Rdguez", "Female", "New Vedado", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(3, "Ale Torres", "Male", "New Vedado", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(4, "Karmin Diez", "Female", "Melena City", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(5, "Ricardo Franklin", "Male", "Diez de Octubre", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(1, "Robert Fdez", "Male", "Alamar City", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(2, "Ory Rdguez", "Female", "New Vedado", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(3, "Ale Torres", "Male", "New Vedado", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(4, "Karmin Diez", "Female", "Melena City", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(5, "Ricardo Franklin", "Male", "Diez de Octubre", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(1, "Robert Fdez", "Male", "Alamar City", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(2, "Ory Rdguez", "Female", "New Vedado", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(3, "Ale Torres", "Male", "New Vedado", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(4, "Karmin Diez", "Female", "Melena City", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(5, "Ricardo Franklin", "Male", "Diez de Octubre", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(1, "Robert Fdez", "Male", "Alamar City", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(2, "Ory Rdguez", "Female", "New Vedado", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(3, "Ale Torres", "Male", "New Vedado", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(4, "Karmin Diez", "Female", "Melena City", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(5, "Ricardo Franklin", "Male", "Diez de Octubre", DateTime.Now),
                new Tuple<int, string, string, string, DateTime>(6, "Alina Barrera", "Female", "Playa", DateTime.Now)
            };
        }
    }
}
