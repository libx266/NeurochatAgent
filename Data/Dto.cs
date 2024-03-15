using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public record MessageDto(long? Id, long? SenderId, long? ChatId, string? Text, DateTime? SenderDate);
    public record PromptMessageDto(string? FirstName, string? LastName, string Text);


    public record ChatDto(long? Id, string? Name);
    public record UserDto(long? Id, string? FirstName, string? LastName);
}
