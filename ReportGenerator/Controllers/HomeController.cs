
using iText.IO.Image;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities;
using ReportGenerator.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReportGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IWebHostEnvironment _webHostEnvironment;
        private static Dictionary<string,string> capaData = new Dictionary<string,string>();
        private static JObject JCapadate = null;
        private static JObject JNcrdata= null;
        //-- Declare these variables globally so you can use them in different functions
        static string FONT = "c:/windows/fonts/wingding.ttf";
        static string FONT2 = "c:/windows/fonts/wingding.ttf";
        static string TEXT = "o x \u00fd \u00fe";
        static string TEXT2 = "o";
        static string TEXT3 = "\u00fe";


        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult GenerateDummyPdf()
        {
            try
            {
                Document pdfDoc = new Document(PageSize.A3, 50, 50, 50, 50);

                //pdfDoc.SetPageSize(PageSize.Letter.Rotate());
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                   
                    pdfDoc.Open();
                  
                    // Table of content Page
                    PdfPCell cell = new PdfPCell();
                    PdfPTable table = new PdfPTable(1);
                    refreshCells(ref cell, ref table, 1);
                    Image pdfImg = Image.GetInstance(_webHostEnvironment.WebRootPath + "/Images/Logo_Analinear.jpg");
                    cell.AddElement(pdfImg);
                    cell.Border = Rectangle.NO_BORDER;
                    table.WidthPercentage = 25;
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.PaddingLeft = 10;
                    cell.PaddingBottom = 10;
                    table.AddCell(cell);
                    pdfDoc.Add(table);

                    refreshCells(ref cell, ref table, 1);
                    table.WidthPercentage = 100;
                    Paragraph para = new Paragraph();

                    // Create a font with bold style
                    Font boldFont = new Font(Font.HELVETICA, 15, Font.BOLD);
                    Font boldFont2=new Font(Font.HELVETICA,13, Font.BOLD);
                    // Create a chunk with text
                    Chunk chunk = new Chunk("Corrective Action- Preventive Action Report (CAPA)", boldFont);

                    // Add the chunk to the paragraph
                    para.Add(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = new BaseColor(252, 223, 143);
                    cell.AddElement(para);
                    cell.Padding = 20;
                    table.AddCell(cell);
                   
                    pdfDoc.Add(table);


                    #region checkbox
                    // Create a checkbox field
                    

                    refreshCells(ref cell, ref table, 8);
                    table.WidthPercentage = 100;
                    cell.Colspan = 2;
                    cell.AddElement(new Paragraph("Necessity for CAPA"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 4;
                    Paragraph checkboxgroup = new Paragraph();
                    for(int i = 0; i < 4; i++)
                    {
                        string label = "NCR";
                        string statusId = "cncr";
                        if (i == 1) { label = "RMA";statusId = "crma"; }
                        else if (i == 2) { label = "Oppurtunity";statusId = "coppur"; }
                        else if (i == 3) { label = "Others";statusId = "coth"; }
                        
                        checkboxgroup.Add(AddCheckbox(JCapadate[statusId].ToString(), label));
                        checkboxgroup.Add(new Phrase("      "));
                    }
;                   cell.AddElement(checkboxgroup);
                    cell.Colspan = 4;
                    table.AddCell(cell);
                    #region checkbox adding

                    #endregion

                    refreshCell(ref cell);
                    cell.Colspan = 1;
                    cell.AddElement(new Paragraph($"Doc No : {JCapadate["docno"]}"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 1;
                    cell.AddElement(new Paragraph($"CAPA : {JCapadate["capa"]} "));
                    table.AddCell(cell);
                    
                    pdfDoc.Add(table);
                    #endregion


                    refreshCells(ref cell, ref table, 8);
                    table.WidthPercentage = 100;
                    cell.Colspan = 2;
                    cell.AddElement(new Paragraph("NCR/RMA/Other references :"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 2;
                    cell.AddElement(new Paragraph(JCapadate["nroref"].ToString()));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 1;
                    cell.AddElement(new Paragraph("customer"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 1;
                    cell.AddElement(new Paragraph(JCapadate["customer"].ToString()));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 1;
                    cell.AddElement(new Paragraph("date"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 1;
                    cell.AddElement(new Paragraph(JCapadate["date"].ToString()));
                    table.AddCell(cell);
                    pdfDoc.Add(table);



                    refreshCells(ref cell, ref table, 8);
                    table.WidthPercentage=100;
                    cell.Colspan = 2;
                    cell.AddElement(new Paragraph("Project/Product/ Part No & details:"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 3;
                    cell.AddElement(new Paragraph(JCapadate["pppdetails"].ToString()));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 1;
                    cell.AddElement(new Paragraph("WO ID:"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 2;
                    cell.AddElement(new Paragraph(JCapadate["woid"].ToString()));
                    //cell.AddElement(new Paragraph("WO ID:"));
                    table.AddCell(cell);
                    pdfDoc.Add(table);


                    refreshCells(ref cell, ref table, 8);
                    table.WidthPercentage = 100;
                    cell.Colspan = 2;
                    cell.AddElement(new Paragraph("Identification Of Problem :"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 6;
                    cell.AddElement(new Paragraph(JCapadate["iop"].ToString()));
                    // cell.AddElement(new Paragraph("WO ID:"));
                    table.AddCell(cell);
                    pdfDoc.Add(table);

                   
                    refreshCells(ref cell, ref table, 8);
                    table.WidthPercentage = 100;
                    cell.Colspan = 2;
                    cell.AddElement(new Paragraph("Correction/containment Action :"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 6;
                    cell.AddElement(new Paragraph(JCapadate["cca"].ToString()));
                    //cell.AddElement(new Paragraph("WO ID:"));
                    table.AddCell(cell);
                    pdfDoc.Add(table);

                    refreshCells(ref cell, ref table, 10);
                    table.WidthPercentage = 100;
                    cell.Colspan = 2;
                    cell.PaddingLeft = 3;
                    cell.PaddingBottom = 5;
                    cell.AddElement(new Chunk("Why Why Analysis",boldFont2));
                    //cell.AddElement(new Paragraph("Why Why Analysis"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 8;
                    //cell.AddElement(new Paragraph("WO ID:"));
                    table.AddCell(cell);
                    // table.AddCell(cell);
                    pdfDoc.Add(table);

                    for(int i = 0; i < 5; i++)
                    {
                        string w = $"w{i + 1}";
                        string a=$"a{i+1}";

                        refreshCells(ref cell, ref table, 11);
                        table.WidthPercentage = 100;
                        cell.Colspan = 1;
                        cell.AddElement(new Paragraph($"Why {i+1}"));
                        table.AddCell(cell);
                        refreshCell(ref cell);
                        cell.Colspan = 4;
                        cell.AddElement(new Paragraph(JCapadate[w].ToString()));
                        //cell.AddElement(new Paragraph("WO ID:"));
                        table.AddCell(cell);
                        // table.AddCell(cell);
                        refreshCell(ref cell);
                        cell.Colspan = 1;
                        cell.AddElement(new Paragraph("Ans :"));
                        table.AddCell(cell);
                        refreshCell(ref cell);
                        cell.Colspan = 5;
                        cell.AddElement(new Paragraph(JCapadate[a].ToString()));
                        // cell.AddElement(new Paragraph("A"));
                        table.AddCell(cell);
                        pdfDoc.Add(table);
                    }

                    refreshCells(ref cell, ref table, 11);
                    table.WidthPercentage = 100;
                    cell.Colspan = 2;
                    cell.AddElement(new Paragraph($"RC (Root Cause)"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 9;
                    cell.AddElement(new Paragraph(JCapadate[$"rc1"].ToString()));
                    //cell.AddElement(new Paragraph("WO ID:"));
                    table.AddCell(cell);
                    pdfDoc.Add(table);

                    refreshCells(ref cell, ref table, 11);
                    table.WidthPercentage = 100;
                    cell.Colspan = 2;
                    cell.AddElement(new Paragraph($"RC (Root Cause)"));
                    table.AddCell(cell);

                    #region checkboxadd
                    refreshCell(ref cell);
                    cell.Colspan = 2;
                    cell.AddElement(AddCheckbox(JCapadate["pbc"].ToString(), "Poor Base Conditions"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 2;
                    cell.AddElement(AddCheckbox(JCapadate["poc"].ToString(), "Poor Operating Conditions"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 1;
                    cell.AddElement(AddCheckbox(JCapadate["deter"].ToString(), "Deterioration"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 2;
                    cell.AddElement(AddCheckbox(JCapadate["wap"].ToString(), "Weak Assy Process"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 1;
                    cell.AddElement(AddCheckbox(JCapadate["wd"].ToString(), "Weak Design"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 1;
                    cell.AddElement(AddCheckbox(JCapadate["pc"].ToString(), "Poor Skills"));
                    table.AddCell(cell);

                    pdfDoc.Add(table);
                    #endregion
                    //cell.AddElement(new Paragraph("WO ID:"));


                    refreshCells(ref cell, ref table, 11);
                    table.WidthPercentage = 100;
                    cell.Colspan = 3;
                    cell.PaddingLeft = 3;
                    cell.PaddingBottom = 5;
                    cell.AddElement(new Chunk("Implementation of action plan", boldFont2));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 8;
                    //cell.AddElement(new Paragraph("WO ID:"));
                    table.AddCell(cell);
                    pdfDoc.Add(table);

                    for(int i = 0; i < 2; i++)
                    {
                        char decide = i == 0 ? 'c' : 'p';
                        refreshCells(ref cell, ref table, 7);
                        table.WidthPercentage = 100;
                        cell.Colspan = 1;

                        cell.AddElement(new Paragraph($"corrective action "));
                        table.AddCell(cell);
                        refreshCell(ref cell);
                        cell.Colspan = 2;
                        cell.AddElement(new Paragraph(JCapadate[$"{decide}a"].ToString()));
                        //cell.AddElement(new Paragraph("WO ID:"));
                        table.AddCell(cell);
                        refreshCell(ref cell);
                        cell.Colspan = 1;
                        cell.AddElement(new Paragraph("Target Date"));
                        table.AddCell(cell);
                        refreshCell(ref cell);
                        cell.Colspan = 1;
                        cell.AddElement(new Paragraph(JCapadate[$"{decide}td"].ToString()));
                        //cell.AddElement(new Paragraph("WO ID:"));
                        table.AddCell(cell);
                        refreshCell(ref cell);
                        cell.Colspan = 1;
                        cell.AddElement(new Paragraph("Responsible Person sign :"));
                        table.AddCell(cell);

                        refreshCell(ref cell);
                        cell.Colspan = 1;
                        cell.AddElement(new Paragraph(JCapadate[$"{decide}rps"].ToString()));
                        //cell.AddElement(new Paragraph("WO ID:"));
                        table.AddCell(cell);
                        pdfDoc.Add(table);
                    }

                    refreshCells(ref cell, ref table, 7);
                    table.WidthPercentage = 100;
                    cell.Colspan = 1;
                    
                    cell.AddElement(new Paragraph($"Prepared By") { Alignment=Element.ALIGN_CENTER});
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 1;
                    cell.AddElement(new Paragraph(JCapadate[$"prepby"].ToString()));
                    //cell.AddElement(new Paragraph("WO ID:"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 1;
                    cell.AddElement(new Paragraph("Approved By"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 1;
                    cell.AddElement(new Paragraph(JCapadate[$"appby"].ToString()));
                    //cell.AddElement(new Paragraph("WO ID:"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 2;
                    cell.AddElement(new Paragraph("verification of implementation by :"));
                    table.AddCell(cell);

                    refreshCell(ref cell);
                    cell.Colspan = 1;
                    cell.AddElement(new Paragraph(JCapadate[$"verby"].ToString()));
                    //cell.AddElement(new Paragraph("WO ID:"));
                    table.AddCell(cell);
                    pdfDoc.Add(table);

                    pdfDoc.Close();
                    
                    byte[] data_Save = memoryStream.ToArray();
                    
                    return File(data_Save, "application/pdf");
                }
            }
            catch(Exception ex)
            {
                
            }
            return View();
        }
        private static Phrase AddCheckbox(string status, string label)
        {
            //-- Local Decleration in function to add check/uncheck box
            BaseFont bf = BaseFont.CreateFont(FONT, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font fs = new iTextSharp.text.Font(bf, 15);

            Paragraph UnCheck = new Paragraph(TEXT2, fs);
            Paragraph Check = new Paragraph(TEXT3, fs);
            Phrase PM = new Phrase();
            Chunk c1 = new Chunk();
            if(status=="on")
            PM.Add(Check);
            else if(status=="off")
                PM.Add(UnCheck);
            c1 = new Chunk(label);
            PM.Add(c1);
            return PM;
        }
        public IActionResult GenerateDummyPdf2()
        {
            try
            {
                Document pdfDoc = new Document(PageSize.A3, 50, 50, 50, 50);

                //pdfDoc.SetPageSize(PageSize.Letter.Rotate());
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                    pdfDoc.Open();
                    // Create a font with bold style
                    Font boldFont = new Font(Font.HELVETICA, 15, Font.BOLD);
                    Font boldFont2 = new Font(Font.HELVETICA, 13, Font.BOLD);
                    Font boldFont3 = new Font(Font.HELVETICA, 11, Font.BOLD);
                    // Table of content Page
                    PdfPCell cell = new PdfPCell();
                    PdfPTable table = new PdfPTable(1);
                    refreshCells(ref cell, ref table, 4);
                    Image pdfImg = Image.GetInstance(_webHostEnvironment.WebRootPath + "/Images/QA.png");
                    cell.AddElement(pdfImg);
                    cell.Border = Rectangle.NO_BORDER;
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.PaddingLeft = 10;
                    cell.PaddingBottom = 10;
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Border = Rectangle.NO_BORDER;
                    Image pdfImg2 = Image.GetInstance(_webHostEnvironment.WebRootPath + "/Images/Logo_Analinear.jpg");
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Border = Rectangle.NO_BORDER;
                    cell.AddElement(pdfImg2);
                    cell.PaddingRight = 15;
                    cell.PaddingTop = 13;
                    table.AddCell(cell);
                    pdfDoc.Add(table);

                    refreshCells(ref cell, ref table, 1);
                   // cell.Border = Rectangle.NO_BORDER;
                    table.WidthPercentage = 100;
                    Paragraph para = new Paragraph();

                   
                    // Create a chunk with text
                    Chunk chunk = new Chunk("NON-CONFORMANCE REPORT (NCR)", boldFont);

                    // Add the chunk to the paragraph
                    para.Add(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = new BaseColor(167, 213, 239);
                    cell.AddElement(para);
                    cell.Padding = 10;
                    table.AddCell(cell);

                    pdfDoc.Add(table);

                    // Create a checkbox field
                    #region row1
                    refreshCells(ref cell, ref table, 9);
                    table.WidthPercentage = 100;
                    cell.Colspan = 2;
                    cell.AddElement(new Chunk("Type",boldFont3));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 2;
                    cell.AddElement(new Chunk("Project", boldFont3));
                    table.AddCell(cell);

                    refreshCell(ref cell);
                    cell.Colspan = 2;
                    cell.AddElement(new Chunk($"Part/Process/Material", boldFont3));
                    table.AddCell(cell);

                    refreshCell(ref cell);
                    cell.Colspan = 1;
                    cell.AddElement(new Chunk($"Doc No", boldFont3));
                    table.AddCell(cell);

                    refreshCell(ref cell);
                    cell.Colspan = 2;
                      cell.AddElement(new Chunk($" {JNcrdata["dno"]}"));
                    table.AddCell(cell);
                    pdfDoc.Add(table);

                    #endregion

                    #region row2
                    refreshCells(ref cell, ref table, 9);
                    table.WidthPercentage = 100;
                    cell.Colspan = 2;
                    Paragraph checkboxcontent = new Paragraph();
                    checkboxcontent.Add(AddCheckbox(JNcrdata["electrical"].ToString(), "Electrical\n"));
                    checkboxcontent.Add(AddCheckbox(JNcrdata["mechanical"].ToString(), "Mechanical"));
                    cell.AddElement(checkboxcontent);
                   // cell.AddElement(new Chunk("Type"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 2;
                    cell.AddElement(new Chunk($"{JNcrdata["pname"]}"));
                    table.AddCell(cell);

                    refreshCell(ref cell);
                    cell.Colspan = 2;
                    cell.AddElement(new Chunk($"{JNcrdata["ppm"]}"));
                    table.AddCell(cell);

                    refreshCell(ref cell);
                    cell.Colspan = 3;
                    //adding table in the cell
                    PdfPTable tb = new PdfPTable(3);
                    tb.WidthPercentage = 100;
                    PdfPCell tcell = new PdfPCell();
                    tcell.AddElement(new Chunk("Date",boldFont3));
                    tcell.Padding = 5;
                    tb.AddCell(tcell);
                    refreshCell(ref tcell);
                    tcell.Colspan = 2;
                    tcell.AddElement(new Chunk(JNcrdata["date1"].ToString()));
                    tb.AddCell(tcell);
                    cell.AddElement(tb);
                    refreshCells(ref  tcell, ref tb, 3);
                    tb.WidthPercentage = 100;
                    tcell.AddElement(new Chunk("Reference Doc",boldFont3));
                    tcell.PaddingBottom = 5;
                    tb.AddCell(tcell);
                    refreshCell(ref tcell);
                    tcell.AddElement(new Chunk(JNcrdata["rdoc"].ToString()));
                    tcell.Colspan = 2;
                    tb.AddCell(tcell);
                  
                    cell.Padding = 0;
                    cell.AddElement(tb);
                    refreshCells(ref tcell, ref tb, 3);
                    tb.WidthPercentage = 100;
                    tcell.AddElement(new Chunk("Work Order ID",boldFont3));
                    tb.AddCell(tcell);
                    refreshCell(ref tcell);
                    tcell.AddElement(new Chunk(JNcrdata["woid"].ToString()));
                    tcell.Colspan = 2;
                    tb.AddCell(tcell);
                    cell.AddElement(tb);
                    table.AddCell(cell);
                    pdfDoc.Add(table);

                    #endregion

                    #region row3 and row4

                    refreshCells(ref cell, ref table, 9);
                    table.WidthPercentage = 100;
                    cell.Colspan = 6;
                    cell.AddElement(new Chunk("Description of Non-conformance", boldFont3));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 3;
                    cell.AddElement(new Chunk("Identified By", boldFont3));
                    table.AddCell(cell);
                    pdfDoc.Add(table);

                    refreshCells(ref cell, ref table, 9);
                    table.WidthPercentage = 100;
                    cell.Colspan = 6;
                    cell.AddElement(new Chunk(JNcrdata["dnc"].ToString()));
                    // cell.AddElement(new Chunk("Description of Non-conformance"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 3;
                    cell.AddElement(new Chunk(JNcrdata["iby"].ToString()));
                    // cell.AddElement(new Chunk("Identified By"));
                    table.AddCell(cell);
                    pdfDoc.Add(table);
                    #endregion

                    #region row5 and row6

                    refreshCells(ref cell, ref table, 9);
                    table.WidthPercentage = 100;
                    cell.Colspan = 6;
                    cell.AddElement(new Chunk("Reason(s) for Non-Conformance", boldFont3));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 3;
                    cell.AddElement(new Chunk("Immediate Action required (Correction/Containment)", boldFont3));
                    table.AddCell(cell);
                    pdfDoc.Add(table);

                    refreshCells(ref cell, ref table, 9);
                    table.WidthPercentage = 100;
                    cell.Colspan = 6;
                    cell.AddElement(new Chunk(JNcrdata["rnc"].ToString()));
                    // cell.AddElement(new Chunk("Description of Non-conformance"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 3;
                    cell.AddElement(new Chunk(JNcrdata["iar"].ToString()));
                    // cell.AddElement(new Chunk("Identified By"));
                    table.AddCell(cell);
                    pdfDoc.Add(table);
                    #endregion
                    #region row7
                    refreshCells(ref cell, ref table, 9);
                    table.WidthPercentage = 100;
                    cell.Colspan = 6;
                    cell.Border = Rectangle.NO_BORDER;
                    cell.Border = Rectangle.LEFT_BORDER;
                   // cell.AddElement(new Chunk("CAPA ref no", boldFont3));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 1;
                    cell.AddElement(new Chunk("Date ",boldFont3));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 2;
                    cell.AddElement(new Chunk(JNcrdata["date2"].ToString()));
                    // cell.AddElement(new Chunk("Immediate Action required (Correction/Containment)"));
                    table.AddCell(cell);
                    pdfDoc.Add(table);

                    refreshCells(ref cell, ref table, 9);
                    table.WidthPercentage = 100;
                    cell.Colspan = 1;
                    cell.AddElement(new Chunk("CAPA ref no", boldFont3));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 8;
                    cell.AddElement(new Chunk(JNcrdata["capa"].ToString()));
                    // cell.AddElement(new Chunk("Immediate Action required (Correction/Containment)"));
                    table.AddCell(cell);
                    pdfDoc.Add(table);

                    #endregion


                    #region row8

                    refreshCells(ref cell, ref table, 9);
                    table.WidthPercentage = 100;
                    cell.Colspan = 1;
                    cell.AddElement(new Chunk("Raised By", boldFont3));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 2;
                    cell.AddElement(new Chunk(JNcrdata["rby"].ToString()));

                    // cell.AddElement(new Chunk("Immediate Action required (Correction/Containment)"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 1;
                    cell.AddElement(new Chunk("Verified By", boldFont3));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 2;
                    cell.AddElement(new Chunk(JNcrdata["vby"].ToString()));

                    // cell.AddElement(new Chunk("Immediate Action required (Correction/Containment)"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 1;
                    cell.AddElement(new Chunk("Approved By", boldFont3));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 2;
                    cell.AddElement(new Chunk(JNcrdata["aby"].ToString()));

                    // cell.AddElement(new Chunk("Immediate Action required (Correction/Containment)"));
                    table.AddCell(cell);

                    pdfDoc.Add(table);
                    #endregion

                    #region row9

                    refreshCells(ref cell, ref table, 9);
                    table.WidthPercentage = 100;
                    cell.Colspan = 1;
                    cell.AddElement(new Chunk("Closed By", boldFont3));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 5;
                    cell.AddElement(new Chunk(JNcrdata["cby"].ToString()));

                    // cell.AddElement(new Chunk("Immediate Action required (Correction/Containment)"));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 1;
                    cell.AddElement(new Chunk("Date", boldFont3));
                    table.AddCell(cell);
                    refreshCell(ref cell);
                    cell.Colspan = 2;
                    cell.AddElement(new Chunk(JNcrdata["date3"].ToString()));

                    // cell.AddElement(new Chunk("Immediate Action required (Correction/Containment)"));
                    table.AddCell(cell);
                   
                    pdfDoc.Add(table);
                    #endregion




                    pdfDoc.Close();

                    byte[] data_Save = memoryStream.ToArray();

                    return File(data_Save, "application/pdf");
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }



        public void refreshCells(ref PdfPCell obj,ref PdfPTable tbl,int col)
        {
            obj=new PdfPCell();
            obj.PaddingBottom = 5;
            obj.PaddingLeft = 5;
            tbl=new PdfPTable(col);
        }
        public void refreshCell(ref PdfPCell obj)
        {
            obj = new PdfPCell();
            obj.PaddingBottom = 5;
            obj.PaddingLeft = 10;
        }
        public Image getCheckedIcon(string status)
        {
            Image img= null;
            if(status=="off")
            img = Image.GetInstance(_webHostEnvironment.WebRootPath + "/Images/unchecked.png");
            else if(status=="on")
            img = Image.GetInstance(_webHostEnvironment.WebRootPath + "/Images/checked.png");
            img.Width = 1;

            // Set the desired width and height of the image
            //  image.SetWidth(200);
            //  image.SetHeight(100);
            return img;
        }
        

        public IActionResult Capa()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult Capa([FromBody] JObject obj)
        {
            // FillCapaData(obj);
            JCapadate = obj;
          //  Console.WriteLine("vamsi");
            return RedirectToAction("GenerateDummyPdf");

        }
        private static bool FillCapaData(JArray obj)
        {
            try
            {
                foreach (JObject jObject in obj)
                {
                    foreach (JProperty property in jObject.Properties())
                    {
                        capaData[property.Name] = property.Value.ToString();
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        private static string GetSequenceData()
        {
            try
            {
                string data = "";


                return data;
            }
            catch(Exception ex)
            {
                return "null";
            }
        }
        public IActionResult Ncr()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Ncr([FromBody] JObject obj)
        {
            JNcrdata = obj;
            return RedirectToAction("GenerateDummyPdf2");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    public class CheckboxCellEvent : IPdfPCellEvent
    {
        public void CellLayout(PdfPCell cell, Rectangle position, PdfContentByte[] canvases)
        {
            PdfWriter writer = canvases[0].PdfWriter;
            Rectangle rect = new Rectangle(position.GetLeft(3), position.GetBottom(3), position.GetLeft(13), position.GetTop(13));
            PdfFormField checkbox = PdfFormField.CreateCheckBox(writer);
            checkbox.SetWidget(rect, PdfAnnotation.HighlightNone);
            writer.AddAnnotation(checkbox);
        }
    }

}
