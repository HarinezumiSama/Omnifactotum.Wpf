using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Markup;
using Omnifactotum.Wpf;

[assembly: AssemblyTitle("Omnifactotum.Wpf")]
[assembly: CLSCompliant(true)]
[assembly: Guid("45a781f2-ed60-4f4b-9eba-976ea2f9d4a4")]

//// XAML-specific annotations: Root

[assembly: XmlnsPrefix(InternalConstants.RootXamlNamespace, InternalConstants.RootXamlPrefix)]
[assembly: XmlnsDefinition(InternalConstants.RootXamlNamespace, InternalConstants.RootClrNamespace)]

//// XAML-specific annotations: Converters

[assembly: XmlnsPrefix(InternalConstants.ConvertersXamlNamespace, InternalConstants.ConvertersXamlPrefix)]
[assembly: XmlnsDefinition(InternalConstants.ConvertersXamlNamespace, InternalConstants.ConvertersClrNamespace)]

//// Friend test assemblies

[assembly: InternalsVisibleTo("Omnifactotum.Wpf.Tests, PublicKey="
    + "00240000048000009400000006020000002400005253413100040000010001000B64FE607D7D606B7CD66A17D09515AC0434"
    + "49BC4FC2FBF176798CC82CB54BACB7F3E0CD46AC87E7CA4844D605A12176A0ADCC3151F04083CE9D7D05DE1B8EB1C20EC73C"
    + "FAC51054937B63AA852260E36A0329D4EECAC965970E4848AE9196BD64F81409213333982CB415D43E5781CA2096E640D423"
    + "6C068FB54623980EE7B1")]