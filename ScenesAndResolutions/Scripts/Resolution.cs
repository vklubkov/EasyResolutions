//-----------------------------------------------------------------------
// <copyright file="Resolution.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions
{
    using System.Xml.Linq;

    /// <summary>
    /// Represents the scene resolution.
    /// </summary>
    public class Resolution
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScenesAndResolutions.Resolution"/> class.
        /// </summary>
        public Resolution()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScenesAndResolutions.Resolution"/> class
        /// using the specified with and height.
        /// </summary>
        /// <param name="width">Resolution width.</param>
        /// <param name="height">Resolution height.</param>
        public Resolution(int width, int height)
        {
            this.Set(width, height);
        }

        /// <summary>
        /// Gets the resolution width.
        /// </summary>
        public int Width
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the resolution height.
        /// </summary>
        public int Height
        {
            get;
            private set;
        }

        /// <summary>
        /// Sets the width and height of the resolution.
        /// If at least one of the specified values <c>&lt;= 0</c>, 
        /// than both the width and height of the resolution are set to <c>0</c>.
        /// Otherwise, the specified values are used.
        /// </summary>
        /// <param name="width">Resolution width.</param>
        /// <param name="height">Resolution height.</param>
        public void Set(int width, int height)
        {
            if (width > 0 && height > 0)
            {
                this.Width = width;
                this.Height = height;
            }
            else
            {
                this.Width = 0;
                this.Height = 0;
            }
        }

        /// <summary>
        /// Reads resolution from the specified file extension string.
        /// </summary>
        /// <returns><c>true</c>, if the resolution was successfully read, <c>false</c> otherwise.</returns>
        /// <param name="extension">File extension string, possibly containing the resolution.</param>
        public bool FromExtension(string extension)
        {
            if (extension == null)
            {
                return false;
            }

            return this.FromString(extension.TrimStart('.'));
        }
        
        /// <summary>
        /// Returns a file extension <see cref="System.String"/> that 
        /// represents the current <see cref="ScenesAndResolutions.Resolution"/>.
        /// </summary>
        /// <returns>A file extension <see cref="System.String"/> that represents 
        /// the current <see cref="ScenesAndResolutions.Resolution"/>.</returns>
        public string ToExtension()
        {
            if (this.Width == 0 || this.Height == 0)
            {
                return string.Empty;
            }
            
            return "." + this;
        }
        
        /// <summary>
        /// Reads resolution from the specified string.
        /// </summary>
        /// <returns><c>true</c>, if the resolution was successfully read, <c>false</c> otherwise.</returns>
        /// <param name="str">String, possibly containing the resolution.</param>
        public bool FromString(string str)
        {
            if (null == str)
            {
                return false;
            }

            var tokens = str.Split('x');
            if (tokens.Length != 2)
            {
                return false;
            }
            
            int width;
            int height;
            if (!int.TryParse(tokens[0], out width) || !int.TryParse(tokens[1], out height))
            {
                return false;
            }
            
            this.Set(width, height);
            return true;
        }
        
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents 
        /// the current <see cref="ScenesAndResolutions.Resolution"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the 
        /// current <see cref="ScenesAndResolutions.Resolution"/>.</returns>
        public override string ToString()
        {
            return string.Format("{0}x{1}", this.Width, this.Height);
        }

        /// <summary>
        /// Gets the aspect ratio factor of current <see cref="ScenesAndResolutions.Resolution"/>.
        /// </summary>
        /// <returns>The aspect ratio factor.</returns>
        public float GetAspectRatioFactor()
        {
            if (this.Width == 0 || this.Height == 0)
            {
                return 0f;
            }

            var width = (float)this.Width;
            var height = (float)this.Height;
            return (width > height) ? (width / height) : (height / width);
        }

        /// <summary>
        /// Determines whether this resolution is landscape.
        /// </summary>
        /// <returns><c>true</c> if this resolution is landscape; otherwise, <c>false</c>.</returns>
        public bool IsLandscape()
        {
            return this.Width >= this.Height;
        }

        /// <summary>
        /// Determines whether this resolution is portrait.
        /// </summary>
        /// <returns><c>true</c> if this resolution is portrait; otherwise, <c>false</c>.</returns>
        public bool IsPortrait()
        {
            return this.Width <= this.Height;
        }

        /// <summary>
        /// Determines whether current <see cref="ScenesAndResolutions.Resolution"/> is a zero resolution 
        /// (either width is 0 or height is 0 or both are 0).
        /// </summary>
        /// <returns><c>true</c> if this resolutions is zero resolutions; otherwise, <c>false</c>.</returns>
        public bool IsZero()
        {
            return this.Width == 0 || this.Height == 0;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in 
        /// hashing algorithms and data structures such as a hash table.</returns>
        public override int GetHashCode()
        {
            var hash = 17;
            hash = (hash * 23) + this.Width;
            hash = (hash * 23) + this.Height;
            return hash;
        }
        
        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to 
        /// the current <see cref="ScenesAndResolutions.Resolution"/>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the 
        /// current <see cref="ScenesAndResolutions.Resolution"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current 
        /// <see cref="ScenesAndResolutions.Resolution"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as Resolution;
            return (other == null) ? false : ((this.Width == other.Width) && (this.Height == other.Height));
        }

        /// <summary>
        /// Deserializes <see cref="ScenesAndResolutions.Resolution"/> 
        /// from a <see cref="System.Xml.Linq.XElement"/>.
        /// </summary>
        /// <returns><c>true</c>, if read was successful, <c>false</c> otherwise.</returns>
        /// <param name="element"><see cref="System.Xml.Linq.XElement"/> containing the resolution.</param>
        public bool FromXml(XElement element)
        {
            if (element == null)
            {
                return false;
            }
            
            var name = element.Name.LocalName;
            if (!name.Equals("Resolution"))
            {
                return false;
            }
            
            int width;
            int height;
            if (!element.TryIntAttribute("w", out width) || !element.TryIntAttribute("h", out height))
            {
                return false;
            }
            
            this.Set(width, height);
            return true;
        }

        /// <summary>
        /// Returns a <see cref="System.Xml.Linq.XElement"/> that represents 
        /// the current <see cref="ScenesAndResolutions.Resolution"/>.
        /// </summary>
        /// <returns>A <see cref="System.Xml.Linq.XElement"/> that represents the 
        /// current <see cref="ScenesAndResolutions.Resolution"/>.</returns>
        public XElement ToXml()
        {
            return new XElement("Resolution", new XAttribute("w", this.Width), new XAttribute("h", this.Height));
        }
    }
}