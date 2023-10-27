using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Drawing;
using WEBAPI.Controllers;
using Winnovative.HtmlToPdfClient;

//using Winnovative;

namespace WEBAPI.Tools
{
    
    public static class ConvertHTmlToPdf
    {
        public static async Task<IActionResult> ConvertCurrentPageToPdf(Controller pController, object model, String pViewName1, String pFileName, string footerName)
        {
            // Get the view HTML string
            var htmlToConvert = (await RenderViewAsync(pController, pViewName1, model));

            // Get the base URL
            var baseUrl = "https://trephoapp.azurewebsites.net";

            const string pdfurl = "http://anothertest.westeurope.cloudapp.azure.com";
            var htmlToPdfConverter = new HtmlToPdfConverter(true, pdfurl);

            //HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();
            htmlToPdfConverter.LicenseKey = "b+Hx4PHg8vH29eDx9u7w4PPx7vHy7vn5+fng8A==";
            htmlToPdfConverter.ServicePassword = "";
            //htmlToPdfConverter.RenderedHtmlElementSelector = "#ToGenerate";
            //htmlToPdfConverter.HtmlViewerZoom = 130;

            htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;
            htmlToPdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
            htmlToPdfConverter.PdfDocumentOptions.AutoSizePdfPage = false;
            //htmlToPdfConverter.PdfDocumentOptions.StretchToFit = true;
            //htmlToPdfConverter.NavigationTimeout = 480;
            //htmlToPdfConverter.PdfDocumentOptions.JpegCompressionEnabled = false;
            htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;
            htmlToPdfConverter.PdfFooterOptions.PageNumberingStartIndex = 10;
            htmlToPdfConverter.PdfDocumentOptions.ShowHeader = false;
            htmlToPdfConverter.PdfDocumentOptions.RightMargin = htmlToPdfConverter.PdfDocumentOptions.LeftMargin = 10;
            htmlToPdfConverter.PdfDocumentOptions.TopMargin = 20;
            htmlToPdfConverter.PdfDocumentOptions.BottomMargin = 0;

            // Optionally add a space between footer and the page body
            // Leave this option not set for no spacing

            // Draw footer elements
            if (htmlToPdfConverter.PdfDocumentOptions.ShowFooter)
               await DrawFooterAsync(pController, model, footerName, htmlToPdfConverter, false, true, footerName);


            //string secondPageHtmlString = File.ReadAllText(secondPageLink);

           // string footerBaseUrl = "https://localhost:7217/Views/Commande/Footer.cshtml";

            // Set the footer height in points
            htmlToPdfConverter.PdfFooterOptions.FooterHeight = 65;

            // Set footer background color
            htmlToPdfConverter.PdfFooterOptions.FooterBackColor = RgbColor.White;

            // PdfPage secondPdfPage = null;

            // Create a HTML element to be added in footer
            /* HtmlToPdfElement secondHtml = new(0, 0, secondPageHtmlString, footerBaseUrl)
            {
                // Set the HTML element to fit the container height
                FitHeight = true
            };
            // Create a PDF page where to add the second HTML
            //secondPdfPage = htmlToPdfConverter.();

            // Add the second HTML to PDF document
            secondPdfPage.AddElement(secondHtml);*/





            //htmlToPdfConverter.ConversionDelay = 2;

            htmlToPdfConverter.PdfSecurityOptions.CanPrint = true;

            //htmlToPdfConverter.PdfSecurityOptions.KeySize = EncryptionKeySize.EncryptKey128Bit;

            // Convert the HTML string to a PDF document in a memory buffer
            var outPdfBuffer = htmlToPdfConverter.ConvertHtml(htmlToConvert, baseUrl);


            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = pFileName + ".pdf",

                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };
            //Response.Headers.Add("Content-Disposition", "inline; filename=" + pFileName + ".pdf");

            // Send the PDF file to browser
            //return new FileContentResult(outPdfBuffer, "application/pdf")
            //{
            //    FileDownloadName = pFileName + ".pdf"
            //};
            FileResult fileResult = new FileContentResult(outPdfBuffer, "application/pdf");
            fileResult.FileDownloadName = pFileName + ".pdf";

            return fileResult;
        }

        public static async Task<string> RenderViewAsync<TModel>(this Controller controller, string viewName, TModel model, bool partial = false)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;
            }

            controller.ViewData.Model = model;

            await using var writer = new StringWriter();
            var viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
            var viewResult = viewEngine.FindView(controller.ControllerContext, viewName, !partial);

            if (viewResult.Success == false)
            {
                return $"A view with the name {viewName} could not be found";
            }

            var viewContext = new ViewContext(
                controller.ControllerContext,
                viewResult.View,
                controller.ViewData,
                controller.TempData,
                writer,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);

            return writer.GetStringBuilder().ToString();
        }
        private static async Task DrawFooterAsync(Controller pController, object model, String pViewName1, HtmlToPdfConverter htmlToPdfConverter, bool addPageNumbers, bool drawFooterLine, string footerPath)
        {

            string footerHtmlString = (await RenderViewAsync(pController, pViewName1, model));

            string footerBaseUrl = "https://app-emea-we-dssprod-dss-001.azurewebsites.net/Views/Commande/Footer.cshtml";

            // Set the footer height in points
            htmlToPdfConverter.PdfFooterOptions.FooterHeight = 65;

            // Set footer background color
            htmlToPdfConverter.PdfFooterOptions.FooterBackColor = RgbColor.White;


            // Create a HTML element to be added in footer
            HtmlToPdfElement footerHtml = new(footerHtmlString, footerBaseUrl)
            {
                // Set the HTML element to fit the container height
                FitHeight = true
            };

            // Add HTML element to footer
            htmlToPdfConverter.PdfFooterOptions.AddElement(footerHtml);

            // Add page numbering
            if (addPageNumbers)
            {
                // Create a text element with page numbering place holders &p; and & P;
                TextElement footerText = new TextElement(0, 30, "Page &p; of &P;  ", new PdfFont("Times New Roman", 10, true));

                // Align the text at the right of the footer
                footerText.TextAlign = HorizontalTextAlign.Right;

                // Set page numbering text color
                footerText.ForeColor = RgbColor.Navy;

                // Embed the text element font in PDF
                footerText.EmbedSysFont = true;

                // Add the text element to footer
                htmlToPdfConverter.PdfFooterOptions.AddElement(footerText);
            }

            if (drawFooterLine)
            {
                // Calculate the footer width based on PDF page size and margins
                float footerWidth = htmlToPdfConverter.PdfDocumentOptions.PdfPageSize.Width -
                            htmlToPdfConverter.PdfDocumentOptions.LeftMargin - htmlToPdfConverter.PdfDocumentOptions.RightMargin;

                // Create a line element for the top of the footer
                LineElement footerLine = new LineElement(0, 0, footerWidth, 0);

                // Set line color
                footerLine.ForeColor = RgbColor.Gray;

                // Add line element to the bottom of the footer
                htmlToPdfConverter.PdfFooterOptions.AddElement(footerLine);
            }

            // set footer visibility in PDF pages
            htmlToPdfConverter.PdfFooterOptions.ShowInFirstPage = true;
            htmlToPdfConverter.PdfFooterOptions.ShowInOddPages = true;
            htmlToPdfConverter.PdfFooterOptions.ShowInEvenPages = true;
        }
    }
}
