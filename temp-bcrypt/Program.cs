using System;
class Program {
    static void Main() {
        var hash = BCrypt.Net.BCrypt.HashPassword("Employee@123");
        Console.WriteLine(hash);
    }
}
