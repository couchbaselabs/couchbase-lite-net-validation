﻿using Microsoft.DotNet.XHarness.TestRunners.Common;
using System.Globalization;

namespace Couchbase.Lite.Tests.Maui
{
	class TestDevice : IDevice
	{
		public string BundleIdentifier => AppInfo.PackageName;

		public string UniqueIdentifier => Guid.NewGuid().ToString("N");

		public string Name => DeviceInfo.Name;

		public string Model => DeviceInfo.Model;

		public string SystemName => DeviceInfo.Platform.ToString();

		public string SystemVersion => DeviceInfo.VersionString;

		public string Locale => CultureInfo.CurrentCulture.Name;
	}
}
