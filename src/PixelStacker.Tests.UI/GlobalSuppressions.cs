// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Assertion", "NUnit2005:Consider using Assert.That(actual, Is.EqualTo(expected)) instead of Assert.AreEqual(expected, actual)", Justification = "Just let me code my own way, FFS.", Scope = "member", Target = "~M:PixelStacker.Tests.UI.MaterialPickerFormTests.Top_Air_to_PureGlass")]
