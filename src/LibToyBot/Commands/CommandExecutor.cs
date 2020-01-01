using System;
using System.Collections.Generic;
using System.Linq;
using LibToyBot.Movement;
using LibToyBot.Outcomes;
using LibToyBot.Reporting;

namespace LibToyBot.Commands
{
    /// <summary>A parser for processing string input and converting to a command</summary>
    internal class CommandExecutor : ICommandExecutor
    {
        private readonly Stack<Call> _callStack;
        private readonly PlaceCommand _placeCommand;
        private readonly MoveCommand _moveCommand;
        private readonly TurnCommand _turnCommand;
        private readonly ReportCommand _reportCommand;

        public CommandExecutor(Stack<Call> callStack,
            PlaceCommand placeCommand, 
            MoveCommand moveCommand, 
            TurnCommand turnCommand, 
            ReportCommand reportCommand)
        {
            _callStack = callStack;
            _placeCommand = placeCommand;
            _moveCommand = moveCommand;
            _turnCommand = turnCommand;
            _reportCommand = reportCommand;


//            _callStack = new Stack<Call>();
        }

        /// <summary>
        ///   <para>Parse and execute strings that contain commands. Returns an ICommand instance of the corresponding command</para>
        ///   <para>Available commands are:</para>
        ///   <para>PLACE X,Y,DIRECTION
        ///   MOVE
        ///   LEFT
        ///   RIGHT
        ///   REPORT
        ///   </para> 
        /// </summary>
        public IOutcome ExecuteCommand(string commandText)
        {
            try
            {
                var sanitizedCommandText = Sanitize(commandText);
                // tokenize the command string on whitespace
                var cmdTokens = sanitizedCommandText.Split(' ');

                // switch over the first token
                var firstToken = cmdTokens.FirstOrDefault();
                Enum.TryParse<RobotCommand>(firstToken, out var commandEnum);

                ICommand command = commandEnum switch
                {
                    RobotCommand.PLACE => _placeCommand,
                    RobotCommand.MOVE => _moveCommand,
                    RobotCommand.LEFT => _turnCommand,
                    RobotCommand.RIGHT => _turnCommand,
                    RobotCommand.REPORT => _reportCommand,
                    _ => throw new ArgumentOutOfRangeException()
                };

                command.Execute(cmdTokens);
                var commandCall = _callStack.Peek();
                return commandCall.Outcome;
            }
            catch (Exception)
            {
                return new ActionOutcome(OutcomeStatus.Fail, "The command is invalid");
            }
        }

        private static string Sanitize(string commandText)
        {
            return commandText.ToUpperInvariant();
        }
    }
}