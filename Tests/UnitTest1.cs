using EMark;

namespace Tests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("<block></block>", new TokenType[]
        { TokenType.Bracket, TokenType.Element, TokenType.Bracket, TokenType.Bracket, TokenType.Element, TokenType.Bracket })]
        [InlineData("<block> hello world ! </block>", new TokenType[]
        { TokenType.Bracket, TokenType.Element, TokenType.Bracket, TokenType.Text, TokenType.Bracket, TokenType.Element, TokenType.Bracket })]
        [InlineData("<block rows= 4 columns =3> hello world ! </block>", new TokenType[]
        { TokenType.Bracket, TokenType.Element, TokenType.Attribute, TokenType.Equal, TokenType.Character, TokenType.Attribute, TokenType.Equal, TokenType.Character, TokenType.Bracket, TokenType.Text, TokenType.Bracket, TokenType.Element, TokenType.Bracket })]
        [InlineData("<block> <column> hello world !          </column> </block>", new TokenType[]
        { TokenType.Bracket, TokenType.Element, TokenType.Bracket,TokenType.Bracket, TokenType.Element, TokenType.Bracket, TokenType.Text,TokenType.Bracket, TokenType.Element, TokenType.Bracket, TokenType.Bracket, TokenType.Element, TokenType.Bracket })]
        public void TestTokenSequence(string input, TokenType[] result)
        {
            var tokenizer = new Tokenizer();
            Assert.Equal(result, tokenizer.Tokenize(input).Select(t => t.Type));
        }
    }
}