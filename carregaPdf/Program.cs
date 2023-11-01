// See https://aka.ms/new-console-template for more information
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.IO;
using System.Text;

Console.WriteLine("Hello, World!");

var dados = File.Open("arquivos/vazio.pdf", FileMode.Open);

var texto = dados.ToString();

var stream = new MemoryStream();
dados.CopyTo(stream);

var bytes = stream.ToArray();

var pdfString = "%PDF-1.4\r\n%����\r\n1 0 obj\r\n<</Title (Documento sem nome)\r\n/Producer (Skia/PDF m116 Google Docs Renderer)>>\r\nendobj\r\n3 0 obj\r\n<</ca 1\r\n/BM /Normal>>\r\nendobj\r\n4 0 obj\r\n<</Length 84>> stream\r\n1 0 0 -1 0 842 cm\r\nq\r\n.75 0 0 .75 0 0 cm\r\n1 1 1 RG 1 1 1 rg\r\n/G3 gs\r\n0 0 794 1123 re\r\nf\r\nQ\r\n\r\nendstream\r\nendobj\r\n2 0 obj\r\n<</Type /Page\r\n/Resources <</ProcSet [/PDF /Text /ImageB /ImageC /ImageI]\r\n/ExtGState <</G3 3 0 R>>>>\r\n/MediaBox [0 0 596 842]\r\n/Contents 4 0 R\r\n/StructParents 0\r\n/Parent 5 0 R>>\r\nendobj\r\n5 0 obj\r\n<</Type /Pages\r\n/Count 1\r\n/Kids [2 0 R]>>\r\nendobj\r\n6 0 obj\r\n<</Type /Catalog\r\n/Pages 5 0 R>>\r\nendobj\r\nxref\r\n0 7\r\n0000000000 65535 f \r\n0000000015 00000 n \r\n0000000278 00000 n \r\n0000000109 00000 n \r\n0000000146 00000 n \r\n0000000466 00000 n \r\n0000000521 00000 n \r\ntrailer\r\n<</Size 7\r\n/Root 6 0 R\r\n/Info 1 0 R>>\r\nstartxref\r\n568\r\n%%EOF\r\n\r\n";

texto = Encoding.UTF8.GetString(bytes);

var documentoPdf = new PdfDocument();




var pdfStream = new MemoryStream(Encoding.UTF8.GetBytes(texto));

PdfDocument pdfPagina = PdfReader.Open(stream, PdfDocumentOpenMode.Import);

documentoPdf.AddPage(pdfPagina.Pages[0]);

Console.WriteLine(texto);

var fluxo = new MemoryStream();
documentoPdf.Save(fluxo);
var bytesJuntados = fluxo.ToArray();

