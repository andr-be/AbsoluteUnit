# AbsoluteUnit 📏
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![Tests](https://img.shields.io/badge/tests-passing-brightgreen)
![.NET 6+](https://img.shields.io/badge/.NET-6%2B-blue)

A robust, extensible CLI tool for scientific unit conversion and analysis. AbsoluteUnit provides precise handling of SI, US Customary, and derived units with support for complex compound units, prefix manipulation, and automated unit simplification.

## 🌟 Features

- **Comprehensive Unit Support**
  - Full SI base and derived units
  - US Customary measurements
  - Complex compound unit handling (e.g., kg⋅m/s²)
  - Scientific notation and prefix manipulation

- **Smart Unit Operations**
  - Conversion between compatible unit systems
  - Automatic unit simplification (e.g., kg⋅m/s² → N)
  - Base unit expression (e.g., N → kg⋅m/s²)
  - Precise handling of unit prefixes (micro to tera)

- **Robust Architecture**
  - Type-safe unit manipulation
  - Extensive test coverage
  - Clean, modular design
  - Comprehensive error handling

## 🚀 Quick Start

```bash
# Convert units
absoluteunit --convert "5918.2 m/s" "in/µs" -std
> Result:    0.233e0 in.µs^-1

# Express in base units
absoluteunit --express "1 kN" -ver
> Result:    1000 kg.m.s^-2 (1.000 x1000)

# Simplify compound units
absoluteunit --simplify "1000 kg.m.s^-2" -dec 2
> Result:    1.00 kN
```

## 📦 Installation

```bash
# Via .NET CLI
dotnet tool install --global AbsoluteUnit

# Or build from source
git clone https://github.com/andr-be/AbsoluteUnit
cd AbsoluteUnit
dotnet build
```

## 🛠️ Usage

### Basic Conversion
```bash
absoluteunit --convert <from> <to> [flags]
```

### Unit Expression
```bash
absoluteunit --express <measurement> [flags]
```

### Unit Simplification
```bash
absoluteunit --simplify <measurement> [flags]
```

### Flags
- `-dec, --decimal <n>`: Set decimal precision
- `-std, --standard`: Use scientific notation
- `-ver, --verbose`: Show conversion steps
- `-d, --debug`: Enable debug output

## 🔧 Technical Details

### Architecture
- **Command Pattern**: Flexible command processing system
- **Factory Pattern**: Robust unit and measurement creation
- **Parser System**: Sophisticated measurement string parsing
- **Validation Layer**: Comprehensive unit compatibility checking

### Type System
```csharp
// Example of the type-safe unit system
public record Unit(IUnitType UnitType, int Exponent = 1, SIPrefix? Prefix = null)
{
    public double ConversionToBase() => UnitType.ToBase() * Prefix.Value;
    public double ConversionFromBase() => UnitType.FromBase() / Prefix.Value;
}
```

### Supported Unit Types
- **SI Base**: meter, kilogram, second, ampere, kelvin, mole, candela
- **SI Derived**: newton, pascal, joule, watt, etc.
- **US Customary**: inch, foot, mile, pound, gallon, etc.
- **Time**: year, month, day, hour, minute

## 🧪 Testing

Extensive test suite covering:
- Unit conversion accuracy
- Parser edge cases
- CLI integration
- Error handling
- Prefix manipulation

```bash
dotnet test
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Open a Pull Request

## 📝 License

This project is MIT licensed. See the [LICENSE](LICENSE.txt) file for details.

## 🔨 Development Roadmap

- [ ] Implement additional derived units
- [ ] Add temperature conversion support
- [ ] Create interactive mode
- [ ] Build web API wrapper
- [ ] Add custom unit definition support

## ✨ Acknowledgments

- SI/US Customary conversion factors from NIST
- Built with .NET 8

## 📫 Contact

Ben Andrew - [andr-be@github.com](mailto:andr-be@github.com)

Project Link: [https://github.com/andr-be/AbsoluteUnit](https://github.com/andr-be/AbsoluteUnit)

---
⭐ If you find this tool useful, please consider giving it a star!