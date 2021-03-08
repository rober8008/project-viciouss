using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ctaDATAMODEL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using iTextSharp.text.pdf.draw;

namespace ctaSERVICES.Reporting
{
    public class ReportService
    {
        public ReportData_Retriever Retriever { get; set; }

        public ReportService()
        {
            this.Retriever = new ReportData_Retriever();
        }

        public MemoryStream CreatePdf(string imageURL, string rulesURL)
        {
            var workStream = new MemoryStream();
            var eventHandler = new PageHeaderEventHandler();
            var doc = new Document(PageSize.A4, 10f, 10f, 20f, 0f);

            var writer = PdfWriter.GetInstance(doc, workStream);
            writer.PageEvent = eventHandler;
            writer.CloseStream = false;

            doc.Open();                      

            Image jpg = Image.GetInstance(imageURL);
            //Resize image depend upon your need
            jpg.ScaleToFit(220, 200);
            jpg.Alignment = Element.ALIGN_LEFT;
            doc.Add(jpg);            

            AddBlankLine(doc);

            Chunk date = new Chunk(DateTime.Now.ToString("dddd, MMMM dd, yyyy", new CultureInfo("es-ES")));
            date.Font.Size = 14.0f;
            date.Font.SetColor(51, 51, 51);
            date.Font.SetStyle("bold");

            Paragraph title = new Paragraph("Reporte Técnico de Activos");
            title.Font.Size = 18.0f;
            title.Font.SetColor(51, 51, 51);
            title.Font.SetStyle("bold");
            title.Add(new Chunk(new VerticalPositionMark()));
            title.Add(date);
            doc.Add(title);

            AddBlankLine(doc);

            //Paragraph title2 = new Paragraph("¿Cómo debe leerse el Reporte Técnico?");
            //title2.Font.Size = 15.0f;
            //title2.Alignment = Element.ALIGN_LEFT;
            //doc.Add(title2);

            //AddBlankLine(doc);

            doc.Add(new Paragraph("El reporte técnico de Viciouss tiene como objetivo ayudarnos a detectar rápidamente oportunidades de trading, mediante un mapeo de color de los principales indicadores técnicos."));

            AddBlankLine(doc);

            doc.Add(new Paragraph("Mientras predomine el color verde en los indicadores de un determinado activo, significa que estos están alineados indicando tendencia alcista y/o recomendación de compra, o sobreventa en el caso de RSI y Williams R%."));

            AddBlankLine(doc);

            doc.Add(new Paragraph("De lo contrario, a medida que predomine el color rojo para los indicadores de determinado activo, estos estarán alineados indicando tendencia bajista y/o recomendación de venta, o sobrecompra en el caso de RSI y Williams R%."));

            AddBlankLine(doc);

            doc.Add(new Paragraph("En los casos en que el cuadrante se encuentra pintado de color blanco, el indicador estará marcando un rango de oscilación normal, o sin definir tendencia en el caso del Momentum."));

            AddBlankLine(doc);

            doc.Add(new Paragraph("Las reglas que sigue nuestro reporte técnico son las siguientes:"));

            AddBlankLine(doc);

            Image rules = Image.GetInstance(rulesURL);
            //Resize image depend upon your need
            rules.ScaleToFit(440, 380);

            rules.Alignment = Element.ALIGN_CENTER;
            doc.Add(rules);

            AddBlankLine(doc);            

            Paragraph text1 = new Paragraph("Los valores corresponden al último cierre de mercado.");
            //text1.Font.SetColor(0, 138, 105);
            doc.Add(text1);

            //Get Sources
            var sources = this.Retriever.GetReportData();

            doc.SetMargins(10f, 10f, 20f + eventHandler.GetHeaderHight, 0f);
            // Add a new page to the pdf file
            doc.NewPage();

            //Seccion Indices
            doc.Add(new Paragraph("INDICES"));
            AddBlankLine(doc);
            if (sources.Indices.Count() > 0)
            {
                doc.Add(AddContent(sources.Indices));
            }
            else
                doc.Add(new Paragraph("Datos no disponibles."));

            AddBlankLine(doc);

            //Seccion ADRs
            doc.Add(new Paragraph("ADRs"));
            AddBlankLine(doc);
            if (sources.ADRs.Count() > 0)
            {
                doc.Add(AddContent(sources.ADRs));
            }
            else
                doc.Add(new Paragraph("Datos no disponibles."));

            AddBlankLine(doc);

            //Seccion BYMAs
            doc.Add(new Paragraph("ACCIONES BYMA"));
            AddBlankLine(doc);
            if (sources.BYMAs.Count() > 0)
            {
                doc.Add(AddContent(sources.BYMAs));
            }
            else
                doc.Add(new Paragraph("Datos no disponibles."));

            AddBlankLine(doc);

            //Seccion CEDEARs
            doc.Add(new Paragraph("CEDEARs"));
            AddBlankLine(doc);
            if (sources.CEDEARs.Count() > 0)
            {
                doc.Add(AddContent(sources.CEDEARs));
            }
            else
                doc.Add(new Paragraph("Datos no disponibles."));

            AddBlankLine(doc);

            //Seccion Bonos
            doc.Add(new Paragraph("BONOS"));
            AddBlankLine(doc);
            if (sources.Bonos.Count() > 0)
            {
                doc.Add(AddContent(sources.Bonos));
            }
            else
                doc.Add(new Paragraph("Datos no disponibles."));

            // Closing the document  
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return workStream;
        }

        #region Color Rules

        private void AddWillianRCell(PdfPTable tableLayout, double? willianR)
        {
            if (willianR > -20)
                AddRedCellToBody(tableLayout, ParseValue(willianR));
            else if (willianR >= -80 && willianR <= -20)
                AddCellToBody(tableLayout, ParseValue(willianR));
            else if(willianR < -80)
                AddGreenCellToBody(tableLayout, ParseValue(willianR));
            else
                AddCellToBody(tableLayout, ParseValue(willianR));
        }

        private void AddBoolingerCells(PdfPTable tableLayout, double? ma20, double? up, double? low, double? price)
        {
            if (up < price)
            {
                AddRedCellToBody(tableLayout, ParseValue(ma20));
                AddRedCellToBody(tableLayout, ParseValue(up));
                AddRedCellToBody(tableLayout, ParseValue(low));
            }
            else if (up > price && price > low)
            {
                AddCellToBody(tableLayout, ParseValue(ma20));
                AddCellToBody(tableLayout, ParseValue(up));
                AddCellToBody(tableLayout, ParseValue(low));
            }
            else if (low > price)
            {
                AddGreenCellToBody(tableLayout, ParseValue(ma20));
                AddGreenCellToBody(tableLayout, ParseValue(up));
                AddGreenCellToBody(tableLayout, ParseValue(low));
            }
            else
            {
                AddCellToBody(tableLayout, ParseValue(ma20));
                AddCellToBody(tableLayout, ParseValue(up));
                AddCellToBody(tableLayout, ParseValue(low));
            }
        }

        private void AddMACDCells(PdfPTable tableLayout, double? macd, double? ma9)
        {
            if (macd > ma9)
            {
                AddGreenCellToBody(tableLayout, ParseValue(macd));
                AddGreenCellToBody(tableLayout, ParseValue(ma9));
            }
            else if (macd < ma9)
            {
                AddRedCellToBody(tableLayout, ParseValue(macd));
                AddRedCellToBody(tableLayout, ParseValue(ma9));
            }
            else
            {
                AddCellToBody(tableLayout, ParseValue(macd));
                AddCellToBody(tableLayout, ParseValue(ma9));
            }
        }

        private void AddSOFastCells(PdfPTable tableLayout, double? k14, double? d3)
        {
            if (k14 > d3)
            {
                AddGreenCellToBody(tableLayout, ParseValue(k14));
                AddGreenCellToBody(tableLayout, ParseValue(d3));
            }
            else if (k14 < d3)
            {
                AddRedCellToBody(tableLayout, ParseValue(k14));
                AddRedCellToBody(tableLayout, ParseValue(d3));
            }
            else
            {
                AddCellToBody(tableLayout, ParseValue(k14));
                AddCellToBody(tableLayout, ParseValue(d3));
            }
        }

        private void AddMomentumCell(PdfPTable tableLayout, double? momentum)
        {
            if (momentum > 101)
                AddGreenCellToBody(tableLayout, ParseValue(momentum));
            else if (momentum >= 99 && momentum <= 101)
                AddCellToBody(tableLayout, ParseValue(momentum));
            else if(momentum < 99)
                AddRedCellToBody(tableLayout, ParseValue(momentum));
            else
                AddCellToBody(tableLayout, ParseValue(momentum));
        }

        private void AddRSICell(PdfPTable tableLayout, double? RSI)
        {
            if(RSI > 70)
                AddRedCellToBody(tableLayout, ParseValue(RSI));
            else if(RSI >= 30 && RSI <= 70)
                AddCellToBody(tableLayout, ParseValue(RSI));
            else if(RSI < 30)
                AddGreenCellToBody(tableLayout, ParseValue(RSI));
            else
                AddCellToBody(tableLayout, ParseValue(RSI));
        }

        private void AddMACells(PdfPTable tableLayout, double? MA20, double? MA50, double? MA200)
        {
            if (MA20 > MA50 && MA50 > MA200)
            {
                AddGreenCellToBody(tableLayout, ParseValue(MA20));
                AddGreenCellToBody(tableLayout, ParseValue(MA50));
                AddGreenCellToBody(tableLayout, ParseValue(MA200));
            }
            else if (MA20 < MA50 && MA50 > MA200)
            {
                AddYellowCellToBody(tableLayout, ParseValue(MA20));
                AddYellowCellToBody(tableLayout, ParseValue(MA50));
                AddYellowCellToBody(tableLayout, ParseValue(MA200));
            }
            else if (MA20 < MA50 && MA50 <= MA200)
            {
                AddRedCellToBody(tableLayout, ParseValue(MA20));
                AddRedCellToBody(tableLayout, ParseValue(MA50));
                AddRedCellToBody(tableLayout, ParseValue(MA200));
            }
            else
            {
                AddCellToBody(tableLayout, ParseValue(MA20));
                AddCellToBody(tableLayout, ParseValue(MA50));
                AddCellToBody(tableLayout, ParseValue(MA200));
            }
        }

        #endregion

        #region Body Cells
        // Method to add single cell to the body  
        private void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText,
                new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255) });
        }

        private void AddYellowCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText,
                new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(254, 255, 167) });
        }

        //private void AddBlueCellToBody(PdfPTable tableLayout, string cellText)
        //{
        //    tableLayout.AddCell(new PdfPCell(new Phrase(cellText,
        //        new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
        //    { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(42, 64, 234) });
        //}

        //private void AddGreenCellToBody(PdfPTable tableLayout, string cellText)
        //{
        //    tableLayout.AddCell(new PdfPCell(new Phrase(cellText,
        //        new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
        //    { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(20, 188, 31) });
        //}

        private void AddRedCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText,
                new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(236, 117, 118) });
        }

        //private void AddRedCellToBody(PdfPTable tableLayout, string cellText)
        //{
        //    tableLayout.AddCell(new PdfPCell(new Phrase(cellText,
        //        new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
        //    { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(224, 15, 19) });
        //}

        private void AddGreenCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText,
                new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(145, 228, 120) });
        }

        #endregion

        #region Table Format Method

        private void AddBlankLine(Document document)
        {
            var tableLayout = new PdfPTable(15);

            tableLayout.AddCell(new PdfPCell(new Phrase("hollow text",
                new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(255, 255, 255))))
            { Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT, Colspan = 100 });

            document.Add(tableLayout);
        }

        private PdfPTable AddContent(IEnumerable<Stock_Report> source)
        {
            //Create PDF Table with 15 columns  
            var tableLayout = new PdfPTable(15);

            float[] headers2 = { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }; //Header Widths  
            tableLayout.SetWidths(headers2); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 0;

            //Add body  
            foreach (var item in source)
            {
                AddCellToBody(tableLayout, item.Stock.symbol);
                AddCellToBody(tableLayout, item.Price.ToString());

                AddMACells(tableLayout, item.MA20, item.MA50, item.MA200);

                AddRSICell(tableLayout, item.RSI14);

                AddMomentumCell(tableLayout, item.Momentum12);

                AddSOFastCells(tableLayout, item.SOFast_k14, item.SOFast_d3);

                AddMACDCells(tableLayout, item.MACD2612, item.MA9);

                AddBoolingerCells(tableLayout, item.MA20, item.BoolUP, item.BoolLOW, item.Price);

                AddWillianRCell(tableLayout, item.WilliansR);
            }

            return tableLayout;
        }

        private string ParseValue(double? value)
        {
            return value.HasValue ? value.Value.ToString("#.00") : "-";
        }

        #endregion
    }

    public class PageHeaderEventHandler : PdfPageEventHelper
    {
        protected PdfPTable tblHeader1;
        protected PdfPTable tblHeader2;
        protected float tableHeight;

        public PageHeaderEventHandler()
        {
            float[] headers = { 10, 10, 10, 10, 10, 10, 10, 20, 20, 30, 10 }; //Header Widths
            tblHeader1 = new PdfPTable(11);
            tblHeader1.SetTotalWidth(headers);
            tblHeader1.TotalWidth = 575;
            AddTopCellToHeader(tblHeader1, "Activo");
            AddTopCellToHeader(tblHeader1, "Precio");
            AddTopCellToHeader(tblHeader1, "MA 20");
            AddTopCellToHeader(tblHeader1, "MA 50");
            AddTopCellToHeader(tblHeader1, "MA 200");
            AddTopCellToHeader(tblHeader1, "RSI 14");
            AddTopCellToHeader(tblHeader1, "Momentum 12");
            AddCellToHeader(tblHeader1, "SO(Fast)");
            AddCellToHeader(tblHeader1, "MACD");
            AddCellToHeader(tblHeader1, "Bollinger");
            AddTopCellToHeader(tblHeader1, "Williams R%");

            float[] headers2 = { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }; //Header Widths  
            tblHeader2 = new PdfPTable(15);
            tblHeader2.SetTotalWidth(headers2);
            tblHeader2.TotalWidth = 575; 
            AddBottomCellToHeader(tblHeader2, "");
            AddBottomCellToHeader(tblHeader2, "");
            AddBottomCellToHeader(tblHeader2, "");
            AddBottomCellToHeader(tblHeader2, "");
            AddBottomCellToHeader(tblHeader2, "");
            AddBottomCellToHeader(tblHeader2, "");
            AddBottomCellToHeader(tblHeader2, "");
            AddCellToHeader(tblHeader2, "%K(14)");
            AddCellToHeader(tblHeader2, "%D(3)");
            AddCellToHeader(tblHeader2, "MACD (26,12)");
            AddCellToHeader(tblHeader2, "MA(9)");
            AddCellToHeader(tblHeader2, "MA(20)");
            AddCellToHeader(tblHeader2, "Bollinger Up");
            AddCellToHeader(tblHeader2, "Bollinger Lo");
            AddBottomCellToHeader(tblHeader2, "");

            tableHeight = tblHeader1.TotalHeight + tblHeader2.TotalHeight;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            if (writer.PageNumber != 1)
            {
                var tableYpos = document.Top + ((document.TopMargin + tableHeight) / 2);

                tblHeader1.WriteSelectedRows(0, -1, document.Left, tableYpos, writer.DirectContent);

                tblHeader2.WriteSelectedRows(0, -1, document.Left, tableYpos - tblHeader1.TotalHeight, writer.DirectContent);
            }
        }

        public float GetHeaderHight
        { get { return tableHeight; } }

        #region Header Cells

        // Method to add single cell to the Header  
        private void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText,
                new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.WHITE)))
            { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(0, 138, 105) });
        }

        private void AddTopCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText,
                new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.WHITE)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(0, 138, 105),
                BorderWidthBottom = 0
            });
        }

        private void AddBottomCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText,
                new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.WHITE)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(0, 138, 105),
                BorderWidthTop = 0
            });
        }

        #endregion
    }
}
