# Contributing to CardSight AI .NET SDK

Thank you for your interest in contributing to the CardSight AI .NET SDK! We welcome contributions from the community.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Setup](#development-setup)
- [How to Contribute](#how-to-contribute)
- [Development Workflow](#development-workflow)
- [Code Guidelines](#code-guidelines)
- [Testing](#testing)
- [Pull Request Process](#pull-request-process)
- [Reporting Issues](#reporting-issues)

## Code of Conduct

This project adheres to a code of conduct that all contributors are expected to follow. Please be respectful and constructive in all interactions.

## Getting Started

1. Fork the repository on GitHub
2. Clone your fork locally
3. Create a new branch for your changes
4. Make your changes
5. Submit a pull request

## Development Setup

### Prerequisites

- .NET 9.0 SDK or later
- Git
- A code editor (Visual Studio, VS Code, or Rider recommended)

### Clone and Build

```bash
# Clone your fork
git clone https://github.com/YOUR-USERNAME/cardsight-sdk-dotnet.git
cd cardsight-sdk-dotnet

# Build the solution
dotnet build

# Run tests
dotnet test

# Run the example project
cd examples/CardSightAI.Examples
dotnet run
```

## How to Contribute

### Types of Contributions

We welcome several types of contributions:

- **Bug fixes**: Fix issues in the existing codebase
- **Documentation**: Improve or add documentation
- **Examples**: Add new example code demonstrating SDK usage
- **Tests**: Add or improve test coverage
- **Features**: Propose and implement new features (please open an issue first to discuss)

### What NOT to Contribute

- **Generated code changes**: Do not manually modify `src/CardSightAI/Generated/CardSightApiClient.cs`. This file is auto-generated from the OpenAPI specification. If you find issues with the generated code, please report them as issues with details about what needs to be changed in the OpenAPI spec.
- **Breaking changes**: Avoid making breaking changes to the public API without discussion

## Development Workflow

### Creating a Branch

Create a descriptive branch name:

```bash
git checkout -b feature/add-retry-logic
git checkout -b fix/timeout-issue
git checkout -b docs/improve-readme
```

### Making Changes

1. Make your changes in your branch
2. Follow the [Code Guidelines](#code-guidelines)
3. Add or update tests as needed
4. Update documentation if needed
5. Ensure all tests pass

### Commit Messages

Write clear, concise commit messages:

```
Add retry logic for transient failures

- Implement exponential backoff
- Add MaxRetryAttempts configuration option
- Update tests to verify retry behavior
```

## Code Guidelines

### C# Style

- Follow standard C# naming conventions
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and concise
- Use async/await for asynchronous operations

### Example

```csharp
/// <summary>
/// Retrieves card information by ID
/// </summary>
/// <param name="id">The unique identifier of the card</param>
/// <returns>The card information</returns>
/// <exception cref="CardSightAINotFoundException">Thrown when the card is not found</exception>
public async Task<Card> GetCardAsync(string id)
{
    if (string.IsNullOrWhiteSpace(id))
    {
        throw new ArgumentException("Card ID cannot be null or empty", nameof(id));
    }

    return await _client.GetCardAsync(id);
}
```

### Code Organization

- Place new classes in appropriate namespaces
- Keep related functionality together
- Separate concerns (configuration, exceptions, extensions, etc.)

### Auto-Generated Code

The SDK uses NSwag to automatically generate the API client from the OpenAPI specification at build time. This happens in the `GenerateNSwagClient` MSBuild target defined in `CardSightAI.csproj`.

**Important**: Do not modify the generated code directly. If you need to change the generated client:

1. Identify what needs to change in the OpenAPI specification
2. Report the issue with specific details
3. The API team will update the OpenAPI spec
4. The SDK will automatically pick up changes on the next build

## Testing

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run tests for a specific project
dotnet test tests/CardSightAI.Tests/CardSightAI.Tests.csproj
```

### Writing Tests

- Write unit tests for new functionality
- Use xUnit testing framework
- Follow the Arrange-Act-Assert pattern
- Use descriptive test names

Example:

```csharp
[Fact]
public void Constructor_WithNullApiKey_ThrowsValidationException()
{
    // Arrange
    var options = new CardSightAIOptions { ApiKey = null };

    // Act & Assert
    var exception = Assert.Throws<CardSightAIValidationException>(
        () => new CardSightAIClient(options)
    );
    Assert.Contains("API key is required", exception.Message);
}
```

### Test Coverage

- Aim for high test coverage of new code
- Test both success and error paths
- Test edge cases and boundary conditions

## Pull Request Process

### Before Submitting

1. Ensure your code builds without warnings: `dotnet build`
2. Run all tests: `dotnet test`
3. Update documentation if needed
4. Update CHANGELOG.md with your changes
5. Rebase on the latest main branch

### Submitting a Pull Request

1. Push your branch to your fork
2. Open a pull request against the `main` branch
3. Fill out the pull request template
4. Wait for review and address any feedback

### Pull Request Template

```markdown
## Description
Brief description of your changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Documentation update
- [ ] Code refactoring
- [ ] Test improvement

## Changes Made
- List specific changes
- Include relevant details

## Testing
- Describe how you tested your changes
- Include test results if applicable

## Checklist
- [ ] Code builds without warnings
- [ ] All tests pass
- [ ] Added/updated tests for changes
- [ ] Updated documentation
- [ ] Updated CHANGELOG.md
```

## Reporting Issues

### Bug Reports

When reporting bugs, please include:

- Clear description of the issue
- Steps to reproduce
- Expected behavior
- Actual behavior
- SDK version
- .NET version
- Operating system
- Sample code (if applicable)

### Feature Requests

When requesting features:

- Describe the feature and its use case
- Explain why it would be valuable
- Provide examples of how it would be used
- Consider backward compatibility

### Security Issues

If you discover a security vulnerability, please email security@cardsight.ai instead of opening a public issue.

## Questions?

If you have questions about contributing, feel free to:

- Open a discussion on GitHub
- Review existing issues and pull requests
- Contact us at support@cardsight.ai

## License

By contributing to this project, you agree that your contributions will be licensed under the MIT License.

Thank you for contributing to CardSight AI .NET SDK!
