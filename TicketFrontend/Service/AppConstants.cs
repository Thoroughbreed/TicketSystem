namespace TicketFrontend.Service;

public static class AppConstants
{
    private static readonly string _port = "8443";
    private static readonly Uri BaseAddress = new Uri($"https://localhost:{_port}/");
    public  static readonly string _statusURL = $"{BaseAddress}status";
    public  static readonly string _priorityURL  = $"{BaseAddress}priority";
    public  static readonly string _categoryURL = $"{BaseAddress}category";
    public  static readonly string _userURL = $"{BaseAddress}users";
    public  static readonly string _roleURL = $"{BaseAddress}roles";
    public  static readonly string _ticketUrl = $"{BaseAddress}tickets";
    public  static readonly string _commentUrl = $"{BaseAddress}comments";
    public  static readonly string _changelogUrl  = $"{BaseAddress}changelog";
}