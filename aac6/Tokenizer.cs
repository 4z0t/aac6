using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EMark;
public class Tokenizer
{
    public IEnumerable<Token> Tokenize(string code)
    {
        var allRules = Rules.GetAllRules();
        var regexPattern = string.Join("|", allRules.Select(x => $"({x})"));
        Console.WriteLine(regexPattern);
        var regex = Regex.Matches(code, regexPattern, RegexOptions.Singleline);

        foreach (Match item in regex)
        {
            string token = item.Value;

            if (string.IsNullOrWhiteSpace(token)) { yield return new Token(token, TokenType.Space); continue; }
            if (Rules.Bracket.Contains(token)) { yield return new Token(token, TokenType.Bracket); continue; }
            if (Rules.Element.Contains(token)) { yield return new Token(token, TokenType.Element); continue; }
            if (Rules.Attributes.Contains(token)) { yield return new Token(token, TokenType.Attribute); continue; }
            if (Rules.Equal == token) { yield return new Token(token, TokenType.Equal); continue; }
            if (Regex.Match(token, Rules.Character).Success) { yield return new Token(token, TokenType.Character); continue; }


        }
    }

    public static class Rules
    {
        static Rules()
        {
            Attributes = new[] { "rows", "columns", "bgcolor", "width", "height", "valign", "halign", "textcolor" };
            Bracket = new[] { "</", ">", "<" };
            Character = "[^<>\\s]+";
            Equal = "=";
            Space = "[\n\r\t ]+";
            Element = new[] { "block", "column", "row" };
        }

        public static IEnumerable<string> GetAllRules()
        {
            List<string> list = new();

            list.AddRange(Attributes);
            list.Add(Equal);
            list.AddRange(Element);
            list.AddRange(Bracket);
            list.Add(Character);
            list.Add(Space);
            return list;
        }


        public static string Character { get; }
        public static string Equal { get; }
        public static string Space { get; }
        public static string[] Element { get; }
        public static string[] Attributes { get; }
        public static string[] Bracket { get; }
    }


}


public enum TokenType
{
    Character,
    Attribute,
    Equal,
    Space,
    Element,
    Bracket,
    Text
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
