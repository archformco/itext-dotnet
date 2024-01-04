/*
This file is part of the iText (R) project.
Copyright (c) 1998-2024 Apryse Group NV
Authors: Apryse Software.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
using System;
using iText.Kernel.Pdf;
using iText.Pdfa.Exceptions;
using iText.Test;

namespace iText.Pdfa.Checker {
    [NUnit.Framework.Category("UnitTest")]
    public class PdfA1CheckerTest : ExtendedITextTest {
        private PdfA1Checker pdfA1Checker = new PdfA1Checker(PdfAConformanceLevel.PDF_A_1B);

        [NUnit.Framework.SetUp]
        public virtual void Before() {
            pdfA1Checker.SetFullCheckMode(true);
        }

        [NUnit.Framework.Test]
        public virtual void CheckCatalogDictionaryWithoutAAEntry() {
            PdfDictionary catalog = new PdfDictionary();
            catalog.Put(PdfName.AA, new PdfDictionary());
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => pdfA1Checker.CheckCatalogValidEntries
                (catalog));
            NUnit.Framework.Assert.AreEqual(PdfAConformanceException.A_CATALOG_DICTIONARY_SHALL_NOT_CONTAIN_AA_ENTRY, 
                e.Message);
        }

        [NUnit.Framework.Test]
        public virtual void CheckCatalogDictionaryWithoutOCPropertiesEntry() {
            PdfDictionary catalog = new PdfDictionary();
            catalog.Put(PdfName.OCProperties, new PdfDictionary());
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => pdfA1Checker.CheckCatalogValidEntries
                (catalog));
            NUnit.Framework.Assert.AreEqual(PdfAConformanceException.A_CATALOG_DICTIONARY_SHALL_NOT_CONTAIN_OCPROPERTIES_KEY
                , e.Message);
        }

        [NUnit.Framework.Test]
        public virtual void CheckCatalogDictionaryWithoutEmbeddedFiles() {
            PdfDictionary names = new PdfDictionary();
            names.Put(PdfName.EmbeddedFiles, new PdfDictionary());
            PdfDictionary catalog = new PdfDictionary();
            catalog.Put(PdfName.Names, names);
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => pdfA1Checker.CheckCatalogValidEntries
                (catalog));
            NUnit.Framework.Assert.AreEqual(PdfAConformanceException.A_NAME_DICTIONARY_SHALL_NOT_CONTAIN_THE_EMBEDDED_FILES_KEY
                , e.Message);
        }

        [NUnit.Framework.Test]
        public virtual void CheckValidCatalog() {
            pdfA1Checker.CheckCatalogValidEntries(new PdfDictionary());
        }

        // checkCatalogValidEntries doesn't change the state of any object
        // and doesn't return any value. The only result is exception which
        // was or wasn't thrown. Successful scenario is tested here therefore
        // no assertion is provided
        [NUnit.Framework.Test]
        public virtual void CheckSignatureTest() {
            PdfDictionary dict = new PdfDictionary();
            pdfA1Checker.CheckSignature(dict);
            NUnit.Framework.Assert.IsTrue(pdfA1Checker.ObjectIsChecked(dict));
        }
    }
}
