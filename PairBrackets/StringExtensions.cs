using System.Collections.ObjectModel;

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
                    _ = stack.Pop(); // Explicitly discard the value to avoid IDE0058
                    count++;
                }
            }

            return count;
        }

        public static ReadOnlyCollection<(int openIndex, int closeIndex)> GetBracketPairPositions(this string text)
        {
            ArgumentNullException.ThrowIfNull(text);

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
                else if (pairs.ContainsValue(c) && stack.Count > 0 && pairs.TryGetValue(stack.Peek().bracket, out var expected) && expected == c)
                {
                    var (_, openIndex) = stack.Pop();
                    result.Add((openIndex, i));
                }
            }

            // The test expects pairs sorted by openIndex (outermost first)
            result.Sort((a, b) => a.Item1.CompareTo(b.Item1));
            return new ReadOnlyCollection<(int, int)>(result);
        }

        public static bool ValidateBrackets(this string text, BracketTypes bracketTypes)
        {
            ArgumentNullException.ThrowIfNull(text);

            // Only use the bracket types requested
            var typePairs = new Dictionary<BracketTypes, (char open, char close)>
            {
                { BracketTypes.RoundBrackets, ('(', ')') },
                { BracketTypes.SquareBrackets, ('[', ']') },
                { BracketTypes.CurlyBrackets, ('{', '}') },
                { BracketTypes.AngleBrackets, ('<', '>') },
            };

            var openSet = new HashSet<char>();
            var closeSet = new HashSet<char>();
            var typeMap = new Dictionary<char, char>(); // close -> open

            foreach (var pair in typePairs)
            {
                if (bracketTypes == BracketTypes.All || bracketTypes.HasFlag(pair.Key))
                {
                    _ = openSet.Add(pair.Value.open); // Explicitly discard the return value to avoid IDE0058
                    _ = closeSet.Add(pair.Value.close); // Explicitly discard the return value to avoid IDE0058
                    typeMap[pair.Value.close] = pair.Value.open;
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
                    if (open != typeMap[c])
                    {
                        return false;
                    }
                }
            }

            return stack.Count == 0;
        }
    }
}
