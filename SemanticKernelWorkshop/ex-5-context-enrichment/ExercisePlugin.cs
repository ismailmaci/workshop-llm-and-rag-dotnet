using System.ComponentModel;
using Microsoft.SemanticKernel;

public class ExercisePlugin
{
    [KernelFunction, Description(@"Return exercises requested by the user.")]
    public string ListExercises()
    {
        var directory = Directory.GetCurrentDirectory();
        var exercises = File.ReadAllText(Path.Combine(directory, "resources/exercises.txt"));
        return exercises;
    }

    [KernelFunction, Description(@"Return exercises requested by the user.")]
    public string ListRecentExercises()
    {
        var directory = Directory.GetCurrentDirectory();
        var exercises = File.ReadAllText(Path.Combine(directory, "resources/recentExercises.txt"));
        return exercises;
    }
}
