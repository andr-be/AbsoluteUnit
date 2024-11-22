# AbsoluteUnit
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![Tests](https://img.shields.io/badge/tests-passing-brightgreen)
![.NET 8](https://img.shields.io/badge/.NET-8-blue)

A precision-focused CLI tool for scientific unit conversion and analysis, designed for industrial measurement applications. Built with rigorous type safety and mathematical accuracy in mind.

## Key Features

- **Industrial-Grade Unit Conversion**
  - Handles complex compound units (e.g., kg⋅m/s²)
  - Supports SI prefixes from quecto (10⁻³⁰) to quetta (10³⁰)
  - Precise handling of derived unit relationships

- **Intelligent Unit Processing**
  - Advanced unit simplification algorithm
  - Automatic base unit decomposition
  - Configurable output formatting with scientific notation

- **Type-Safe Architecture**
  - Robust measurement validation
  - Comprehensive unit compatibility checking
  - Detailed error reporting for invalid conversions

## Usage Examples

```bash
# Convert ultrasonic measurement units
absoluteunit --convert "0.2330 in/µs" "m/s" -dec 2
> Result:    5918.20 m.s^-1

# Simplify force measurements
absoluteunit --simplify "1000 kg.m.s^-2" -std
> Result:    1.000e3 N

# Express derived units in base form
absoluteunit --express "1 kN" -ver
> Result:    1000 kg.m.s^-2 (1.000 x1000)
```

## Local Development

```bash
git clone https://github.com/andr-be/AbsoluteUnit
cd AbsoluteUnit
dotnet build
dotnet run -- --convert "1 m" "ft" -dec 4
```

## Technical Implementation

The core of AbsoluteUnit is built around a sophisticated unit processing pipeline:

1. **Command Processing**
   - Robust CLI argument parsing
   - Flexible flag system for output customization
   - Structured command validation

2. **Measurement Analysis**
   - Type-safe unit representation
   - Prefix normalization
   - Complex unit relationship mapping

3. **Unit Manipulation**
   - Dynamic unit simplification
   - Base unit conversion
   - Compound unit handling

### Key Design Patterns
- Command Pattern for operation encapsulation
- Factory Pattern for unit construction
- Builder Pattern for measurement creation

## Testing
Extensive test suite covering:
- Unit conversion accuracy
- Parser edge cases
- CLI integration
- Compound unit handling
- Prefix manipulation

## Project Status
Currently focusing on:
- Expanding derived unit coverage
- Enhancing simplification algorithm
- Improving error messaging

## License
MIT Licensed. See [LICENSE](LICENSE.txt) for details.

## Author
Ben Andrew - NDT Technical Specialist & Software Developer  
[andr-be@github.com](mailto:andr-be@github.com)

---
A project born from industrial measurement needs, built with software engineering principles.