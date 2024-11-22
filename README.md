# AbsoluteUnit
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![Tests](https://img.shields.io/badge/tests-50%25%20passing-yellow)
![.NET 8](https://img.shields.io/badge/.NET-8-blue)

A precision-focused unit conversion system built for NDT (Non-Destructive Testing) applications, with particular emphasis on ultrasonic measurement systems. Designed to eliminate conversion errors in scientific calculations through strong type safety and rigorous validation.

## Why This Exists

In ultrasonic NDT, unit conversion errors between imperial and metric systems can lead to significant measurement mistakes. For example, converting between inches/microsecond and meters/second requires handling compound units and scientific notation correctly. This tool was built to solve these real industrial challenges.

## Core Features

### Measurement Engine
- **Type-Safe Unit System**
  - Comprehensive validation of unit compatibility
  - Automatic handling of compound units (e.g., kg⋅m/s²)
  - Robust error handling with detailed error codes and messages
  
- **Precision-Focused Design**
  - Handles scientific notation (e.g., 2.330e-1 in/µs)
  - Supports common SI prefixes (milli to kilo)
  - Preserves measurement accuracy through conversions

### Command Line Interface
```bash
# Ultrasonic Velocity Conversion (NDT)
absoluteunit --convert "0.2330 in/µs" "m/s" -dec 2
> Result:    5918.20 m.s^-1

# Force Measurement Simplification
absoluteunit --simplify "1000 kg.m.s^-2" -std
> Result:    1.000e3 N

# Error Handling Example
absoluteunit --convert "100 meters" "kilograms"
> ERROR 500: Invalid conversion ... Cannot convert length to mass
```

## Technical Implementation

### Architecture
- **Clean Command Pattern**
  - Separation between parsing, validation, and execution
  - Extensible command system for future measurement operations
  - Structured error propagation

- **Strong Type System**
  - Unit compositions validated at runtime
  - Prefix handling through dedicated SIPrefix system
  - Comprehensive unit test coverage

### Key Components
```plaintext
Program/
├── Commands/            # Command pattern implementation
├── Structures/          # Core measurement types
├── UnitTypes/          # Unit system definitions
└── Parsers/            # Input processing
```

### Testing Focus
- 300+ unit tests covering:
  - Complex unit conversions
  - Error handling paths
  - CLI integration
  - Edge cases in scientific notation
  - Parser reliability

## Development Status

### Recent Updates
- Implemented standardized error code system
- Enhanced scientific notation handling
- Improved CLI output formatting

### Current Focus
- Expanding derived unit coverage
- Refining unit simplification algorithm
- Adding additional NDT-specific unit types

## Getting Started

```bash
# Clone and Build
git clone https://github.com/andr-be/AbsoluteUnit
cd AbsoluteUnit
dotnet build

# Basic Usage
dotnet run -- --convert "1 m" "ft" -dec 4

# Run Tests
dotnet test
```

## Command Line Options
```bash
Commands:
  --convert, -c    Convert between compatible units
  --simplify, -s   Simplify compound units
  --express, -e    Express in base SI units

Flags:
  -dec <n>         Set decimal places
  -std            Use scientific notation
  -ver            Show conversion steps
```

## License
MIT Licensed. See [LICENSE](LICENSE.txt) for details.

## Author
Ben Andrew  
NDT Technical Specialist & Software Developer  
[andr-be@github.com](mailto:andr-be@github.com)

---
*Built with .NET 8, rigorous testing, and real industrial experience in measurement systems.*