using System.Collections.Generic;
using LibToyBot.Commands;
using LibToyBot.Movement;
using LibToyBot.Reporting;
using LibToyBot.Spatial;

namespace LibToyBot
{
    public static class ToyBox
    {
        private static Robot _robot;

        public static Robot Robot => GetRobot();

        private static Robot GetRobot()
        {
            if (_robot != null) return _robot;
            
            var callStack = new Stack<Call>();
            var boundsEvaluator = new BoundsEvaluator(new Table());
            var positionTracker = new PositionTracker();
            var movementProcessor = new MovementProcessor(boundsEvaluator, positionTracker);
            var placeCommand = new PlaceCommand(callStack, movementProcessor);
            var moveCommand = new MoveCommand(callStack, movementProcessor);
            var turnCommand = new TurnCommand(callStack, movementProcessor);
            var positionReporter = new PositionReporter(positionTracker);
            var reportCommand = new ReportCommand(callStack, positionReporter);
            var commandExecutor = new CommandExecutor(
                callStack,
                placeCommand,
                moveCommand,
                turnCommand,
                reportCommand);
            _robot = new Robot(commandExecutor);

            return _robot;
        }
    }
}
