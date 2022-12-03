## Html Data Analyzer

**!!!** This is a testing task to demonstrate the abilities of design and implementation.

### Prerequisites 

- Visual Studio 2022 with Desktop development
- .NET 6 SDK

### Local development

- Open solution in Visual Studio
- Select `App` as a startup project
- Start application by debugging

Please mind some delay during start. The application might download and prepare the latest version of a headless browser.

### CI process

[GitHub Actions](https://github.com/fpodshivadlov/test-html-data-analyzer/actions) is set up for CI process. It contains:
- Application building
- Unit/integration tests running
- Built app artifact publishing

### Description

This is not a finished application but a proof-of-concept/prototype.

It covers:
- A .NET desktop application with MVVP pattern
- Using a headless browser (as the most reliable way to work with the data as the real user sees it)
- `iframe` support
- Visibility calculation on the browser side (with some limitations right now)
- Keeping as simple as possible (no need in DI containers, etc. as of now)

Meanwhile there are a log things to improve:
- Cross-platforming
- Consider more modern technology rather than WPF
- Better UX/UI for the application
- Performance improvements/optimizations
- The first run takes time (due to the headless browser downloading)
- Negative scenarios/validation/better error handling
- More sophisticated image/text resolution (including visibility)
- More configuration
- Improve CI/CD approach
- Better code coverage/quality gates
- etc.
