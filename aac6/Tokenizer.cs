using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static Tokenizer;

public class Tokenizer
{
    public IEnumerable<Token> Tokenize(string code)
    {
        var allRules = Rules.GetAllRules();
        var regexPattern = string.Join("|", allRules.Select(x =>$"({x})"));
        Console.WriteLine(regexPattern);
        var regex = Regex.Matches(code, regexPattern, RegexOptions.Singleline);

        foreach (Match item in regex)
        {
            string token = item.Value;

            if (string.IsNullOrWhiteSpace(token)) { yield return new Token(token, TokenType.Space); continue; }
            if (Rules.ElementSep.Contains(token)) { yield return new Token(token, TokenType.ElementSep); continue; }
            if (Rules.Syntaxis.Contains(token)) { yield return new Token(token, TokenType.Syntaxis); continue; }
            if (Rules.Attributes.Contains(token)) { yield return new Token(token, TokenType.Attribute); continue; }
            if (Rules.Equal == (token)) { yield return new Token(token, TokenType.Equal); continue; }
            if (Regex.Match(token, Rules.Character).Success) { yield return new Token(token, TokenType.Character); continue; }


        }
    }

    public static class Rules
    {
        static Rules()
        {
            Attributes = new[] { "rows", "columns", "bgcolor", "width", "height", "valign", "halign", "textcolor" };
            ElementSep = "</>".Select(x => x.ToString()).ToArray();
            Character = "[^<>\\s]+";
            Equal = "=";
            Space = "[\n\r\t ]+";
            Syntaxis = new[] { "block", "column", "row" };
        }

        public static IEnumerable<string> GetAllRules()
        {
            List<string> list = new();

            list.AddRange(Attributes);
            list.Add(Equal);
            list.AddRange(Syntaxis);
            list.AddRange(ElementSep);
            list.Add(Character);
            list.Add(Space);
            return list;
        }


        public static string Character { get; }
        public static string Equal { get; }
        public static string Space { get; }
        public static string[] Syntaxis { get; }
        public static string[] Attributes { get; }
        public static string[] ElementSep { get; }
    }

    public enum TokenType
    {
        Character,
        Attribute,
        Equal,
        Space,
        Syntaxis,
        ElementSep
    }
}



public class Token
{
    public Token(string token, TokenType identity)
    {
        TokenString = token;
        Type = identity;
    }

    public string TokenString { get; }
    public TokenType Type { get; }

    public override string ToString()
    {
        return $"{TokenString}|{Type}";
    }
}
