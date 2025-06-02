using System;
using System.Collections.Generic;

namespace PairBrackets
{
    [Flags]
    public enum BracketType
    {
        Round = 1,
        Square = 2,
        Curly = 4
    }

    public static class StringExtensions
    {
        // 1. Use Stack for efficient pair counting.
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

        // 2. Use List to collect tuple positions of all bracket pairs.
        public static List<(int openIndex, int closeIndex)> GetBracketPairPositions(this string text)
        {
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
                        result.Add((openIndex, i));
                    }
                }
            }

            return result;
        }

        // 3. Use HashSet for fast bracket lookup; validate as per bracketTypes flags.
        public static bool ValidateBrackets(this string text, BracketType bracketTypes)
        {
            var typePairs = new Dictionary<BracketType, (char open, char close)>
            {
                { BracketType.Round, ('(', ')') },
                { BracketType.Square, ('[', ']') },
                { BracketType.Curly, ('{', '}') }
            };

            var bracketStack = new Stack<char>();
            var openSet = new HashSet<char>();
            var closeSet = new HashSet<char>();
            foreach (var type in typePairs.Keys)
            {
                if (bracketTypes.HasFlag(type))
                {
                    openSet.Add(typePairs[type].open);
                    closeSet.Add(typePairs[type].close);
                }
            }

            foreach (var c in text)
            {
                if (openSet.Contains(c))
                {
                    bracketStack.Push(c);
                }
                else if (closeSet.Contains(c))
                {
                    if (bracketStack.Count == 0)
                    {
                        return false;
                    }

                    char open = bracketStack.Pop();
                    var expected = typePairs[bracketTypes].open == open ? typePairs[bracketTypes].close : '\0';
                    if (!typePairs.Values.Any(pair => pair.open == open && pair.close == c))
                    {
                        return false;
                    }
                }
            }

            return bracketStack.Count == 0;
        }
    }
}
