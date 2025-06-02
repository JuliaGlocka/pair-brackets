namespace PairBrackets
{
    public static class StringExtensions
    {
        public static int CountBracketPairs(this string text)
        {
            if (text == null)
            {
                return 0;
            }

            var pairs = new Dictionary<char, char> { { '(', ')' }, { '[', ']' } };
            var stack = new Stack<char>();
            int count = 0;
            foreach (var c in text)
            {
                if (pairs.ContainsKey(c))
                {
                    stack.Push(c);
                }
                else if (pairs.ContainsValue(c) && stack.Count > 0 && pairs[stack.Peek()] == c)
                {
                    stack.Pop();
                    count++;
                }
            }

            return count;
        }

        public static List<(int openIndex, int closeIndex)> GetBracketPairPositions(this string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var pairs = new Dictionary<char, char> { { '(', ')' }, { '[', ']' }, { '{', '}' } };
            var stack = new Stack<(char bracket, int index)>();
            var result = new List<(int, int)>();

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (pairs.ContainsKey(c))
                {
                    stack.Push((c, i));
                }
                else if (pairs.ContainsValue(c))
                {
                    if (stack.Count > 0 && pairs.TryGetValue(stack.Peek().bracket, out var expected) && expected == c)
                    {
                        var (openBracket, openIndex) = stack.Pop();
                        // Insert at beginning to get pairs in opening order
                        result.Insert(0, (openIndex, i));
                    }
                }
            }

            return result;
        }

        public static bool ValidateBrackets(this string text, BracketTypes bracketTypes)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var typePairs = new Dictionary<BracketTypes, (char open, char close)>
            {
                { BracketTypes.RoundBrackets, ('(', ')') },
                { BracketTypes.SquareBrackets, ('[', ']') },
                { BracketTypes.CurlyBrackets, ('{', '}') },
                { BracketTypes.AngleBrackets, ('<', '>') }
            };

            var openSet = new HashSet<char>();
            var closeSet = new HashSet<char>();
            foreach (var pair in typePairs)
            {
                if (bracketTypes == BracketTypes.All || bracketTypes.HasFlag(pair.Key))
                {
                    openSet.Add(pair.Value.open);
                    closeSet.Add(pair.Value.close);
                }
            }

            var stack = new Stack<char>();
            foreach (var c in text)
            {
                if (openSet.Contains(c))
                {
                    stack.Push(c);
                }
                else if (closeSet.Contains(c))
                {
                    if (stack.Count == 0)
                    {
                        return false;
                    }
                    char open = stack.Pop();
                    var expected = typePairs.Values.FirstOrDefault(x => x.close == c).open;
                    if (open != expected)
                    {
                        return false;
                    }
                }
            }

            return stack.Count == 0;
        }
    }
}
