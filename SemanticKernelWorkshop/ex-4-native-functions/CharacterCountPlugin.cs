using System.ComponentModel;
using Microsoft.SemanticKernel;

public class CharacterCountPlugin
{
    [KernelFunction, Description(@"Count the amount of 'r's' in a string.")]
    public Task<string> GetRCharacters(string input)
    {
        Console.WriteLine($"Received input: {input}");
        int count = input.Count(c => c == 'r');
        return Task.FromResult($"The amount of 'r's' in the input is: {count}");
    }
}
