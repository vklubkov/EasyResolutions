//-----------------------------------------------------------------------
// <copyright file="XElementHelper.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions
{
    using System.Xml.Linq;
    
    /// <summary>
    /// Helper class for <see cref="System.Xml.Linq.XElement"/>.
    /// </summary>
    public static class XElementHelper
    {   
        /// <summary>
        /// Extracts integer attribute from <see cref="System.Xml.Linq.XElement"/> without throwing exceptions.
        /// </summary>
        /// <returns><c>true</c>, if attribute was successfully extracted, <c>false</c> otherwise.</returns>
        /// <param name="element"><see cref="System.Xml.Linq.XElement"/> to extract the attribute from.</param>
        /// <param name="name">Attribute name.</param>
        /// <param name="value">Extracted value.</param>
        public static bool TryIntAttribute(this XElement element, string name, out int value)
        {
            value = 0;
            
            if (element == null)
            {
                return false;
            }
            
            var attribute = element.Attribute(name);
            if (attribute == null)
            {
                return false;
            }
            
            if (!int.TryParse(attribute.Value, out value))
            {
                return false;
            }
            
            return true;
        }
    }
}