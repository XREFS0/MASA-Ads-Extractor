# lib/ — Third-party dependencies

This folder is **not committed** to the repository.

The project depends on two commercial third-party libraries that cannot be redistributed
under their own licenses:

| DLL | Vendor | Purpose |
|---|---|---|
| `EO.Base.dll` | Essential Objects (EO.WebBrowser) | Core library for the embedded Chromium browser |
| `EO.WebBrowser.dll` | Essential Objects | Embedded Chromium browser |
| `EO.WebBrowser.WinForm.dll` | Essential Objects | WinForms host for the browser |
| `EO.WebEngine.dll` | Essential Objects | Chromium engine |
| `ComponentFactory.Krypton.Toolkit.dll` | ComponentFactory Krypton Toolkit | UI toolkit / modern theme |

## How to obtain them

1. **EO.WebBrowser** — available from [essentialobjects.com](https://www.essentialobjects.com/). You need the `.NET` edition. The four `EO.*` DLLs listed above ship in its distribution package.
2. **Krypton Toolkit** — `ComponentFactory.Krypton.Toolkit.dll` ships with the ComponentFactory Krypton Toolkit / Krypton Suite distribution.

Copy the required DLLs into this folder:

```
MASA Ads Extractor\lib\
├── ComponentFactory.Krypton.Toolkit.dll
├── EO.Base.dll
├── EO.WebBrowser.dll
├── EO.WebBrowser.WinForm.dll
└── EO.WebEngine.dll
```

After the DLLs are in place, build the solution normally (see the main [README](../README.md)).

> Note: These DLLs are also copied into the project's build output (`bin\Release\`) automatically by MSBuild.
> A deployment-ready copy of the compiled application is kept locally in the `ori\` folder, which is also excluded from the repository.