/*
This file is part of the iText (R) project.
Copyright (c) 1998-2024 Apryse Group NV
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
using Org.BouncyCastle.Asn1.Cms;
using iText.Bouncycastlefips.Asn1;
using iText.Bouncycastlefips.Asn1.X509;
using iText.Commons.Bouncycastle.Asn1;
using iText.Commons.Bouncycastle.Asn1.Cms;
using iText.Commons.Bouncycastle.Asn1.X509;

namespace iText.Bouncycastlefips.Asn1.Cms {
    /// <summary>
    /// Wrapper class for
    /// <see cref="Org.BouncyCastle.Asn1.Cms.KeyTransRecipientInfo"/>.
    /// </summary>
    public class KeyTransRecipientInfoBCFips : Asn1EncodableBCFips, IKeyTransRecipientInfo {
        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Asn1.Cms.KeyTransRecipientInfo"/>.
        /// </summary>
        /// <param name="keyTransRecipientInfo">
        /// 
        /// <see cref="Org.BouncyCastle.Asn1.Cms.KeyTransRecipientInfo"/>
        /// to be wrapped
        /// </param>
        public KeyTransRecipientInfoBCFips(KeyTransRecipientInfo keyTransRecipientInfo)
            : base(keyTransRecipientInfo) {
        }

        /// <summary>
        /// Creates new wrapper instance for
        /// <see cref="Org.BouncyCastle.Asn1.Cms.KeyTransRecipientInfo"/>.
        /// </summary>
        /// <param name="recipientIdentifier">RecipientIdentifier wrapper</param>
        /// <param name="algorithmIdentifier">AlgorithmIdentifier wrapper</param>
        /// <param name="octetString">ASN1OctetString wrapper</param>
        public KeyTransRecipientInfoBCFips(IRecipientIdentifier recipientIdentifier, IAlgorithmIdentifier algorithmIdentifier
            , IAsn1OctetString octetString)
            : base(new KeyTransRecipientInfo(((RecipientIdentifierBCFips)recipientIdentifier).GetRecipientIdentifier()
                , ((AlgorithmIdentifierBCFips)algorithmIdentifier).GetAlgorithmIdentifier(), ((Asn1OctetStringBCFips)octetString
                ).GetOctetString())) {
        }

        /// <summary>Gets actual org.bouncycastle object being wrapped.</summary>
        /// <returns>
        /// wrapped
        /// <see cref="Org.BouncyCastle.Asn1.Cms.KeyTransRecipientInfo"/>.
        /// </returns>
        public virtual KeyTransRecipientInfo GetKeyTransRecipientInfo() {
            return (KeyTransRecipientInfo)GetEncodable();
        }
    }
}