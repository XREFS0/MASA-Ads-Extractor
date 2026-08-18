# MASA Ads Extractor

[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Platform: Windows](https://img.shields.io/badge/platform-Windows-blue)](https://github.com/)

<p align="center"><img src="logo.jpg" alt="MASA Ads Extractor logo" width="160"></p>

A Windows desktop application that extracts business listings and classifieds data from online business directories, mines contact details, and exports the results to **Excel / CSV / TXT**.

Users browse the target website inside the built-in browser, run their search, and then extract the visible results into a grid with one click. Phone numbers and e-mail addresses are automatically mined from the individual detail pages.

> **Disclaimer:** Web scraping may violate the terms of service of individual websites and, depending on your jurisdiction and the data involved, could be subject to legal restrictions (e.g. GDPR). You are responsible for ensuring that your use of this tool complies with the terms of the websites you target and with applicable law. Use it responsibly and at your own risk. See [SECURITY.md](SECURITY.md).

## Features

- **Embedded Chromium browser** (EO.WebBrowser) — browse and search target sites, then extract with one click.
- **Automatic site detection** — the extraction engine is selected from the URL currently open in the browser.
- **20+ supported sites**, auto-detected by domain:

  | Site | Countries |
  |---|---|
  | kijiji.it / kijiji.ca | IT, CA |
  | subito.it, bakeca.it, secondamano.it | IT |
  | aziende.virgilio.it | IT |
  | paginegialle.it | IT |
  | autoscout24.de / .es / .fr | DE, ES, FR |
  | gelbeseiten.de | DE |
  | pagesjaunes.fr | FR |
  | paginasamarillas | ES / LatAm |
  | yellowpages.com | US |
  | yellowpages.ca | CA |
  | yellowpages.pl / .com.au / albania.com | PL, AU, AL |
  | yell.com | UK |
  | goldenpages.be | BE |
  | herold.at | AT |
  | infobel.com | multi |
  | trovanumeri.com | IT |
  | paginiaurii.ro | RO |
  | detelefoongids.nl | NL |
  | guiamais.com.br | BR |
  | yp.ru | RU |
  | yelp.com / .it / .fr / .es / .pt / .se / .cz | US, IT, FR, ES, PT, SE, CZ |

- **Phone and e-mail mining** — detail pages are scanned for contact information (optional).
- **Export** to Excel (XLS), CSV, or TXT with selectable columns and encodings (ASCII / UTF-7 / UTF-8).
- **Proxy support** — single proxy, random proxy from a list, or free proxy lists (with automatic proxy checking).
- **Random request delays** between requests to be gentle to target sites.
- **Multi-language interface** — English, Italian, German, French, Spanish (editable `languages\lang-*.txt` files).
- **Auto-export** of results to a chosen folder.

## Requirements

- **Windows 7 / 8 / 10 / 11** (x64 recommended)
- **[.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)**
- **Microsoft Excel** — required only for XLS export (export uses the Excel COM API)
- **Third-party commercial libraries** (not included in this repository due to their licenses — see [lib/README.md](MASA%20Ads%20Extractor/lib/README.md)):
  - **EO.WebBrowser** (Essential Objects) — `EO.Base.dll`, `EO.WebBrowser.dll`, `EO.WebBrowser.WinForm.dll`, `EO.WebEngine.dll`
  - **ComponentFactory Krypton Toolkit** — `ComponentFactory.Krypton.Toolkit.dll`

  Obtain them from their vendors and place them in `MASA Ads Extractor\lib\`.

## Building

The project targets **.NET Framework 4.8** and uses the classic (non-SDK) MSBuild format.

1. **Get the third-party DLLs** listed above and place them in `MASA Ads Extractor\lib\`.
2. Open `MASA Ads Extractor.sln` in **Visual Studio 2019/2022** (with the ".NET desktop development" workload) and build the `Release` configuration — or build from the command line:

   ```powershell
   msbuild "MASA Ads Extractor\MASA Ads Extractor.csproj" /p:Configuration=Release /p:Platform=AnyCPU
   ```

3. The build output, including the `languages\` folder, is written to `MASA Ads Extractor\bin\Release\`.

Run `MASA Ads Extractor.exe` from the build output folder.

## Usage

1. Launch the application.
2. Browse to a supported website using the built-in browser and run your search.
3. Wait for the results to load, then click **Get data**.
4. Review the extracted records in the results grid (13 columns: source, title, description, advertiser, location, type, date, phone, price, image, e-mail, note, details link).
5. Use **File → Export** to save the selected rows (or the whole list) to XLS / CSV / TXT.

Connection behaviour (proxies, delays, e-mail mining) is configured in **Settings**.

## Configuration

- Settings are stored per-user in `%APPDATA%\MASA Ads Extractor\settings.cfg` (XML).
- Interface strings are plain-text files under `languages\lang-*.txt` (one per language) — editable without recompiling.

## Project structure

```
MASA Ads Extractor.sln
MASA Ads Extractor/
├── MASA Ads Extractor.csproj      # Classic MSBuild project (.NET Framework 4.8)
├── app.config
├── lib/                           # Third-party DLLs (not committed) - see lib/README.md
├── languages/                     # lang-en/fr/ge/it/sp.txt (copied to build output)
├── WebScraper.cs                  # DOM helpers over EO.WebBrowser
├── HTTPScraper.cs                 # HTTP + regex HTML parsing
├── ProxyServer.cs                 # Proxy checking
├── ExcelDocument.cs               # XLS export via Excel COM
├── Microsoft/Office/Interop/Excel/# Embedded Excel Interop stubs (decompiled)
└── MASA_Ads_Extractor/
    ├── MainForm(.Designer).cs     # Main window, embedded browser, results grid
    ├── SettingsForm(.Designer).cs # Settings window
    ├── ProxiesForm(.Designer).cs  # Proxy checker window
    ├── AboutForm(.Designer).cs
    ├── Program.cs                 # Entry point
    ├── Settings.cs                # User settings model (XML)
    ├── Languages.cs               # Multi-language loader
    ├── ExportManager.cs           # TXT / CSV / XLS export
    ├── EmailMiner.cs / PhoneMiner.cs
    ├── DetailsPage.cs / DataItem.cs
    ├── LinksScrapers/             # Result-link extraction per site
    ├── DataScrapers/              # Detail-page field extraction per site
    └── Properties/                # Assembly info, resources, settings
```

## Contributing

Pull requests are welcome. Please keep changes focused and consistent with the existing decompiled-code style. For major changes, open an issue first to discuss what you would like to change.

## Security

Please report security vulnerabilities through the repository's **Security Advisories** mechanism. See [SECURITY.md](SECURITY.md) for the responsible-use policy.

## License

This project is licensed under the **MIT License** — see [LICENSE](LICENSE).

> **Note:** Third-party libraries (EO.WebBrowser, Krypton Toolkit) are **not** covered by this license and are subject to their own commercial terms.