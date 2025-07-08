using Microsoft.VisualStudio.TestTools.UnitTesting;

// TODO Problem 1 - Run test cases and record any defects the test code finds in the comment above the test method.
// DO NOT MODIFY THE CODE IN THE TESTS in this file, just the comments above the tests.
// Fix the code being tested to match requirements and make all tests pass.

[TestClass]
public class TakingTurnsQueueTests
{
    [TestMethod]
    // Scenario: Create a queue with the following people and turns: Bob (2), Tim (5), Sue (3) and
    // run until the queue is empty
    // Expected Result: Bob, Tim, Sue, Bob, Tim, Sue, Tim, Sue, Tim, Tim
    // Defect(s) Found:
    // 1. The PersonQueue.Enqueue method originally inserted elements at the beginning of the list,
    //    violating the FIFO principle of a queue. This resulted in an incorrect order of elements
    //    being dequeued (e.g., Sue, Tim, Bob instead of Bob, Tim, Sue). This has been corrected
    //    to use Add, which appends to the end.
    // 2. The TakingTurnsQueue.GetNextPerson method's logic for re-enqueuing people was incorrect.
    //    It only re-enqueued if 'person.Turns > 1'. This meant people with 0 or less turns (infinite turns)
    //    were not re-enqueued, and people with exactly 1 turn were also not correctly handled if they
    //    were supposed to be re-enqueued. The logic has been updated to re-enqueue if 'person.Turns <= 0'
    //    (for infinite turns) or if 'person.Turns > 1' (for finite turns, after decrementing).
    public void TestTakingTurnsQueue_FiniteRepetition()
    {
        var bob = new Person("Bob", 2);
        var tim = new Person("Tim", 5);
        var sue = new Person("Sue", 3);

        Person[] expectedResult = [bob, tim, sue, bob, tim, sue, tim, sue, tim, tim];

        var players = new TakingTurnsQueue();
        players.AddPerson(bob.Name, bob.Turns);
        players.AddPerson(tim.Name, tim.Turns);
        players.AddPerson(sue.Name, sue.Turns);

        int i = 0;
        while (players.Length > 0)
        {
            if (i >= expectedResult.Length)
            {
                Assert.Fail("Queue should have ran out of items by now.");
            }

            var person = players.GetNextPerson();
            Assert.AreEqual(expectedResult[i].Name, person.Name);
            i++;
        }
    }

    [TestMethod]
    // Scenario: Create a queue with the following people and turns: Bob (2), Tim (5), Sue (3)
    // After running 5 times, add George with 3 turns.  Run until the queue is empty.
    // Expected Result: Bob, Tim, Sue, Bob, Tim, Sue, Tim, George, Sue, Tim, George, Tim, George
    // Defect(s) Found:
    // 1. Same defects as TestTakingTurnsQueue_FiniteRepetition regarding PersonQueue's LIFO behavior
    //    (corrected to FIFO) and incorrect infinite/finite turns logic in TakingTurnsQueue.GetNextPerson
    //    (corrected to handle both cases).
    // 2. The original PersonQueue.Enqueue method added to the front, which affected the relative
    //    order of elements when new people (like George) were added midway. After correction to
    //    add to the back, the queue maintains the correct FIFO order for additions.
    public void TestTakingTurnsQueue_AddPlayerMidway()
    {
        var bob = new Person("Bob", 2);
        var tim = new Person("Tim", 5);
        var sue = new Person("Sue", 3);
        var george = new Person("George", 3);

        Person[] expectedResult = [bob, tim, sue, bob, tim, sue, tim, george, sue, tim, george, tim, george];

        var players = new TakingTurnsQueue();
        players.AddPerson(bob.Name, bob.Turns);
        players.AddPerson(tim.Name, tim.Turns);
        players.AddPerson(sue.Name, sue.Turns);

        int i = 0;
        for (; i < 5; i++)
        {
            var person = players.GetNextPerson();
            Assert.AreEqual(expectedResult[i].Name, person.Name);
        }

        players.AddPerson("George", 3);

        while (players.Length > 0)
        {
            if (i >= expectedResult.Length)
            {
                Assert.Fail("Queue should have ran out of items by now.");
            }

            var person = players.GetNextPerson();
            Assert.AreEqual(expectedResult[i].Name, person.Name);

            i++;
        }
    }

    [TestMethod]
    // Scenario: Create a queue with the following people and turns: Bob (2), Tim (Forever), Sue (3)
    // Run 10 times.
    // Expected Result: Bob, Tim, Sue, Bob, Tim, Sue, Tim, Sue, Tim, Tim
    // Defect(s) Found:
    // 1. The TakingTurnsQueue.GetNextPerson method did not correctly handle infinite turns (turns <= 0).
    //    Tim (with 0 turns) was not re-enqueued because the original condition 'person.Turns > 1' was not met.
    //    This has been fixed to re-enqueue if 'person.Turns <= 0' or 'person.Turns > 1'.
    // 2. The PersonQueue.Enqueue method added elements to the front, causing an incorrect initial order
    //    (fixed to add to the back).
    // 3. The 'Turns' property of 'Person' was incorrectly decremented even for infinite turns.
    //    The fix ensures that 'person.Turns' is only decremented if 'person.Turns > 0',
    //    preserving the infinite turn value.
    public void TestTakingTurnsQueue_ForeverZero()
    {
        var timTurns = 0;

        var bob = new Person("Bob", 2);
        var tim = new Person("Tim", timTurns);
        var sue = new Person("Sue", 3);

        Person[] expectedResult = [bob, tim, sue, bob, tim, sue, tim, sue, tim, tim];

        var players = new TakingTurnsQueue();
        players.AddPerson(bob.Name, bob.Turns);
        players.AddPerson(tim.Name, tim.Turns);
        players.AddPerson(sue.Name, sue.Turns);

        for (int i = 0; i < 10; i++)
        {
            var person = players.GetNextPerson();
            Assert.AreEqual(expectedResult[i].Name, person.Name);
        }

        // Verify that the people with infinite turns really do have infinite turns.
        var infinitePerson = players.GetNextPerson();
        Assert.AreEqual(timTurns, infinitePerson.Turns, "People with infinite turns should not have their turns parameter modified to a very big number. A very big number is not infinite.");
    }

    [TestMethod]
    // Scenario: Create a queue with the following people and turns: Tim (Forever), Sue (3)
    // Run 10 times.
    // Expected Result: Tim, Sue, Tim, Sue, Tim, Sue, Tim, Tim, Tim, Tim
    // Defect(s) Found:
    // 1. The TakingTurnsQueue.GetNextPerson method did not correctly handle infinite turns (turns <= 0).
    //    Tim (with -3 turns) was not re-enqueued because the original condition 'person.Turns > 1' was not met.
    //    This has been fixed to re-enqueue if 'person.Turns <= 0' or 'person.Turns > 1'.
    // 2. The PersonQueue.Enqueue method added elements to the front, causing an incorrect initial order
    //    (fixed to add to the back).
    // 3. The 'Turns' property of 'Person' was incorrectly decremented even for infinite turns.
    //    The fix ensures that 'person.Turns' is only decremented if 'person.Turns > 0',
    //    preserving the infinite turn value.
    public void TestTakingTurnsQueue_ForeverNegative()
    {
        var timTurns = -3;
        var tim = new Person("Tim", timTurns);
        var sue = new Person("Sue", 3);

        Person[] expectedResult = [tim, sue, tim, sue, tim, sue, tim, tim, tim, tim];

        var players = new TakingTurnsQueue();
        players.AddPerson(tim.Name, tim.Turns);
        players.AddPerson(sue.Name, sue.Turns);

        for (int i = 0; i < 10; i++)
        {
            var person = players.GetNextPerson();
            Assert.AreEqual(expectedResult[i].Name, person.Name);
        }

        // Verify that the people with infinite turns really do have infinite turns.
        var infinitePerson = players.GetNextPerson();
        Assert.AreEqual(timTurns, infinitePerson.Turns, "People with infinite turns should not have their turns parameter modified to a very big number. A very big number is not infinite.");
    }

    [TestMethod]
    // Scenario: Try to get the next person from an empty queue
    // Expected Result: Exception should be thrown with appropriate error message.
    // Defect(s) Found: None. This test case passes with the current implementation,
    // as the empty queue check and exception throwing are correctly implemented.
    public void TestTakingTurnsQueue_Empty()
    {
        var players = new TakingTurnsQueue();

        try
        {
            players.GetNextPerson();
            Assert.Fail("Exception should have been thrown.");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("No one in the queue.", e.Message);
        }
        catch (AssertFailedException)
        {
            throw;
        }
        catch (Exception e)
        {
            Assert.Fail(
                     string.Format("Unexpected exception of type {0} caught: {1}",
                                     e.GetType(), e.Message)
            );
        }
    }
}