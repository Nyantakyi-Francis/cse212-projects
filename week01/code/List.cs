using System;
using System.Collections.Generic;
using System.Linq; // Required for Console.Out.WriteLine in Run method

public static class Lists
{
    public static void Run()
    {
        // Example usage as per assignment description
        var data1 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        Console.WriteLine($"Original List 1: {string.Join(", ", data1)}");
        RotateListRight(data1, 5);
        Console.WriteLine($"Rotated List 1 (amount 5): {string.Join(", ", data1)}"); // Expected: {5, 6, 7, 8, 9, 1, 2, 3, 4}

        Console.WriteLine(); // Add a newline for readability

        var data2 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        Console.WriteLine($"Original List 2: {string.Join(", ", data2)}");
        RotateListRight(data2, 3);
        Console.WriteLine($"Rotated List 2 (amount 3): {string.Join(", ", data2)}"); // Expected: {7, 8, 9, 1, 2, 3, 4, 5, 6}

        Console.WriteLine(); // Add a newline for readability

        var data3 = new List<int> { 10, 20, 30, 40 };
        Console.WriteLine($"Original List 3: {string.Join(", ", data3)}");
        RotateListRight(data3, 1);
        Console.WriteLine($"Rotated List 3 (amount 1): {string.Join(", ", data3)}"); // Expected: {40, 10, 20, 30}

        Console.WriteLine(); // Add a newline for readability

        var data4 = new List<int> { 10, 20, 30, 40 };
        Console.WriteLine($"Original List 4: {string.Join(", ", data4)}");
        RotateListRight(data4, 4); // Rotate by full length, should result in original order
        Console.WriteLine($"Rotated List 4 (amount 4): {string.Join(", ", data4)}"); // Expected: {10, 20, 30, 40}
    }

    /// <summary>
    /// Rotates the elements of a list to the right by a specified amount.
    /// The rotation is performed in-place.
    /// </summary>
    /// <param name="data">The list of integers to rotate.</param>
    /// <param name="amount">The number of positions to rotate to the right.
    /// This value will be in the range of 1 and data.Count, inclusive.</param>
    private static void RotateListRight(List<int> data, int amount)
    {
        // Plan (using GetRange, RemoveRange, AddRange as hinted):
        // 1. Handle edge cases: if the list is empty or has only one element, no rotation is needed.
        //    Also, if amount is a multiple of data.Count, the list remains unchanged.
        //    The problem states amount is between 1 and data.Count, so we simplify the modulo for effectiveAmount.
        // 2. Calculate the effective rotation amount. This handles cases where 'amount' might be
        //    greater than the list's length (though the problem states it won't be, it's good practice).
        //    Since amount is guaranteed to be 1 to data.Count, effectiveAmount will just be 'amount'.
        // 3. Determine the number of elements to move from the end to the front. This is 'effectiveAmount'.
        // 4. Extract these 'moved' elements into a new temporary list using GetRange.
        // 5. Remove these elements from their original position at the end of the 'data' list using RemoveRange.
        // 6. Insert the 'moved' elements at the beginning of the 'data' list using InsertRange.

        // 1. Handle edge cases: If the list is empty or has only one element, no rotation is needed.
        if (data == null || data.Count <= 1)
        {
            return;
        }

        // 2. Calculate the effective rotation amount.
        // Since 'amount' is guaranteed to be between 1 and data.Count,
        // 'amount % data.Count' will correctly give the effective rotation.
        // If amount == data.Count, effectiveAmount will be 0, meaning no actual rotation is needed.
        int effectiveAmount = amount % data.Count;

        // If effectiveAmount is 0, it means the list rotates back to its original position.
        if (effectiveAmount == 0)
        {
            return;
        }

        // 3. Determine the starting index of elements to move from the end.
        // These are the last 'effectiveAmount' elements.
        int startIndexToMove = data.Count - effectiveAmount;

        // 4. Extract these 'moved' elements into a new temporary list using GetRange.
        // GetRange(index, count)
        // index: The zero-based starting index of the range to get.
        // count: The number of elements in the range.
        List<int> movedElements = data.GetRange(startIndexToMove, effectiveAmount);

        // 5. Remove these elements from their original position at the end of the 'data' list using RemoveRange.
        // RemoveRange(index, count)
        // index: The zero-based starting index of the range of elements to remove.
        // count: The number of elements to remove.
        data.RemoveRange(startIndexToMove, effectiveAmount);

        // 6. Insert the 'moved' elements at the beginning of the 'data' list using InsertRange.
        // InsertRange(index, collection)
        // index: The zero-based index at which the new elements should be inserted.
        // collection: The collection whose elements should be inserted.
        data.InsertRange(0, movedElements);
    }
}
