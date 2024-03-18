namespace Ovning4;

class Program
{
    private enum MenuOption
    {
        Exit,
        ExamineList,
        ExamineQueue,
        ExamineStack,
        CheckParentheses,
        ReverseText
    }
    private static readonly int _maxOptionLength =
        Enum.GetValues<MenuOption>().Select(option => $"{option}".Length).Max();
    private static readonly Dictionary<MenuOption, string> _descriptions = new()
    {
        { MenuOption.Exit            , "Exit the application."                  },
        { MenuOption.ExamineList     , "Examine the List type."                 },
        { MenuOption.ExamineQueue    , "Examine the Queue type."                },
        { MenuOption.ExamineStack    , "Examine the Stack type."                },
        { MenuOption.CheckParentheses, "Check a string for matching enclosers." },
        { MenuOption.ReverseText     , "Reverse a piece of text."               },
    };
    private static readonly Dictionary<MenuOption, Action> _actions = new()
    {
        { MenuOption.Exit            , Exit                },
        { MenuOption.ExamineList     , PushPop.ExamineList },
        { MenuOption.ExamineQueue    , PushPop.ExamineQueue},
        { MenuOption.ExamineStack    , PushPop.ExamineStack},
        { MenuOption.CheckParentheses, CheckParentheses    },
        { MenuOption.ReverseText     , ReverseText         },
    };

    static void Exit() => Environment.Exit(0);

    static void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine("Please choose one of the options below.");
        string format = $"{{0}} : {{1,-{_maxOptionLength}}} : {{2}}";
        foreach (KeyValuePair<MenuOption, string> kvp in _descriptions)
        {
            MenuOption option = kvp.Key;
            string description = kvp.Value;
            Console.WriteLine(format, (int)option, option, description);
        }
    }

    /// <summary>Handle the menus for the program.</summary>
    static void Main()
    {
        while (true)
        {
            // Display the menu and ask for a choice
            DisplayMenu();

            string? readResult;
            MenuOption option;
            while (true)
            {
                readResult = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(readResult))
                {
                    Console.WriteLine("Please enter some input!");
                    continue;
                }
                readResult = readResult.Trim();

                bool success = Enum.TryParse(readResult, out option);
                if (success)
                    success &= Enum.IsDefined(option);
                if (!success)
                {
                    Console.WriteLine($"'{readResult}' is not a valid menu option.");
                    continue;
                }
                break;
            }

            _actions[option]();
        }
    }

    private static readonly Dictionary<char, char> _closerToOpener = new()
    {
        { ')', '(' },
        { ']', '[' },
        { '}', '{' }
    };

    /// <summary>Check if the various enclosers (), [], {} in a string are matched.</summary>
    static void CheckParentheses()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Examine if the enclosers in a string are correct.");
            Console.WriteLine("Please provide a string to check:");
            string? readResult = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(readResult))
            {
                Console.WriteLine("Please enter non-empty text:");
                readResult = Console.ReadLine();
            }

            // Keep a stack of the openers; check each closer against the top of the stack
            bool correct = true;
            Stack<(char,int)> openers = new();
            for (int currPosition = 0; currPosition < readResult.Length; currPosition++)
            {
                char curr = readResult[currPosition];
                if (_closerToOpener.ContainsValue(curr))
                {
                    openers.Push((curr,currPosition));
                    continue;
                }
                if (!_closerToOpener.ContainsKey(curr))
                    continue;

                // Are there any openers left on the stack?
                if (openers.Count == 0)
                {
                    // Draw an arrow pointing out the unmatchable closer
                    string arrowFormat = $"{{0,{currPosition+1}}}";
                    Console.WriteLine(string.Format(arrowFormat, '1'));
                    Console.WriteLine("This closer (1) is unmatchable.");
                    correct = false;
                    break;
                }

                // Does the closer curr match the most recent opener?
                (char lastOpener, int openerPosition) = openers.Pop();
                if (lastOpener != _closerToOpener[curr])
                {
                    // Draw arrows pointing out the mismatch
                    string arrowFormat
                        = $"{{0,{openerPosition+1}}}{{1,{currPosition - openerPosition}}}";
                    Console.WriteLine(string.Format(arrowFormat, '1', '2'));
                    Console.WriteLine("The closer (2) does not match the most recent opener (1).");
                    correct = false;
                    break;
                }
            }
            // If any openers are left over, they are unmatchable
            if (openers.Count != 0)
            {
                (_, int openerPosition) = openers.Pop();
                string arrowFormat = $"{{0,{openerPosition+1}}}";
                Console.WriteLine(string.Format(arrowFormat, '1'));
                Console.WriteLine("This opener (1) is unmatchable.");
                correct = false;
            }

            if (correct)
                Console.WriteLine("The given text has correctly matched enclosers.");
            else
                Console.WriteLine("The given text does not have correctly matched enclosers.");

            do
            {
                Console.Write("Would you like to input another string [y/n]? ");
                readResult = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(readResult))
                {
                    Console.WriteLine("Please use non-empty input.");
                    continue;
                }

                char answer = readResult.ToLower()[0];
                if (answer == 'n')
                    return;
                else if (answer == 'y')
                    break;
                else
                    Console.WriteLine("Please enter an input starting with 'y' or 'n'.");
            } while (true);
        }
    }

    /// <summary>Take a string from the user and reverse it using a Stack.</summary>
    static void ReverseText()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Reverse text with help of a Stack.");
            Console.WriteLine("Please enter some text to reverse:");
            string? readResult = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(readResult))
            {
                Console.WriteLine("Please enter non-empty text:");
                readResult = Console.ReadLine();
            }

            // Construct a stack, feeding it the characters in readResult
            Stack<char> chars = new(readResult.ToCharArray());
            // Values are added to the "front" of the stack, so ToArray will reverse the characters
            string reversed = new(chars.ToArray());

            Console.WriteLine($"The reversed text: {reversed}");
            Console.WriteLine();

            do
            {
                Console.Write("Would you like to input another string [y/n]? ");
                readResult = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(readResult))
                {
                    Console.WriteLine("Please use non-empty input.");
                    continue;
                }

                char answer = readResult.ToLower()[0];
                if (answer == 'n')
                    return;
                else if (answer == 'y')
                    break;
                else
                    Console.WriteLine("Please enter an input starting with 'y' or 'n'.");
            } while (true);
        }
    }

    static int RecursiveEven(int n) => n == 1 ? 2 : RecursiveEven(n - 1) + 2;

    static int RecursiveFibonacci(int n)
    {
        if (n == 0) return 1;
        else if (n == 1) return 1;
        else return RecursiveFibonacci(n - 1) + RecursiveFibonacci(n - 2);
    }

    static int IterativeEven(int n)
    {
        int result = 2;
        for (int i = 0; i < n - 1; i++) result += 2;
        return result;
    }

    static int IterativeFibonacci(int n)
    {
        int result0 = 1;
        int result1 = 1;
        if (n == 0) return result0;
        else if (n == 1) return result1;

        for (int i = 1; i < n; i++)
        {
            int temp = result1;
            result1 += result0;
            result0 = temp;
        }
        return result1;
    }

}

