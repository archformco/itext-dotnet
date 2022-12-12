/*
This file is part of the iText (R) project.
Copyright (c) 1998-2022 iText Group NV
Authors: iText Software.

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
using iText.Test;

namespace iText.Kernel.Counter {
    public class ContextManagerTest : ExtendedITextTest {
        [NUnit.Framework.Test]
        public virtual void GetRecognisedNamespaceForSpecificNamespaceTest() {
            String outerNamespaces = NamespaceConstant.PDF_OCR.ToLowerInvariant();
            String innerNamespaces = NamespaceConstant.PDF_OCR_TESSERACT4.ToLowerInvariant();
            // Since both NamespaceConstant.PDF_OCR and NamespaceConstant.PDF_OCR_TESSERACT4 are registered
            // and the latter one begins with the former, we should check that correct namespaces are
            // recognized for each of them
            NUnit.Framework.Assert.IsTrue(innerNamespaces.StartsWith(outerNamespaces));
            NUnit.Framework.Assert.AreEqual(outerNamespaces, ContextManager.GetInstance().GetRecognisedNamespace(outerNamespaces
                ));
            NUnit.Framework.Assert.AreEqual(innerNamespaces, ContextManager.GetInstance().GetRecognisedNamespace(innerNamespaces
                ));
        }

        [NUnit.Framework.Test]
        public virtual void NotRegisteredNamespaceTest() {
            String notRegisteredNamespace = "com.hello.world";
            NUnit.Framework.Assert.AreEqual(null, ContextManager.GetInstance().GetRecognisedNamespace(notRegisteredNamespace
                ));
        }
    }
}
