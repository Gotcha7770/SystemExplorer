using Xunit;

namespace SystemExplorer.Tests.Directories;

public class CreateDirectoryTest
{
    [Fact]
    public void CreateDirectoryTest_Success()
    {
        // ARRANGE
            /// Создать VMFactory -> Получить комманду CreateDirectoryCommand

        string[] pathes = new[] 
        {
            @"D:\test_folder\папка",
            @"D:\test_folder",
            @"D:\",
            string.Empty
        };
        // ACT 


        // ASSERT
        
    }
}
