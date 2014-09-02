//-----------------------------------------------------------------------
// <copyright file="Log.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions
{
    using UnityEngine;

    /// <summary>
    /// Scenes and resolutions logger.
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Enables or disables logging.
        /// </summary>
        private static bool enabled = true;

        /// <summary>
        /// Gets or sets a value, which switches off (if <c>false</c>) or turns on (if <c>true</c>) logging.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public static bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// Logs the specified message as info.
        /// </summary>
        /// <param name="message">Info message.</param>
        public static void Info(string message)
        {
            if (Debug.isDebugBuild && Log.Enabled)
            {
                Debug.Log(message);
            }
        }

        /// <summary>
        /// Logs the specified message as warning.
        /// </summary>
        /// <param name="message">Warning message.</param>
        public static void Warning(string message)
        {
            if (Debug.isDebugBuild && Log.Enabled)
            {
                Debug.LogWarning(message);
            }
        }
    }
}