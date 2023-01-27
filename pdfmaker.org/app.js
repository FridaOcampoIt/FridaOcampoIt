const pdfprint = require("pdfmake");
const fs =require("fs");

const fonts =require("./fonts");
const styles =require("./styles");
const {content} =require("./pdfContent");
const PdfPrinter = require("pdfmake");

 let docDefinicion={
    content:content,
    styles:styles
 };
 const print = new PdfPrinter(fonts);

 let pdfDoc =printer.createPdfKitDocument(docDefinition);
 pdfDoc.pipe(fs.createReadStream("pdfs/pdfTest.pdf"));
 pdfDoc.end();