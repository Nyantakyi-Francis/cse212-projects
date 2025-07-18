using System;
using System.Collections.Generic; // Required for List<T>
using System.Linq; // Required for string.Join and List.Sum() if used elsewhere

public static class Arrays
{
    public static void Run()
    {
        // --- Example usage for MultiplesOf ---
        var multiples1 = MultiplesOf(3, 5);
        Console.Out.WriteLine("MultiplesOf(3, 5): double[]{{{0}}}", string.Join(", ", multiples1)); // Expected: double[]{3, 6, 9, 12, 15}

        // Example matching the TestMultiplesOf_Fractional test
        var multiples2 = MultiplesOf(1.5, 10);
        Console.Out.WriteLine("MultiplesOf(1.5, 10): double[]{{{0}}}", string.Join(", ", multiples2)); // Expected: double[]{1.5, 3.0, 4.5, 6.0, 7.5, 9.0, 10.5, 12.0, 13.5, 15.0}

        // Example matching the TestMultiplesOf_Negative test
        var multiples3 = MultiplesOf(-2, 10);
        Console.Out.WriteLine("MultiplesOf(-2, 10): double[]{{{0}}}", string.Join(", ", multiples3)); // Expected: double[]{-2, -4, -6, -8, -10, -12, -14, -16, -18, -20}

        Console.WriteLine(); // Add a newline for readability

        // --- Example usage for RotateListRight ---
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

        var data3 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        Console.WriteLine($"Original List 3: {string.Join(", ", data3)}");
        RotateListRight(data3, 9); // Rotate by full length, should result in original order
        Console.WriteLine($"Rotated List 3 (amount 9): {string.Join(", ", data3)}"); // Expected: {1, 2, 3, 4, 5, 6, 7, 8, 9}

        Console.WriteLine(); // Add a newline for readability

        var data4 = new List<int> { 10, 20, 30, 40 };
        Console.WriteLine($"Original List 4: {string.Join(", ", data4)}");
        RotateListRight(data4, 1);
        Console.WriteLine($"Rotated List 4 (amount 1): {string.Join(", ", data4)}"); // Expected: {40, 10, 20, 30}
    }

    /// <summary>
    /// Creates and returns an array of multiples of a number.
    /// The starting number and the number of multiples are provided as inputs.
    /// </summary>
    /// <param name="startingNumber">The number to find multiples of (can be fractional).</param>
    /// <param name="count">The number of multiples to generate.</param>
    /// <returns>An array of doubles containing the multiples.</returns>
    public static double[] MultiplesOf(double startingNumber, int count) // Changed from private to public
    {
        // Plan:
        // 1. Create a new double array of the specified 'count' size.
        // 2. Loop from 0 to count - 1 (inclusive).
        // 3. In each iteration, calculate the current multiple: startingNumber * (index + 1).
        // 4. Store this calculated multiple in the array at the current index.
        // 5. After the loop, return the populated array.

        // 1. Create a new double array of the specified 'count' size.
        double[] resultArray = new double[count];

        // 2. Loop from 0 to count - 1 (inclusive).
        for (int i = 0; i < count; i++)
        {
            // 3. In each iteration, calculate the current multiple: startingNumber * (i + 1).
            // The (i + 1) is used because we want the 1st multiple, 2nd multiple, etc.,
            // not the 0th multiple.
            resultArray[i] = startingNumber * (i + 1);
        }

        // 5. After the loop, return the populated array.
        return resultArray;
    }

    /// <summary>
    /// Rotates the elements of a list to the right by a specified amount.
    /// The rotation is performed in-place.
    /// </summary>
    /// <param name="data">The list of integers to rotate.</param>
    /// <param name="amount">The number of positions to rotate to the right.
    /// This value will be in the range of 1 and data.Count, inclusive.</param>
    public static void RotateListRight(List<int> data, int amount) // Changed from private to public
    {
        // Plan (using GetRange, RemoveRange, AddRange as hinted):
        // 1. Handle edge cases: if the list is null, empty, or has only one element, no rotation is needed.
        // 2. Calculate the effective rotation amount. This handles cases where 'amount' might be
        //    greater than the list's length (though the problem states it won't be, it's good practice).
        //    Since amount is guaranteed to be 1 to data.Count, effectiveAmount will just be 'amount'.
        // 3. Determine the number of elements to move from the end to the front. This is 'effectiveAmount'.
        // 4. Extract these 'moved' elements into a new temporary list using GetRange.
        // 5. Remove these elements from their original position at the end of the 'data' list using RemoveRange.
        // 6. Insert the 'moved' elements at the beginning of the 'data' list using InsertRange.

        // 1. Handle edge cases: If the list is null, empty, or has only one element, no rotation is needed.
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
        // This handles the TestRotateListRight_Rotate9() case.
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
