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

        public void TestTokenSequence(string input, TokenType[] result)
        {
            var tokenizer = new Tokenizer();
            var r = tokenizer.Tokenize(input);
            Assert.Equal(result, r.Select(t => t.Type));
        }
    }
}