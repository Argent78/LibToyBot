using LibToyBot.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace LibToyBot.Sample.NetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PlaceAndMoveRobot();
        }

        public static void PlaceAndMoveRobot()
        {
            var robot = GetRobot();
            robot.Action("PLACE 1,2,EAST");
            robot.Action("MOVE"); // 2,2,EAST
            robot.Action("LEFT"); // 2,2,NORTH
            robot.Action("MOVE");
            robot.Action("MOVE"); // 2,4,NORTH
            robot.Action("REPORT"); // Print position & orientation
        }

        private static Robot GetRobot()
        {
            return new ServiceCollection()
                .AddRobot()
                .BuildServiceProvider()
                .GetService<Robot>();
        }
    }
}
