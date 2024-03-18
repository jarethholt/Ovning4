# C# Övning 4 - Minneshantering
---
## Answers to the pre-coding exercise questions:

1. The stack works by holding the data and pointers relevant to current execution. In the context of code execution, the stack holds the methods to be executed in the order to execute them. Methods are popped off the top of the stack after they have run; but if they call other methods while running, those are added to the stack first and have to finish before the current method is considered finished.

    The memory heap is a general region of memory to be dynamically (runtime) allocated that can be accessed globally, not just in the current position of the stack.

2. Value types (or struct) refer to objects that directly contain their data and are usually immutable. Assignments of new variables to this variable "copy by value", i.e. read and store the value without any further impact on this variable.

    Reference types (or class) refer to objects that do not directly contain their data. Instead, variables of a reference type point to the corresponding data in the heap. Assignments for these "copy by reference", i.e. create a pointer to the exact same data, allowing multiple variables the possibility of changing that data for all others.

3. In `ReturnValue`, both `x` and `y` are the native `int` value types, so using `y = x` copies the value of `x` to the value of `y`. Setting `y = 4` afterwards cannot change `x`.
   
    In `ReturnValue2`, both `x` and `y` are now reference types, so using `y = x` aims the pointer for the `y` data to the same place as `x`'s pointer. Then setting `y = 4` changes the data in the pointed location; when `x` retrieves that data, it gets the value 4.

---
## Answers to Övning 1 questions

1. (Question asks to write code.)
2. The list's capacity grows when the current capacity is *exceeded*. For example, if the capacity is 4, it is increased when trying to add the 5th element.
3. The capacity is doubled each time: 1 -> 2 -> 4 -> 8 -> ...
4. Growing the list requires allocating more space for data. It's expected that the list will grow, so it's more efficient to allocate plenty of space at once. The specific capacity increases suggest the list is backed by a binary heap.
5. The capacity is never decreased despite the count falling below any amount of thresholds. As far as I can tell, `List.TrimExcess` is the only way to reduce the capacity of a list.
6. An array is definitely better than a list anytime you know exactly what size you need beforehand. Otherwise I would say lists are a better choice. Since C# has `List.EnsureCapacity`, I don't think there's any amount of knowledge of the _approximate_ size that would make an array more efficient.

---
## Answers to questions 2 and 3

**Exercise 2.1**: For this I will write a queue as `{a, b, c, ...}` and the queue will be added to on the right and removed from on the left.

a. ICA opens with an empty register         : `{}`
b. Kalle gets in line                       : `{Kalle}`
c. Greta gets in line                       : `{Kalle, Greta}`
d. Kalle gets checked out (leaves the queue): `{Greta}`
e. Stine gets in line                       : `{Greta, Stine}`
f. Greta gets checked out                   : `{Stine}`
g. Olle gets in line                        : `{Stine, Olle}`

**Answer to 2.2**: The input to `ExamineQueue` that would simulate this line is:

    +Kalle
    +Greta
    -
    +Stine
    -
    +Olle

**Answer to 3.1**: Using a stack removes the most recently-added member. The same input as 2.2 would lead instead to the following sequence (using the right side as the top of the stack):

    (init) -> {}
    +Kalle -> {Kalle}
    +Greta -> {Kalle, Greta}
    -      -> {Kalle}
    +Stine -> {Kalle, Stine}
    -      -> {Kalle}
    +Olle  -> {Kalle, Olle}

Kalle will never get checked out as long as Ica is busy!

---
## Answer to 4.1

Reading the string from left to right, every time a closer `)/]/}` is encountered, it must match the *most recent unmatched* opener `(/[/{` encountered. If not, you have part of the string like the example `([)]` where the nominally-outer pair `()` is finished before the inner one `[]` is. Once you do find a match for an inner pair, you can ignore the opener; the "most recent unmatched" opener is then rolled back by one.

A stack is the best data structure for this. While reading from left to right, keep a stack of all the openers `(/[/{` encountered. Then:

- When a closer is encountered, it must match the opener on the top of the stack. If not, the string is incorrectly matched.
- If it does match, pop that opener off the stack and continue.
- If the stack is ever empty when a closer is encountered, the closer is unmatchable.
- If the stack is *not* empty after the whole string is parsed, any leftover openers are unmatchable.

Take the string `([{}]({}))` as an example. Going through each character and looking at the resulting stack (without brackets, for clarity):

    Init        -> empty stack
    ( added     -> (
    [ added     -> (, [
    { added     -> (, [, {
    } matches { -> (, [
    ] matches [ -> (
    ( added     -> (, (
    { added     -> (, (, {
    } matches { -> (, (
    ) matches ( -> (
    ) matches ( -> empty

The stack is empty at the end, so the string is correct.

---
## Answer to 5.1

    RecursiveOdd(1) -> (n == 1) -> return 1.
    
    RecursiveOdd(3) -> (n != 1) -> RecursiveOdd(3-1) + 2
      RecursiveOdd(2) -> (n != 1) -> RecursiveOdd(2-1) + 2
        RecursiveOdd(1) -> (n == 1) -> return 1
      RecursiveOdd(2) -> RecursiveOdd(2-1) + 2 = 1 + 2 = 3
    RecursiveOdd(3) -> RecursiveOdd(2) + 2 = 3 + 2 = 5.
    
    RecursiveOdd(5) -> RecursiveOdd(5-1) + 2
      RecursiveOdd(4) -> RecursiveOdd(4-1) + 2 = 5 + 2 = 7
    RecursiveOdd(5) -> RecursiveOdd(4) + 2 = 7 + 2 = 9
    (Skipping past the evaluation of RecursiveOdd(3).)

---
## Answer to 6.1

    IterativeOdd(1):
      result = 1
      for (int i = 0; i < 1-1; i++)
        i = 0; i !< 1-1 ==> stop
      return result = 1
    
    IterativeOdd(3):
      result = 1
      for (int i = 0; i < 3-1; i++)
        i = 0; i < 3-1 ==> result += 2; result = 3
        i = 1; i < 3-1 ==> result += 2; result = 5
        i = 2; i !< 3-1 ==> stop
      return result = 5
    
    IterativeOdd(5):
      result = 1
      for (int i = 0; i < 5-1; i++)
        i = 0; i < 5-1 ==> result += 2; result = 3
        i = 1; i < 5-1 ==> result += 2; result = 5
        i = 2; i < 5-1 ==> result += 2; result = 7
        i = 3; i < 5-1 ==> result += 2; result = 9
        i = 4; i !< 5-1 ==> stop
      return result = 9

---
## Answer to final question:

In their absolute forms, the iterative versions are more *memory*-friendly. They require only a couple of integers (on the stack, and can be overwritten) and no additional method executions to be added to the stack. The recursive versions add new integers with each method call and add the method execution itself to the stack.

However, there are a couple additional things that affect this "memory"-friendliness. First, all of these recursive functions can be tail-call optimized. Since the recursive call is last, it can take the same slot on the stack without having to add a new method call. Second, recursive functions are often cached. The cache takes up space in memory, but can lead to much faster calculation and, in some cases, direct returns with no calculation (and hence memory or CPU overhead) involved.
