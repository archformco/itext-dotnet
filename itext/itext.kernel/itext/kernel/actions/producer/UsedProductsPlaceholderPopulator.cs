/*
This file is part of the iText (R) project.
Copyright (c) 1998-2021 iText Group NV
Authors: iText Software.

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
using System.Collections.Generic;
using System.Text;
using iText.IO.Util;
using iText.Kernel;
using iText.Kernel.Actions.Events;

namespace iText.Kernel.Actions.Producer {
    /// <summary>Class is used to populate <c>usedProducts</c> placeholder.</summary>
    /// <remarks>
    /// Class is used to populate <c>usedProducts</c> placeholder. Placeholder should be configured
    /// with parameter defining the format of output. Within format strings, unquoted letters from
    /// <c>A</c> to <c>Z</c> and from <c>a</c> to <c>z</c> are process as pattern
    /// letters representing appropriate component of <c>usedProducts</c> format. There are three
    /// letters which are allowed in the outputformat:
    /// <para />
    /// <list type="bullet">
    /// <item><description><c>P</c> stands for product name
    /// </description></item>
    /// <item><description><c>V</c> stands for version of the product
    /// </description></item>
    /// <item><description><c>T</c> is for usage type of the product
    /// </description></item>
    /// </list>
    /// <para />
    /// Text can be quoted using single quotes (') to avoid interpretation. All other characters are not
    /// interpreted and just copied into the output string. String may contain escaped apostrophes
    /// <c>\'</c> which processed as characters. Backslash is used for escaping so you need double
    /// backslash to print it <c>\\</c>. All the rest backslashes (not followed by apostrophe or
    /// one more backslash) are simply ignored.
    /// <para />
    /// The result of the processing is the list of all products mentioned among events as a
    /// comma-separated list. The order of the elements is defined by the order of products mentioning in
    /// the <c>events</c>. Equal strings are skipped even if they were generated for different
    /// products (i. e. format <c>P</c> stands for product name only: if several version of the
    /// same product are used, it will be the only mentioning of that product).
    /// </remarks>
    internal class UsedProductsPlaceholderPopulator : AbstractFormattedPlaceholderPopulator {
        private const char PRODUCT_NAME = 'P';

        private const char VERSION = 'V';

        private const char USAGE_TYPE = 'T';

        private const String PRODUCTS_SEPARATOR = ", ";

        /// <summary>
        /// Builds a replacement for a placeholder <c>usedProducts</c> in accordance with the
        /// registered events and provided format.
        /// </summary>
        /// <param name="events">is a list of event involved into document processing</param>
        /// <param name="parameter">defines output format in accordance with the for description</param>
        /// <returns>populated comma-separated list of used products in accordance with the format</returns>
        public override String Populate(IList<ITextProductEventWrapper> events, String parameter) {
            if (parameter == null) {
                throw new ArgumentException(MessageFormatUtil.Format(PdfException.InvalidUsageFormatRequired, "usedProducts"
                    ));
            }
            ICollection<UsedProductsPlaceholderPopulator.ProductRepresentation> usedProducts = new LinkedHashSet<UsedProductsPlaceholderPopulator.ProductRepresentation
                >();
            foreach (ITextProductEventWrapper @event in events) {
                usedProducts.Add(new UsedProductsPlaceholderPopulator.ProductRepresentation(@event));
            }
            ICollection<String> usedProductsRepresentations = new LinkedHashSet<String>();
            foreach (UsedProductsPlaceholderPopulator.ProductRepresentation representation in usedProducts) {
                usedProductsRepresentations.Add(FormatProduct(representation, parameter));
            }
            StringBuilder result = new StringBuilder();
            foreach (String stringRepresentation in usedProductsRepresentations) {
                if (result.Length > 0) {
                    result.Append(PRODUCTS_SEPARATOR);
                }
                result.Append(stringRepresentation);
            }
            return result.ToString();
        }

        private String FormatProduct(UsedProductsPlaceholderPopulator.ProductRepresentation product, String format
            ) {
            StringBuilder builder = new StringBuilder();
            char[] formatArray = format.ToCharArray();
            for (int i = 0; i < formatArray.Length; i++) {
                if (formatArray[i] == APOSTROPHE) {
                    i = AttachQuotedString(i, builder, formatArray);
                }
                else {
                    if (IsLetter(formatArray[i])) {
                        builder.Append(FormatLetter(formatArray[i], product));
                    }
                    else {
                        builder.Append(formatArray[i]);
                    }
                }
            }
            return builder.ToString();
        }

        private String FormatLetter(char letter, UsedProductsPlaceholderPopulator.ProductRepresentation product) {
            if (letter == PRODUCT_NAME) {
                return product.GetProductName();
            }
            else {
                if (letter == VERSION) {
                    return product.GetVersion();
                }
                else {
                    if (letter == USAGE_TYPE) {
                        return product.GetProductUsageType();
                    }
                    else {
                        throw new ArgumentException(MessageFormatUtil.Format(PdfException.PatternContainsUnexpectedCharacter, letter
                            ));
                    }
                }
            }
        }

        private class ProductRepresentation {
            private readonly String productName;

            private readonly String productUsageType;

            private readonly String version;

            public ProductRepresentation(ITextProductEventWrapper @event) {
                productName = @event.GetEvent().GetProductData().GetPublicProductName();
                productUsageType = @event.GetProductUsageType();
                version = @event.GetEvent().GetProductData().GetVersion();
            }

            public virtual String GetProductName() {
                return productName;
            }

            public virtual String GetProductUsageType() {
                return productUsageType;
            }

            public virtual String GetVersion() {
                return version;
            }

            public override bool Equals(Object o) {
                if (this == o) {
                    return true;
                }
                if (o == null || GetType() != o.GetType()) {
                    return false;
                }
                UsedProductsPlaceholderPopulator.ProductRepresentation that = (UsedProductsPlaceholderPopulator.ProductRepresentation
                    )o;
                if (GetProductName() != null ? !GetProductName().Equals(that.GetProductName()) : that.GetProductName() != 
                    null) {
                    return false;
                }
                if (GetProductUsageType() != null ? !GetProductUsageType().Equals(that.GetProductUsageType()) : that.GetProductUsageType
                    () != null) {
                    return false;
                }
                return GetVersion() != null ? GetVersion().Equals(that.GetVersion()) : that.GetVersion() == null;
            }

            public override int GetHashCode() {
                int result = GetProductName() != null ? GetProductName().GetHashCode() : 0;
                result = 31 * result + (GetProductUsageType() != null ? GetProductUsageType().GetHashCode() : 0);
                result = 31 * result + (GetVersion() != null ? GetVersion().GetHashCode() : 0);
                return result;
            }
        }
    }
}
