using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

internal sealed class SupportContextProvider : MessageAIContextProvider
{
    protected override ValueTask<IEnumerable<ChatMessage>> ProvideMessagesAsync(
        InvokingContext context,
        CancellationToken cancellationToken = default)
    {
        Console.WriteLine("\nCONTEXT PROVIDER\nAdding support information to the model context.");

        // This value is hardcoded only to keep the sample self-contained.
        // A real provider could load current data from a database, API, user profile,
        // permissions service, or any other application source on every invocation.
        return ValueTask.FromResult<IEnumerable<ChatMessage>>(
        [
            new ChatMessage(
                ChatRole.User,
                "Support is available Monday to Friday from 09:00 to 17:00 UK time.")
        ]);
    }
}
