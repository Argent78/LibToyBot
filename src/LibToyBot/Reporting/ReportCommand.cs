using System;
using System.Collections.Generic;
using LibToyBot.Commands;
using LibToyBot.Outcomes;

namespace LibToyBot.Reporting
{
    internal class ReportCommand : ICommand
    {
        private readonly Stack<Call> _callStack;
        private readonly IPositionReporter _positionReporter;

        public ReportCommand(Stack<Call> callStack, IPositionReporter positionReporter)
        {
            _callStack = callStack;
            _positionReporter = positionReporter;
        }

        public void Execute(string[] cmdTokens = default)
        {
            var positionReport = _positionReporter.Report();
            Console.WriteLine(positionReport);

            //TODO: Move the outcome creation to the report generator?
            var outcome = new ReportOutcome(OutcomeStatus.Success, positionReport.ToString(), positionReport);
            _callStack.Push(new Call(this, outcome));
        }
    }
}
