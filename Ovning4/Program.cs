using System.Collections;

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
        { MenuOption.ReverseText     , "Reverse a piece of text."               }
    };
    private static readonly Dictionary<MenuOption, Action> _actions = new()
    {
        { MenuOption.Exit            , Exit             },
        { MenuOption.ExamineList     , ExamineList      },
        { MenuOption.ExamineQueue    , ExamineQueue     },
        { MenuOption.ExamineStack    , ExamineStack     },
        { MenuOption.CheckParentheses, CheckParentheses },
        { MenuOption.ReverseText     , ReverseText      },
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

    // Interface to makes a class look like a stack
    private interface IStackLike<T> : IReadOnlyCollection<T>
    {
        public string BaseName { get; }
        public void Push(T item);
        public T? Pop(T item);
    }

    // Inner loop method to update a StackLike object
    private static bool UpdateStackLike<T>(
        T stackLike,
        Action<T> callback
    ) where T : IStackLike<string>
    {
        bool again = true;
        string? readResult = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(readResult))
        {
            Console.WriteLine("Empty input, try again.");
            return again;
        }
        char choice = readResult[0];
        // Using Substring should be safe here even for strings of length 1
        string value = readResult[1..];

        switch (choice)
        {
            case '+':
                stackLike.Push(value);
                Console.Write($"Added {value} to the {stackLike.BaseName}. ");
                break;
            case '-':
                if (stackLike.Count == 0)
                    Console.Write($"Cannot pop an item, the {stackLike.BaseName} is empty. ");
                else
                {
                    string? popped = stackLike.Pop(value);
                    if (popped is null)
                        Console.Write($"{value} is not in the {stackLike.BaseName}. ");
                    else
                        Console.Write($"Popped {popped} from the {stackLike.BaseName}. ");
                }
                break;
            case '0':
                Console.WriteLine("Press enter to return to the main menu.");
                _ = Console.ReadLine();
                again = false;
                return again;
            default:
                Console.WriteLine($"Could not parse choice {choice}, try again.");
                return again;
        }

        callback(stackLike);
        return again;
    }

    // Main menu method to examine an IStackLike.
    private static void ExamineStackLike<T>(
        T stackLike,
        Action<T> callback
    ) where T : IStackLike<string>
    {
        Console.Clear();
        Console.WriteLine(
            $"Examine a {stackLike.BaseName} of strings.\n"
            + $"To add 'value' to the {stackLike.BaseName}, type '+value'.\n"
            + $"To pop an entry from the {stackLike.BaseName}, type '-value'.\n"
            + "To exit to the main menu, type '0'.\n"
        );
        
        bool again;
        do
        {
            again = UpdateStackLike(stackLike, callback);
        } while (again);
    }

    private class ListToStackAdapter<T> : IStackLike<T>
    {
        private readonly List<T> _list = [];
        public string BaseName => "List";
        public int Count => _list.Count;
        public int Capacity => _list.Capacity;

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        public T? Pop(T item)
        {
            bool success = _list.Remove(item);
            return success ? item : default;
        }

        public void Push(T item) => _list.Add(item);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    private static readonly Action<ListToStackAdapter<string>> LCallback = new(stackLike =>
    {
        Console.WriteLine($"Count: {stackLike.Count}; Capacity: {stackLike.Capacity}");
    });

    /// <summary>Examines the datastructure List.</summary>
    /// <remarks>
    /// Allow the user to manipulate a List of strings. The available actions are,
    /// like the main menu, controlled by the first character of each line:
    ///   '+': Add the rest of the input to the list.
    ///        For example, writing '+Adam' would add "Adam" to the list.
    ///   '-': (Attempt to) remove an item from the list.
    ///        For example, writing '-Adam' would remove "Adam" from the list
    ///        if present and print an error otherwise.
    ///   '0': Exit to the main menu.
    /// Entering any other character (or nothing) as the first character will
    /// print an error message and do nothing.
    /// After each item is added, the Count and Capacity of the list are printed.
    /// </remarks>
    static void ExamineList()
    {
        ListToStackAdapter<string> stackLike = new();
        ExamineStackLike(stackLike, LCallback);
    }

    // Adapter to make a Queue look like a Stack
    private class QueueToStackAdapter<T> : IStackLike<T>
    {
        private readonly Queue<T> _queue = new();
        public string BaseName => "Queue";
        public int Count => _queue.Count;

        public IEnumerator<T> GetEnumerator() => _queue.GetEnumerator();

        public T? Pop() => _queue.Dequeue();
        public T? Pop(T item) => this.Pop();

        public void Push(T item) => _queue.Enqueue(item);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    private static readonly Action<IStackLike<string>> QSCallback = new(stackLike =>
    {
        string stackLikeString = string.Join(", ", [.. stackLike]);
        Console.WriteLine($"Current {stackLike.BaseName}: {stackLikeString}");
    });

    /// <summary>Examine the datastructure Queue.</summary>
    /// <remarks>
    /// Allow the user to manipulate a Queue of strings. The available actions are,
    /// like the main menu, controlled by the first character of each input line:
    ///   '+': Add the rest of the input to the queue.
    ///        For example, writing '+Adam' would add "Adam" to the queue.
    ///   '-': Pop the item at the front of the queue.
    ///        Gives an error message if the queue is empty.
    ///   '0': Exit to the main menu.
    /// Entering any other character (or nothing) as the first character will
    /// print an error message and do nothing.
    /// After each change the state of the queue is printed.
    /// </remarks>
    static void ExamineQueue()
    {
        QueueToStackAdapter<string> stackLike = new();
        ExamineStackLike(stackLike, QSCallback);
    }

    // Adapter to make a Stack look like a Stack
    private class StackToStackAdapter<T> : IStackLike<T>
    {
        private readonly Stack<T> _stack = new();
        public string BaseName => "Stack";

        public int Count => _stack.Count;

        public IEnumerator<T> GetEnumerator() => _stack.GetEnumerator();

        public T Pop() => _stack.Pop();
        public T? Pop(T item) => this.Pop();

        public void Push(T item) => _stack.Push(item);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    /// <summary>Examine the datastructure Stack.</summary>
    /// <remarks>
    /// Allow the user to manipulate a Stack of strings. The available actions are,
    /// like the main menu, controlled by the first character of each input line:
    ///   '+': Add the rest of the input to the stack.
    ///        For example, writing '+Adam' would add "Adam" to the stack.
    ///   '-': Pop the item at the front of the stack.
    ///        Gives an error message if the stack is empty.
    ///   '0': Exit to the main menu.
    /// Entering any other character (or nothing) as the first character will
    /// print an error message and do nothing.
    /// After each change the state of the stack is printed.
    /// </remarks>
    static void ExamineStack()
    {
        StackToStackAdapter<string> stackLike = new();
        ExamineStackLike(stackLike, QSCallback);
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

