const PdfPrinter = require("pdfmake");
const fs =require("fs");

const fonts =require("./font");
const styles =require("./styles");
const {content} =require("./pdfContent");

 let docDefinicion={
    content:content,
    styles:styles
 };
 const printer= new PdfPrinter(fonts);

 let pdfDoc =printer.createPdfKitDocument(docDefinicion);
 pdfDoc.pipe(fs.createWriteStream("pdfs/pdfTest.pdf"));
 pdfDoc.end();