using System;
using System.Collections.Generic;
using System.Linq;

namespace PairBrackets
{
    public static class StringExtensions
    {
        // ...other methods...

        public static bool ValidateBrackets(this string text, BracketTypes bracketTypes)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            // Only use the bracket types requested
            var typePairs = new Dictionary<BracketTypes, (char open, char close)>
            {
                { BracketTypes.RoundBrackets, ('(', ')') },
                { BracketTypes.SquareBrackets, ('[', ']') },
                { BracketTypes.CurlyBrackets, ('{', '}') },
                { BracketTypes.AngleBrackets, ('<', '>') },
            };

            // Find only the open/close chars for the selected types
            var openSet = new HashSet<char>();
            var closeSet = new HashSet<char>();
            var closeToOpen = new Dictionary<char, char>();

            foreach (var pair in typePairs)
            {
                if (bracketTypes == BracketTypes.All || bracketTypes.HasFlag(pair.Key))
                {
                    openSet.Add(pair.Value.open);
                    closeSet.Add(pair.Value.close);
                    closeToOpen[pair.Value.close] = pair.Value.open;
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
                    var open = stack.Pop();
                    if (open != closeToOpen[c])
                    {
                        return false;
                    }
                }

                // Ignore any brackets not in the selected type
            }

            return stack.Count == 0;
        }
    }
}
