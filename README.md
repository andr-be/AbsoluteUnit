# AbsoluteUnit
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![Tests](https://img.shields.io/badge/tests-90%25%20passing-green)
![.NET 8](https://img.shields.io/badge/.NET-8-blue)

A useful unit conversion system built to eliminate conversion errors in scientific calculations through strong type safety and rigorous validation.

## Why This Exists

I got tired of Googling unit conversions every time I needed to work with compound units. 
Half the online calculators are questionable, and it's completely impossible to work out which.

So, I built this because:
1. I regularly require unit complex conversions - converting between units like m/s and in/µs should be straightforward
2. It's technically interesting - turns out making a computer understand unit relationships needs some proper type system work
3. I wanted something I could trust to perform this role well - I can't necessarily trust my own maths, but I can trust the tests!

It's also been a great way to explore some interesting programming concepts - parsing, type systems, error handling, domain modelling, etc.

## Core Features

### Measurement Engine
- **Type-Safe Unit System**
  - Comprehensive validation of unit compatibility
  - Automatic handling of compound units (e.g., kg⋅m/s²)
  - Robust error handling with detailed error codes and messages
  
- **Precision-Focused Design**
  - Handles scientific notation (e.g., 2.330e-9 m)
  - Complete SI prefix support (quecto 10⁻³⁰ to quetta 10³⁰)
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
```plaintext
AbsoluteUnit/
├── Program/
│   ├── Commands/          # Command pattern implementation
│   ├── Factories/         # Unit and command creation
│   ├── Parsers/          # Input processing & validation
│   │   └── ParserGroups/ # Parsing data structures
│   ├── Structures/       # Core measurement types
│   └── UnitTypes/        # Unit system definitions
└── Tests/                # Comprehensive test suite
```

### Key Components
- **Command Pattern Implementation**
  - Separation between parsing, validation, and execution
  - Extensible command system for future measurement operations
  - Structured error propagation (ErrorCode enum system)

- **Strong Type System**
  - Unit compositions validated at runtime
  - Comprehensive prefix handling through SIPrefix system
  - Extensive error checking and validation

### Testing Focus
- Thorough test coverage across components:
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
- Expanding unit simplification capabilities
- Refining derived unit handling
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
  -sig <n>         Set significant figures
  -std            Use scientific notation
  -eng            Use engineering notation
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