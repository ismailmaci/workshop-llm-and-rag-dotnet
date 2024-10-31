using System.ComponentModel;
using Microsoft.SemanticKernel;

public class ExercisePlugin
{
    [KernelFunction, Description(@"Return exercises requested by the user.")]
    public string ListExercises()
    {
        var directory = Directory.GetCurrentDirectory();
        var exercises = File.ReadAllLines(Path.Combine(directory, "resources/exercises.txt"));
        var truncatedExercises = exercises.Skip(20);
        var flattenedExercises = string.Join(", ", truncatedExercises);
        return flattenedExercises;
    }

    [KernelFunction, Description(@"Return exercises requested by the user.")]
    public string ListRecentExercises()
    {
        var directory = Directory.GetCurrentDirectory();
        var exercises = File.ReadAllText(Path.Combine(directory, "resources/recentExercises.txt"));
        return exercises;
    }
}
