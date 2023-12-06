using EMark;

namespace Tests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("<block></block>", new TokenType[] 
        { TokenType.Bracket, TokenType.Element, TokenType.Bracket, TokenType.Bracket, TokenType.Element, TokenType.Bracket })]

        public void TestTokenSequence(string input, TokenType[] result)
        {
            var tokenizer = new Tokenizer();
            var r = tokenizer.Tokenize(input);
            int index = 0;
            var l = Program.ScanTokens(r);
            foreach (var t in result)
            {
                Assert.Equal(t, l[index].Type);
                index++;
            }

        }
    }
}