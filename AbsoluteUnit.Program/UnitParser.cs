﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AbsoluteUnit
{
    public partial class UnitParser
    {
        public UnitParser() { } 
    }

    public class MeasurementParser
    {
        public MeasurementGroup Measurement { get; }

        public MeasurementParser(string measurementString) 
        {
            Measurement = new(measurementString);
 //           foreach (var unit in Measurement.Units)
        }
    }

    public partial class UnitGroups
    {
        private const string UnitGroupRegexString = @"([ ./])?([A-Za-zµ°Ω]+)+(?:(?:\^)?|(?:\*\*))?((?:\+|-)?\d+)?";
        [GeneratedRegex(UnitGroupRegexString)]
        private static partial Regex Regex();

        public List<UnitGroup> Groups { get; set; } = [];

        public UnitGroups(string unitString)
        {
            if (string.IsNullOrEmpty(unitString))
                throw new Exception("no unitString provided");

            Groups = ParseUnitGroups(unitString);
        }

        private static List<UnitGroup> ParseUnitGroups(string unitString)
        {
            List<UnitGroup> groups = [];
            MatchCollection matches = Regex().Matches(unitString);
            if (matches.Count == 0)
                throw new Exception($"parsing error: {unitString} produced no matches");

            foreach (Match match in matches.Cast<Match>())
            {
                UnitGroup.DivMulti divMulti = match.Groups[1].Success
                    ? UnitGroup.GetDivMulti(match.Groups[1].Value.FirstOrDefault())
                    : UnitGroup.DivMulti.Multiply;

                int exponent = match.Groups[3].Success
                    ? int.Parse(match.Groups[3].Value)
                    : 1;

                string unitSymbol = match.Groups[2].Value;

                UnitGroup newGroup = new(divMulti, unitSymbol, exponent);
                groups.Add(newGroup);
            }
            return groups;
        }
    }

    public partial class MeasurementGroup
    {
        public double Quantity { get; } 
        public int Exponent { get; }
        public UnitGroups Units { get; }

        private const string MeasurementRegexString = @"^(-?\d+[,\d]*\.?\d*)+(?:e(-?\d+))? *([A-Za-zµ°Ω]+[\w\d.*^\-\/]*)+$";
        [GeneratedRegex(MeasurementRegexString)]
        private static partial Regex Regex();

        public MeasurementGroup(string measurement)
        {
            var match = Regex().Match(measurement);
            if (!match.Success) 
                throw new Exception($"Couldn't match anything in [{measurement}]");

            try
            {
                Quantity = ParseQuantity(match.Groups[1].Value);
                Exponent = ParseExponent(match.Groups[2].Value);
                Units = new UnitGroups(match.Groups[3].Value);
            }
            catch (Exception e)
            {
                throw new Exception($"Unable to correctly parse MeasurementGroup for {measurement}", innerException: e);
            }
        }

        private static int ParseExponent(string exponentString) => int.Parse(exponentString);

        private static double ParseQuantity(string quantityString) => double.Parse(quantityString);
    }

    public record UnitGroup(UnitGroup.DivMulti _DivMulti, string UnitSymbol, int Exponent) 
    {
        public static DivMulti GetDivMulti(char c)
        {
            return c switch
            {
                '/' => DivMulti.Divide,
                '.' => DivMulti.Multiply,
                _ => throw new Exception("invalid DivMulti symbol")
            };
        }
        public enum DivMulti
        {
            Divide,
            Multiply,
        }
    }
}