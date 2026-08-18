# Security Policy

## Reporting a Vulnerability

If you discover a security vulnerability in this project, please report it privately through the **Security Advisories** tab of this repository (or via a GitHub issue marked `security` if advisories are not available).

Please **do not** open a public issue for security problems. Include:

- A description of the vulnerability and its impact
- Steps to reproduce
- Affected versions, if known

## Responsible Use

MASA Ads Extractor is an automated browsing/extraction tool. Its users are expected to:

- **Respect the terms of service** of the websites they target, as well as their `robots.txt` and rate limits.
- **Only extract publicly available information** and only for legitimate, lawful purposes.
- **Comply with applicable law**, in particular data-protection rules such as the GDPR when handling personal data (business contact details, e-mail addresses, phone numbers).
- **Not use the tool** for spamming, harassment, fraud, or any activity that harms the targeted websites or individuals.

The maintainers are not responsible for how the software is used. If a targeted website is harmed by the use of this tool, the responsibility lies with the user, not the project.

## Scope

- E-mail addresses and phone numbers extracted from public pages are treated as sensitive personal data by some jurisdictions — handle them accordingly.
- The `lib\` folder contains proprietary third-party DLLs that must be obtained from their vendors; never commit them to this repository.