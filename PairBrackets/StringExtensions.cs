using System;
using System.Collections.Generic;
using System.Linq;

namespace PairBrackets
{
    public static class StringExtensions
    {
        public static int CountBracketPairs(this string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            Stack<char> stack = new();
            int pairCount = 0;

            foreach (char c in text)
            {
                if (c == '(' || c == '{' || c == '[')
                {
                    stack.Push(c);
                }
                else if (c == ')' || c == '}' || c == ']')
                {
                    if (stack.Count > 0 && IsMatchingPair(stack.Peek(), c))
                    {
                        stack.Pop();
                        pairCount++;
                    }
                }
            }

            return pairCount;
        }

        private static bool IsMatchingPair(char open, char close)
        {
            return (open == '(' && close == ')') ||
                   (open == '{' && close == '}') ||
                   (open == '[' && close == ']');
        }
    }
}
