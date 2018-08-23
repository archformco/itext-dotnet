﻿/*
This file is part of the iText (R) project.
Copyright (c) 1998-2017 iText Group NV
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
using System.IO;
using iText.IO.Util;
using iText.Kernel.Pdf.Xobject;
using iText.Test;
using iText.Test.Attributes;

namespace iText.StyledXmlParser.Resolver.Resource {
    class ResourceResolverTest : ExtendedITextTest {
        public static readonly String baseUri = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
           .CurrentContext.TestDirectory) + "/resources/itext/styledxmlparser/resolver/retrieveStreamTest/";

        [NUnit.Framework.Test]
        [LogMessage(LogMessageConstant.UNABLE_TO_RETRIEVE_STREAM_WITH_GIVEN_BASE_URI, Count = 2)]
        public virtual void malformedResourceNameTest() {
            String fileName = "resourceResolverTest .png";
            ResourceResolver resourceResolver = new ResourceResolver(baseUri);
            resourceResolver.RetrieveStream(fileName);
        }

        [NUnit.Framework.Test]
        public virtual void malformedResourceNameTest1() {
            NUnit.Framework.Assert.That(() => {
                String fileName = "retrieveStyl eSheetTest.css";
                ResourceResolver resourceResolver = new ResourceResolver(baseUri);
                resourceResolver.RetrieveStyleSheet(fileName);
            }
            , NUnit.Framework.Throws.TypeOf<FileNotFoundException>());
            ;
        }

        [NUnit.Framework.Test]
        public virtual void RetrieveStreamTest() {
            String fileName = "resourceResolverTest.png";
            byte[] expected = File.ReadAllBytes(baseUri + fileName);
            ResourceResolver resourceResolver = new ResourceResolver(baseUri);
            byte[] stream = resourceResolver.RetrieveStream("resourceResolverTest.png");
            NUnit.Framework.Assert.NotNull(stream);
            NUnit.Framework.Assert.AreEqual(expected.Length, stream.Length);
        }

        [NUnit.Framework.Test]
        public virtual void RetrieveStyleSheetTest() {
            string fileName = "retrieveStyleSheetTest.css";

            Stream expected = new FileStream(baseUri + fileName, FileMode.Open, FileAccess.Read);
            ResourceResolver resourceResolver = new ResourceResolver(baseUri);
            Stream stream = resourceResolver.RetrieveStyleSheet("retrieveStyleSheetTest.css");

            NUnit.Framework.Assert.NotNull(stream);
            NUnit.Framework.Assert.AreEqual(expected.Read(), stream.Read());
        }

        [NUnit.Framework.Test]
        public virtual void RetrieveImageTest() {
            string fileName = "resourceResolverTest.png";
            ResourceResolver resourceResolver = new ResourceResolver(baseUri);
            PdfImageXObject image = resourceResolver.RetrieveImage(fileName);
            NUnit.Framework.Assert.NotNull(image);
            NUnit.Framework.Assert.IsTrue(image.IdentifyImageFileExtension().EqualsIgnoreCase("png"));
        }
        
        [NUnit.Framework.Test]
        [NUnit.Framework.Ignore("RND-1019 — different behaviour on windows and linux")]
        public virtual void absolutePathTest() {
            //TODO check this test with on linux or mac with mono!
            String fileName = "retrieveStyleSheetTest.css";
            String absolutePath = UrlUtil.ToNormalizedURI(baseUri).AbsolutePath + fileName;
            Stream expected = new FileStream(absolutePath, FileMode.Open, FileAccess.Read);

            ResourceResolver resourceResolver = new ResourceResolver(baseUri);
            Stream stream = resourceResolver.RetrieveStyleSheet(absolutePath);
            NUnit.Framework.Assert.NotNull(stream);
            NUnit.Framework.Assert.AreEqual(expected.Read(), stream.Read());
        }

    }
}
