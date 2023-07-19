### Changes in 0.2.0 (since 0.1.0.5)

#### Major changes

- Enabled multi-target build
  - .NET Framework: 4.0, 4.6.1, and 4.7.2
  - .NET: 5.0-windows, 6.0-windows, and 7.0-windows
- Set `Omnifactotum` package dependency version based on the target framework

#### Internal changes

- Migrated projects to SDK-style
- Added various settings (EditorConfig, Git Ignore etc.)
- Added build script and Appveyor build configuration
- Removed obsolete files

### Changes in 0.1.0.5 (since 0.1.0.4)

- Introduced `RelayCommand` and `AsyncRelayCommand`
- Introduced `SmartKeyGestureConverter`

### Changes in 0.1.0.4 (since 0.1.0.1)

- Introduced the `WindowStyles` behavior providing attached properties `CanMinimize`, `CanMaximize`, and `HasSystemMenu`
- Introduced the generic value converters (`BooleanToValueConverter`, `NullableBooleanToValueConverter`, `RelayValueConverter`) and some descendants (`BooleanToVisibilityConverter`, `NullableBooleanToVisibilityConverter`, `BooleanToBrushConverter`, `BooleanToFontWeightConverter`).

### First published version 0.1.0.1

- Introduced the `WpfFactotum` helper class
