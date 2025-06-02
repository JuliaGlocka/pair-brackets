namespace PairBrackets
{
    public static class StringExtensions
    {
        public static IList<(int, int)> GetBracketPairPositions(this string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var stack = new Stack<(char Bracket, int Index)>();
            var pairs = new List<(int, int)>();
            var matchingBrackets = new Dictionary<char, char>
            {
                { ')', '(' },
                { ']', '[' },
                { '}', '{' }
            };

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (matchingBrackets.ContainsValue(c)) // Opening brackets
                {
                    stack.Push((c, i));
                }
                else if (matchingBrackets.ContainsKey(c)) // Closing brackets
                {
                    if (stack.Count > 0 && stack.Peek().Bracket == matchingBrackets[c])
                    {
                        var opening = stack.Pop();
                        pairs.Add((opening.Index, i));
                    }
                }
            }

            return pairs;
        }

        public static int CountBracketPairs(this string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var stack = new Stack<char>();
            int count = 0;

            foreach (var ch in text)
            {
                if (ch == '(' || ch == '{' || ch == '[')
                {
                    stack.Push(ch);
                }
                else if (ch == ')' || ch == '}' || ch == ']')
                {
                    if (stack.Count > 0 && IsMatchingPair(stack.Peek(), ch))
                    {
                        stack.Pop();
                        count++;
                    }
                }
            }

            return count;
        }

        private static bool IsMatchingPair(char open, char close)
        {
            return (open == '(' && close == ')') ||
                   (open == '{' && close == '}') ||
                   (open == '[' && close == ']');
        }
    }
}
