/*
This file is part of the iText (R) project.
Copyright (c) 1998-2023 Apryse Group NV
Authors: Apryse Software.

This program is offered under a commercial and under the AGPL license.
For commercial licensing, contact us at https://itextpdf.com/sales.  For AGPL licensing, see below.

AGPL licensing:
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
using System;
using iText.IO.Util;
using iText.Kernel.Pdf.Xobject;
using iText.Kernel.Utils;
using iText.Test;

namespace iText.Kernel.Pdf {
    public class PdfStreamTest : ExtendedITextTest {
        public static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/kernel/pdf/PdfStreamTest/";

        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/kernel/pdf/PdfStreamTest/";

        [NUnit.Framework.OneTimeSetUp]
        public static void Before() {
            CreateOrClearDestinationFolder(destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void StreamAppendDataOnJustCopiedWithCompression() {
            String srcFile = sourceFolder + "pageWithContent.pdf";
            String cmpFile = sourceFolder + "cmp_streamAppendDataOnJustCopiedWithCompression.pdf";
            String destFile = destinationFolder + "streamAppendDataOnJustCopiedWithCompression.pdf";
            PdfDocument srcDocument = new PdfDocument(new PdfReader(srcFile));
            PdfDocument document = new PdfDocument(new PdfWriter(destFile));
            srcDocument.CopyPagesTo(1, 1, document);
            srcDocument.Close();
            String newContentString = "BT\n" + "/F1 36 Tf\n" + "50 700 Td\n" + "(new content here!) Tj\n" + "ET";
            byte[] newContent = newContentString.GetBytes(System.Text.Encoding.UTF8);
            document.GetPage(1).GetLastContentStream().SetData(newContent, true);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destFile, cmpFile, destinationFolder, "diff_"
                ));
        }

        [NUnit.Framework.Test]
        public virtual void RunLengthEncodingTest01() {
            String srcFile = sourceFolder + "runLengthEncodedImages.pdf";
            PdfDocument document = new PdfDocument(new PdfReader(srcFile));
            PdfImageXObject im1 = document.GetPage(1).GetResources().GetImage(new PdfName("Im1"));
            PdfImageXObject im2 = document.GetPage(1).GetResources().GetImage(new PdfName("Im2"));
            byte[] imgBytes1 = im1.GetImageBytes();
            byte[] imgBytes2 = im2.GetImageBytes();
            document.Close();
            byte[] cmpImgBytes1 = ReadFile(sourceFolder + "cmp_img1.jpg");
            byte[] cmpImgBytes2 = ReadFile(sourceFolder + "cmp_img2.jpg");
            NUnit.Framework.Assert.AreEqual(imgBytes1, cmpImgBytes1);
            NUnit.Framework.Assert.AreEqual(imgBytes2, cmpImgBytes2);
        }

        [NUnit.Framework.Test]
        public virtual void IndirectRefInFilterAndNoTaggedPdfTest() {
            String inFile = sourceFolder + "indirectRefInFilterAndNoTaggedPdf.pdf";
            String outFile = destinationFolder + "destIndirectRefInFilterAndNoTaggedPdf.pdf";
            PdfDocument srcDoc = new PdfDocument(new PdfReader(inFile));
            PdfDocument outDoc = new PdfDocument(new PdfReader(inFile), new PdfWriter(outFile));
            outDoc.Close();
            PdfDocument doc = new PdfDocument(new PdfReader(outFile));
            PdfStream outStreamIm1 = doc.GetFirstPage().GetResources().GetResource(PdfName.XObject).GetAsStream(new PdfName
                ("Im1"));
            PdfStream outStreamIm2 = doc.GetFirstPage().GetResources().GetResource(PdfName.XObject).GetAsStream(new PdfName
                ("Im2"));
            PdfStream cmpStreamIm1 = srcDoc.GetFirstPage().GetResources().GetResource(PdfName.XObject).GetAsStream(new 
                PdfName("Im1"));
            PdfStream cmpStreamIm2 = srcDoc.GetFirstPage().GetResources().GetResource(PdfName.XObject).GetAsStream(new 
                PdfName("Im2"));
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareStreamsStructure(outStreamIm1, cmpStreamIm1));
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareStreamsStructure(outStreamIm2, cmpStreamIm2));
            srcDoc.Close();
            outDoc.Close();
        }

        [NUnit.Framework.Test]
        public virtual void IndirectFilterInCatalogTest() {
            // TODO DEVSIX-1193 remove NullPointerException after fix
            String inFile = sourceFolder + "indFilterInCatalog.pdf";
            PdfDocument doc = new PdfDocument(new PdfReader(inFile), new PdfWriter(destinationFolder + "indFilterInCatalog.pdf"
                ));
            NUnit.Framework.Assert.Catch(typeof(NullReferenceException), () => doc.Close());
        }

        [NUnit.Framework.Test]
        public virtual void IndirectFilterFlushedBeforeStreamTest() {
            // TODO DEVSIX-1193 remove NullPointerException after fix
            String inFile = sourceFolder + "indFilterInCatalog.pdf";
            String @out = destinationFolder + "indirectFilterFlushedBeforeStreamTest.pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(inFile), new PdfWriter(@out));
            // Simulate the case in which filter is somehow already flushed before stream.
            // Either directly by user or because of any other reason.
            PdfObject filterObject = pdfDoc.GetPdfObject(6);
            filterObject.Flush();
            NUnit.Framework.Assert.Catch(typeof(NullReferenceException), () => pdfDoc.Close());
        }

        [NUnit.Framework.Test]
        public virtual void IndirectFilterMarkedToBeFlushedBeforeStreamTest() {
            // TODO DEVSIX-1193 remove NullPointerException after fix
            String inFile = sourceFolder + "indFilterInCatalog.pdf";
            String @out = destinationFolder + "indirectFilterMarkedToBeFlushedBeforeStreamTest.pdf";
            PdfWriter writer = new PdfWriter(@out);
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(inFile), writer);
            // Simulate the case when indirect filter object is marked to be flushed before the stream itself.
            PdfObject filterObject = pdfDoc.GetPdfObject(6);
            filterObject.GetIndirectReference().SetState(PdfObject.MUST_BE_FLUSHED);
            // The image stream will be marked as MUST_BE_FLUSHED after page is flushed.
            pdfDoc.GetFirstPage().GetPdfObject().GetIndirectReference().SetState(PdfObject.MUST_BE_FLUSHED);
            NUnit.Framework.Assert.Catch(typeof(NullReferenceException), () => writer.FlushWaitingObjects(JavaCollectionsUtil
                .EmptySet<PdfIndirectReference>()));
            NUnit.Framework.Assert.Catch(typeof(NullReferenceException), () => pdfDoc.Close());
        }
    }
}
