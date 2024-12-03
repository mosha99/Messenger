namespace Share.Model;

public record UserMessage(SenderUser From, string Message, TimeOnly SendTime);
