//-----------------------------------------------------------------------
// <copyright file="TestsBase.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions.Tests
{
    using NUnit.Framework;

    internal class TestsBase
    {
        [SetUp]
        public void Init()
        {
            Log.Enabled = false;
        }
        
        [TearDown]
        public void Dispose()
        {
            Log.Enabled = true;
        }
    }
}
